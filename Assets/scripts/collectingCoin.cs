using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectingCoin : MonoBehaviour
{
    private GameManager gameManager;
    public int coin;
    // Start is called before the first frame update
    void Start()
    {
        coin = PlayerPrefs.GetInt("currency");
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "coin")
        {
            Debug.Log("coin collected");
            coin = PlayerPrefs.GetInt("currency") + 100; // Lấy giá trị hiện tại và cộng thêm 100
            PlayerPrefs.SetInt("currency", coin);

            // Kiểm tra nếu gameManager đã được khởi tạo

            gameManager.score += 100;
            Debug.Log(gameManager.score);


            col.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
