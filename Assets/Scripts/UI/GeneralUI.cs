using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{
    [SerializeField]
    private TapToStartPanel TapToStartPanel;
    [SerializeField]
    private GameObject LevelSelectPanel;

    private void Start()
    {
        LevelSelectPanel.SetActive(true);
        TapToStartPanel.gameObject.SetActive(false);

        GameController.instance.OnNewStageOpen += OnNewStageOpen;
    }
    private void OnApplicationQuit()
    {
        GameController.instance.OnNewStageOpen -= OnNewStageOpen;
    }

    private void OnNewStageOpen(Stage stage)
    {
        LevelSelectPanel.SetActive(false);
        TapToStartPanel.OpenTapToStartPanel(stage.stageNo);
    }

    public void  LevelSelectPanelSetEnable(bool enable)
    {
        LevelSelectPanel.SetActive(enable);
    }
}
