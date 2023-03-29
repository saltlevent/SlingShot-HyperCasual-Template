using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentLevel;
    [SerializeField]
    private TextMeshProUGUI nextLevel;
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private TextMeshProUGUI percentText;
    [SerializeField]
    private TextMeshProUGUI availableShotText;

    private void Start()
    {
        StageController.instance.OnScoreChanged += OnScoreChanged;
        CharacterManager.instance.OnCharacterPhaseChanged += OnCharacterPhaseChanged;
    }

    private void OnCharacterPhaseChanged(CharacterManager.CharacterPhase phase)
    {
        if(phase == CharacterManager.CharacterPhase.Flying)
        {
            if (CharacterManager.instance.availableCount > 1)
            {
                availableShotText.text = $"{CharacterManager.instance.availableCount-1} atýþ Kaldý";
            }
            else
            {
                availableShotText.text = "Atýþ Hakký Kalmadý";
            }
        }
    }

    private void OnScoreChanged(int score)
    {
        var maxScore = StageController.instance.maxScore;
        progressPercent((float)score/(float)maxScore);
        Debug.Log($"score:{score}, maxScore{maxScore}, ratio{(float)score / (float)maxScore}");
    }


    public void SetPanelView(int stageNo)
    {
        currentLevel.text = stageNo.ToString();
        nextLevel.text = (stageNo + 1).ToString();
        progressPercent(0);
    }

    /// <summary>
    /// Progress bar indicator'ü.
    /// </summary>
    /// <param name="ratio"> 0 ile 1 arasýnda sayý</param>
    private void progressPercent(float ratio)
    {
        progressImage.fillAmount = ratio;
        percentText.text = $"{Mathf.Round(ratio * 100)}%";
    }
}
