using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public vehicle Vehicle;
    public vehicleList list;
    public controller RR;
    public GameObject neeedle;
    public GameObject startPosition;
    public Text kph;
    public Text currentPosition;
    public Text gearNum;
    public Slider nitrusSlider;
    private float startPosiziton = 32f, endPosition = -211f;
    private float desiredPosition;
    private GameObject[] presentGameObjectVehicles, fullArray;

    [Header("countdown Timer")]
    public float timeLeft = 4;
    public Text timeLeftText;
    public TextMeshProUGUI coin;


    [Header("racers list")]
    public GameObject uiList;
    public GameObject uiListFolder;
    public GameObject backImage;
    private List<vehicle> presentVehicles;
    private List<GameObject> temporaryList;
    private GameObject[] temporaryArray;

    private int highScore = 0;
    private int startPositionXvalue = -50 - 62;
    private bool arrarDisplayed = false, countdownFlag = false;
    public GameObject pauseCanvas;
    private bool isPaused = false;
    public TextMeshProUGUI scoreText;
    private float scoreTimer = 0f;
    private float scoreInterval = 1f; // Cập nhật điểm số mỗi giây
    public int score = 0;
    public GameObject finishCanvas;
    public GameObject currentCanvas;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // private void UpdateHighScoreText()
    // {
    //     highScoreText.text = "High Score: " + highScore;
    // }
    public void Pause()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void Continue()
    {

        isPaused = false;
        finishCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneName);
    }
    private void Awake()
    {

        Instantiate(list.vehicles[PlayerPrefs.GetInt("pointer")], startPosition.transform.position, startPosition.transform.rotation);
        RR = GameObject.FindGameObjectWithTag("Player").GetComponent<controller>();

        presentGameObjectVehicles = GameObject.FindGameObjectsWithTag("AI");

        presentVehicles = new List<vehicle>();
        foreach (GameObject R in presentGameObjectVehicles)
        {
            inputManager inputMgr = R.GetComponent<inputManager>();
            controller ctrl = R.GetComponent<controller>();

            if (inputMgr != null && ctrl != null)
            {
                presentVehicles.Add(new vehicle(inputMgr.currentNode, ctrl.carName, ctrl.hasFinished));
            }

        }

        presentVehicles.Add(new vehicle(RR.gameObject.GetComponent<inputManager>().currentNode, RR.carName, RR.hasFinished));

        temporaryArray = new GameObject[presentVehicles.Count];

        temporaryList = new List<GameObject>();
        foreach (GameObject R in presentGameObjectVehicles)
            temporaryList.Add(R);
        temporaryList.Add(RR.gameObject);

        fullArray = temporaryList.ToArray();

        // displayArray();
        // displayFinish();
        StartCoroutine(timedLoop());
    }


    private void FixedUpdate()
    {
        if (RR.hasFinished) displayFinish();//displayArray();
        kph.text = RR.KPH.ToString("0");
        coin.text = PlayerPrefs.GetInt("currency").ToString();
        if (!isPaused)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreInterval)
            {
                score += 1; // Tăng 1 điểm
                UpdateScoreText(); // Cập nhật UI hiển thị điểm số
                scoreTimer = 0f; // Reset timer
            }
        }
        updateNeedle();
        nitrusUI();
        coundDownTimer();
    }
    public void displayFinish()
    {
        Debug.Log("xong");
        Time.timeScale = 0;
        currentCanvas.SetActive(false);
        finishCanvas.SetActive(true);
    }
    public void updateNeedle()
    {
        desiredPosition = startPosiziton - endPosition;
        float temp = RR.engineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3(0, 0, (startPosiziton - temp * desiredPosition));

    }

    public void changeGear()
    {
        gearNum.text = (!RR.reverse) ? (RR.gearNum + 1).ToString() : "R";

    }

    public void nitrusUI()
    {
        nitrusSlider.value = RR.nitrusValue / 45;
    }

    private void sortArray()
    {

        for (int i = 0; i < fullArray.Length; i++)
        {
            presentVehicles[i].hasFinished = fullArray[i].GetComponent<controller>().hasFinished;
            presentVehicles[i].name = fullArray[i].GetComponent<controller>().carName;
            presentVehicles[i].node = fullArray[i].GetComponent<inputManager>().currentNode;
        }
        if (!RR.hasFinished)
            for (int i = 0; i < presentVehicles.Count; i++)
            {
                for (int j = i + 1; j < presentVehicles.Count; j++)
                {
                    if (presentVehicles[j].node < presentVehicles[i].node)
                    {
                        vehicle QQ = presentVehicles[i];
                        presentVehicles[i] = presentVehicles[j];
                        presentVehicles[j] = QQ;
                    }
                }
            }


        if (arrarDisplayed)
            for (int i = 0; i < temporaryArray.Length; i++)
            {
                temporaryArray[i].transform.Find("vehicle node").gameObject.GetComponent<Text>().text = (presentVehicles[i].hasFinished) ? "finished" : "";
                temporaryArray[i].transform.Find("vehicle name").gameObject.GetComponent<Text>().text = presentVehicles[i].name.ToString();
            }
        presentVehicles.Reverse();
        for (int i = 0; i < temporaryArray.Length; i++)
        {
            if (RR.carName == presentVehicles[i].name)
                currentPosition.text = ((i + 1) + "/" + presentVehicles.Count).ToString();
        }



    }
    // private void displayArray()
    // {
    //     if (arrarDisplayed) return;
    //     uiList.SetActive(true);
    //     for (int i = 0; i < presentVehicles.Count; i++)
    //     {
    //         generateList(i, presentVehicles[i].hasFinished, presentVehicles[i].name);
    //     }

    //     startPositionXvalue = -50;
    //     arrarDisplayed = true;
    //     backImage.SetActive(true);
    // }

    // private void generateList(int index, bool num, string nameValue)
    // {

    //     temporaryArray[index] = Instantiate(uiList);
    //     temporaryArray[index].transform.parent = uiListFolder.transform;
    //     //temporaryArray[index].gameObject.GetComponent<RectTransform>().localScale = new Vector3(2,2,2);
    //     temporaryArray[index].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startPositionXvalue);
    //     //temporaryArray[index].transform.position = new Vector3(0,startPositionXvalue,0);
    //     temporaryArray[index].transform.Find("vehicle name").gameObject.GetComponent<Text>().text = "Phat";//nameValue.ToString();
    //     temporaryArray[index].transform.Find("vehicle node").gameObject.GetComponent<Text>().text = (num) ? "finished" : "";
    //     startPositionXvalue += 50;

    // }

    private IEnumerator timedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            sortArray();
        }
    }

    private void coundDownTimer()
    {
        if (timeLeft <= -5) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) unfreezePlayers();
        else freezePlayers();

        if (timeLeft > 1) timeLeftText.text = timeLeft.ToString("0");
        else if (timeLeft >= -1 && timeLeft <= 1) timeLeftText.text = "GO!";
        else timeLeftText.text = "";

    }

    private void freezePlayers()
    {

        if (countdownFlag) return;

        foreach (GameObject element in fullArray)
        {
            // Debug.Log("Phần tử trong fullArray: " + element.name);

        }
        countdownFlag = true;

    }

    private void unfreezePlayers()
    {
        if (!countdownFlag) return;
        foreach (GameObject D in fullArray)
        {
            D.GetComponent<Rigidbody>().isKinematic = false;
        }
        countdownFlag = false;

    }

    public void loadAwakeScene()
    {
        finishCanvas.SetActive(false);
        Debug.Log("load scene");
        Time.timeScale = 1;
        SceneManager.LoadScene("awakeScene");


    }
}