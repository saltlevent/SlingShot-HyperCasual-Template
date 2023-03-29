using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFinishedPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Title;
    [SerializeField]
    private Button NextLevelButton;


    public void setView(bool success)
    {
        gameObject.SetActive(true);
        if(success)
        {
            if(StageController.instance.maxStageNo == StageController.instance.currentStage.stageNo)
            {
                Title.text = $"Oyundaki tüm seviyeleri bitirdiniz tebrikler";
                NextLevelButton.gameObject.SetActive(false);
            }
            else
            {
                Title.text = $"Bir sonraki seviyeye geçme hakký kazandýnýz.";
                NextLevelButton.gameObject.SetActive(true);
            }
        }
        else
        {
            Title.text = $"Baþarýlý olamadýnýz tekrar deneyin.";
            NextLevelButton.gameObject.SetActive(false);
        }
    }

    public void OpenLevelSelectPanel()
    {
        UIController.instance.GetGeneralUI().LevelSelectPanelSetEnable(true);
        gameObject.SetActive(false);
    }

    public void RestartStage()
    {
        StageController.instance.LoadStage(StageController.instance.currentStage.stageNo);
        gameObject.SetActive(false);
    }

    public void GoNextStage()
    {
        StageController.instance.LoadStage(StageController.instance.currentStage.stageNo+1);
        gameObject.SetActive(false);
    }

}
