using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : MonoBehaviour
{
    public GameObject LevelCard;
    public RectTransform Area;


    private readonly Color orangeColor = new Color(1, 0.396876f, 0);
    private readonly Color greyColor = new Color(0.3882353f, 0.372549f, 0.3411765f);

    private void OnEnable()
    {
        ClearView();
        var data = StageController.instance.getAllStagesData();

        foreach (var item in data.stages)
        {
            var tempGameObject = Instantiate(LevelCard, Area);

            tempGameObject.GetComponent<LevelCard>().stageNo = item.stageNo;

            if (StageController.instance.isAvailableStage(item.stageNo))
                tempGameObject.GetComponent<LevelCard>().SetView(orangeColor, item.stageNo, true);
            else
                tempGameObject.GetComponent<LevelCard>().SetView(Color.grey, item.stageNo, false);
        }
    }

    private void ClearView()
    {
        var childButtons = Area.GetComponentsInChildren<Button>();
        if (childButtons.Length > 0)
        {
            for (int i = childButtons.Length - 1; i >= 0; i--)
            {
                Destroy(childButtons[i].gameObject);
            }
        }


    }
}
