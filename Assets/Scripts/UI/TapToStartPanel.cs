using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TapToStartPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    public void OpenTapToStartPanel(int stageNo)
    {
        gameObject.SetActive(true);
        levelText.text = $"Level {stageNo}";

        GameController.instance.setGamePhase(GameController.GamePhase.Game);
        CharacterManager.instance.setCharacterPhase(CharacterManager.CharacterPhase.Preparing);
    }
}
