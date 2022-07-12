using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public enum PanelNames
{
    MainMenu,
    InGame,
    WinPanel,
    LosePanel,
}

[System.Serializable]
public class UIPanels : SerializableDictionaryBase<PanelNames, UIPanelAndSetup> { }

[System.Serializable]
public class UIPanelAndSetup
{
    public GameObject UIPanel;
    public UnityEvent UIPanelSetup;
}

public class UIManager : MonoBehaviour
{
    public UIPanels UIPanelsDictionary;

    public static UIManager Instance;

    [Header("Collectible Items")]
    public GameObject collectibleUI;
    public Transform collectibleHolder;
    private struct CollectibleObject
    {
        public CollectibleName name;
        public GameObject go;
        public CollectibleObject(CollectibleName _collectibleName, GameObject _go)
        {
            this.name = _collectibleName;
            this.go = _go;
        }
    }

    private List<CollectibleObject> collectibleObjects;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        OpenPanel(PanelNames.MainMenu);
    }

    private void Start()
    {
        collectibleObjects = new List<CollectibleObject>();
        foreach (var item in CollectibleManager.Instance.collectibles)
        {
            var go = Instantiate(collectibleUI, collectibleHolder);
            go.GetComponentInChildren<TMP_Text>().text = item.Value.CollectibleCount.ToString();
            go.GetComponentInChildren<Image>().sprite = item.Value.CollectibleIcon;

            CollectibleObject collectible = new CollectibleObject(item.Key, go);
            collectibleObjects.Add(collectible);
        }
    }

    public GameObject GetCollectibleIcon(CollectibleName _name)
    {
        GameObject temp = null;
        foreach (var item in collectibleObjects)
        {
            if(item.name == _name)
            {
                temp = item.go.GetComponentInChildren<Image>().gameObject;
            }
        }

        return temp;
    }

    public void SetCollectibleCount(CollectibleName _name, int count)
    {
        TMP_Text temp = null;
        foreach (var item in collectibleObjects)
        {
            if (item.name == _name)
            {
                temp = item.go.GetComponentInChildren<TMP_Text>();
            }
        }

        temp.text = count.ToString();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelOnClicked()
    {
        FindObjectOfType<DemoController>().StartNextLevel();
    }

    #region On Panel Opened Actions

    public void SetupInGamePanel()
    {
        ClosePanel(PanelNames.MainMenu);
        ClosePanel(PanelNames.WinPanel);
    }

    #endregion


    #region Panel Functions

    public void OpenPanel(string panel)
    {
        PanelNames panelName;
        if (Enum.TryParse<PanelNames>(panel, out panelName))
            OpenPanel(panelName);
        else
            Debug.LogWarning("Did not find panel: " + panel);
    }

    public void OpenPanel(PanelNames panelName, bool closeOtherPanels)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {

            if (closeOtherPanels)
            {
                CloseAllPanels();
            }

            panelToOpen.UIPanel.SetActive(true);

            if (panelToOpen.UIPanelSetup != null)
            {
                panelToOpen.UIPanelSetup.Invoke();
            }

        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }

    }


    public void OpenPanel(PanelNames[] names, bool closeOtherPanels)
    {
        if (closeOtherPanels)
            CloseAllPanels();

        foreach (PanelNames panelName in names)
            OpenPanel(panelName, false);
    }

    public void OpenPanel(PanelNames name, bool closeOtherPanels, float delay)
    {
        if (closeOtherPanels)
            CloseAllPanels();

        StartCoroutine(AddDelay(delay, () => { OpenPanel(name, closeOtherPanels); }));
    }

    public void OpenPanel(PanelNames panelName)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {
            panelToOpen.UIPanel.SetActive(true);
            panelToOpen.UIPanelSetup?.Invoke();
        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }

    }

    public void ClosePanel(string panel)
    {
        PanelNames panelName;
        if (!Enum.TryParse<PanelNames>(panel, out panelName))
        {
            Debug.LogWarning("No enum for string: " + panel);
            return;
        }

        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }

    public void ClosePanel(PanelNames panelName)
    {
        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }


    public void CloseAllPanels()
    {
        foreach (PanelNames panelName in UIPanelsDictionary.Keys)
            ClosePanel(panelName);
    }

    IEnumerator AddDelay(float xSeconds, UnityAction Action)
    {
        yield return new WaitForSecondsRealtime(xSeconds);
        Action();
    }

    #endregion

}

