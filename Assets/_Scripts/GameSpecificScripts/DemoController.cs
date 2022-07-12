using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    [SerializeField] private GameObject finishPlatformPrefab;
    private bool controlsEnabled = false;
    private bool isStarted = false;
    private PlatformManager platformManager;
    private Chibi chibi;

    [Header("Level Parameters")]
    private int platformlLevel = 0;
    private float finishZStartPos = 20;
    private float tweenTime = 3.5f;


    private void Start()
    {
        platformManager = FindObjectOfType<PlatformManager>();
        chibi = FindObjectOfType<Chibi>();
        SetLevelParameters();

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

            Debug.LogWarning(chibi.transform.position.z);

            if(chibi.transform.position.z > GetFinishPlatformZPos())
            {
                //win
                chibi.Dance();
                controlsEnabled = false;
                UIManager.Instance.OpenPanel(PanelNames.WinPanel);
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
        chibi.StartToRun(tweenTime * 0.375f);
        UIManager.Instance.CloseAllPanels();
        UIManager.Instance.OpenPanel(PanelNames.InGame);
    }

    private void SetLevelParameters()
    {
        var go = Instantiate(finishPlatformPrefab, transform);
        go.transform.position = new Vector3(0, 0, GetFinishPlatformZPos());
        tweenTime -= (float)platformlLevel / 5;
        platformManager.SetLevelParameters(GetFinishPlatformZPos(), tweenTime);
    }

    public float GetFinishPlatformZPos()
    {
        return finishZStartPos + (finishZStartPos * (1.2f + (float)platformlLevel / 10) * platformlLevel);
    }
}
