using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField]
    private GeneralUI generalUI;
    [SerializeField]
    private InGameUI inGameUI;

    private void Awake()
    {
        instance = this;
    }

    public void CloseGeneralUI()
    {
        generalUI.gameObject.SetActive(false);
    }
    public void CloseInGameUI()
    {
        inGameUI.gameObject.SetActive(false);
    }

    public GeneralUI GetGeneralUI()
    {
        return generalUI;
    }

    public InGameUI GetInGameUI()
    {
        return inGameUI;
    }
}
