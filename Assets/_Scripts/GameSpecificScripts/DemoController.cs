using UnityEngine;
using DG.Tweening;

public class DemoController : MonoBehaviour
{
    [SerializeField] private GameObject finishPlatformPrefab;
    [SerializeField] private GameObject[] collectiblesPrefab;
    [SerializeField] private GameObject[] backgrounds;


    private bool controlsEnabled = false;
    private bool isStarted = false;
    private PlatformManager platformManager;
    private Chibi chibi;

    [Header("Level Parameters")]
    private int platformlLevel = 0;
    private float finishZStartPos = 40;
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

            if(chibi.transform.position.z > GetFinishPlatformZPos())
            {
                chibi.Dance();
                controlsEnabled = false;
                UIManager.Instance.OpenPanel(PanelNames.WinPanel);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isStarted)
            {
                Start2Run();
            }
        }
    }

    public void StartNextLevel()
    {
        platformManager.CreateFullSize();
        platformlLevel++;
        SetLevelParameters();
        SoundManager.Instance.ResetCombo();
        Start2Run();
    }

    private void Start2Run()
    {
        isStarted = true;
        controlsEnabled = true;
        platformManager.CreateNewActive();
        chibi.StartToRun(tweenTime * 0.38f);
        UIManager.Instance.CloseAllPanels();
        UIManager.Instance.OpenPanel(PanelNames.InGame);
        CreateCollectibles();
    }

    private void SetLevelParameters()
    {
        var go = Instantiate(finishPlatformPrefab, transform);
        go.transform.position = new Vector3(0, 0.03f, GetFinishPlatformZPos());
        tweenTime -= (float)platformlLevel / 5;
        platformManager.SetLevelParameters(GetFinishPlatformZPos(), tweenTime);

        foreach (var item in backgrounds)
        {
            if (item.transform.position.z + 600 < chibi.transform.position.z)
            {
                var pos = item.transform.position;
                pos.z += 1800;
                item.transform.position = pos;
            }

        }
    }

    private void CreateCollectibles()
    {
        for (int i = (int)chibi.transform.position.z + 3; i < GetFinishPlatformZPos(); i++)
        {
            var rand = Random.Range(0f, 1f);
            if (rand < 0.25f)
                Instantiate(collectiblesPrefab[Random.Range(0, 3)], new Vector3(0, 0.2f, i), Quaternion.identity, transform);
        }
    }

    public float GetFinishPlatformZPos()
    {
        return finishZStartPos + (finishZStartPos * (1.2f + (float)platformlLevel / 10) * platformlLevel);
    }
}
