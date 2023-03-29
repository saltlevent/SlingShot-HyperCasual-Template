using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public ProgressPanel progressPanel;
    public GameFinishedPanel gameFinishedPanel;

    private void Start()
    {
        progressPanel.gameObject.SetActive(false);
        GameController.instance.OnGameFinished += OnGameFinished;
        GameController.instance.OnGamePhaseChanged += OnGamePhaseChanged; ;
    }

    private void OnGamePhaseChanged(GameController.GamePhase obj)
    {
        if (obj == GameController.GamePhase.Preparing)
        {
            progressPanel.gameObject.SetActive(true);
            progressPanel.GetComponent<ProgressPanel>().SetPanelView(StageController.instance.currentStage.stageNo);
        }
    }

    private void OnGameFinished(bool success)
    {
        gameFinishedPanel.setView(success);
    }

    private void OnApplicationQuit()
    {
        GameController.instance.OnGameFinished -= OnGameFinished;
        GameController.instance.OnGamePhaseChanged -= OnGamePhaseChanged;
    }

}
