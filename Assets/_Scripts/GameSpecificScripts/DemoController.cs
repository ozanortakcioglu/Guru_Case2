using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    private bool controlsEnabled = false;
    private bool isStarted = false;
    private PlatformManager platformManager;
    private Chibi chibi;

    private void Start()
    {
        platformManager = FindObjectOfType<PlatformManager>();
        chibi = FindObjectOfType<Chibi>();
    }

    private void Update()
    {
        if (controlsEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                platformManager.PlaceActivePiece();
            }

            if (!platformManager.isOnPlatform(chibi.gameObject.transform.position.z - 0.1f)) //fail
            {
                chibi.Fall();
                controlsEnabled = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isStarted)
            {
                FirstTouch();
            }
        }
    }

    private void FirstTouch()
    {
        isStarted = true;
        controlsEnabled = true;
        platformManager.CreateNewActive();
        chibi.StartToRun(platformManager.GetTweenTime() * 0.4f);
    }
}
