using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI level;

    public int stageNo = 0;
    public bool available = false;

    public void SetView(Color color, int stageNo,bool available)
    {
        SetColor(color);
        SetLevel(stageNo.ToString());

        this.stageNo = stageNo;
        this.available = available;

        GetComponent<Button>().interactable = available;
    }

    private void SetColor(Color color)
    {
        image.color = color;
    }

    private void SetLevel(string levelNo)
    {
        level.text = levelNo;
    }

    public void GoToLevel()
    {
        StageController.instance.LoadStage(stageNo);
    }
}
