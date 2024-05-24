using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    private GameObject tppPlayer;
    private GameObject fppPlayer;
    private CinemachineVirtualCamera virtualCamera;
    private CameraState currentState = CameraState.TPP;

    private enum CameraState
    {
        TPP,
        FPP
    }

    private void Start()
    {
        getObject();
        if (tppPlayer != null && fppPlayer != null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            SwitchToFPP();
        }
        else
        {
            Debug.LogError("tppPlayer or fppPlayer not found!");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraState();
        }
    }
    private void SwitchCameraState()
    {
        switch (currentState)
        {
            case CameraState.TPP:
                SwitchToFPP();
                break;
            case CameraState.FPP:
                SwitchToTPP();
                break;
        }
    }
    private void getObject()
    {
        tppPlayer = GameObject.FindGameObjectWithTag("TPP");
        fppPlayer = GameObject.FindGameObjectWithTag("FPP");
    }

    private void SwitchToTPP()
    {
        if (tppPlayer != null)
        {
            virtualCamera.Follow = tppPlayer.transform;
            virtualCamera.LookAt = tppPlayer.transform;
            currentState = CameraState.TPP;
        }
        else
        {
            Debug.LogError("tppPlayer not found!");
        }
    }

    private void SwitchToFPP()
    {
        if (fppPlayer != null)
        {
            virtualCamera.Follow = fppPlayer.transform;
            virtualCamera.LookAt = fppPlayer.transform;
            currentState = CameraState.FPP;
        }
        else
        {
            Debug.LogError("fppPlayer not found!");
        }
    }
}