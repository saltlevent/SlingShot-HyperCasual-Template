using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public static StageController instance;
    [SerializeField]
    private Transform stageStartTransform;

    public GameObject currentStagePrefab { get; private set; }
    public Stage currentStage { get; private set; }

    private const string JsonPath = "json/stages";
    private const string StagePath = "stages";

    private int _score = 0;
    public int score { get { return _score; } private set { _score = value; OnScoreChanged.Invoke(value); } }
    public int maxScore { get; private set; }

    public int maxStageNo { get; private set; }

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        instance = this;
        maxScore = 0;

#if UNITY_EDITOR
        if (stageStartTransform == null)
        {
            Debug.LogError("sahnede herhangi bir stagePosition yok.");
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }

    private void Start()
    {
        //Oyuna ilk girecekler için
        if (!isAvailableStage(0))
            validateStage(0);
    }

    public Stage getStageData(int stageNo)
    {
        Stages data = getAllStagesData();
        
        return data.stages.Where(a => a.stageNo == stageNo).First();
    }

    public Stages getAllStagesData()
    {
        var text = Resources.Load<TextAsset>(JsonPath);
        var json = text.text;

        Stages data = Stages.FromJson(json);
        
        maxStageNo = data.stages.Max(number => number.stageNo);
        Debug.Log("MaxStage:"+ maxStageNo);
        return data;
    }

    public void LoadStage(int stageNo)
    {
        if (currentStagePrefab != null)
        {
            RemoveCurrentStage();
        }

        var stage = getStageData(stageNo);
        currentStage = stage;
        Debug.Log("loadingStage");
        var tempStage = Resources.Load<GameObject>($"{StagePath}/{stage.prefabFileName}");
        currentStagePrefab = Instantiate(tempStage, stageStartTransform.position, Quaternion.identity);
        
        maxScore = stage.objectCount;

        GameController.instance.StartStage(stage);
    }

    public void AddScore(int value=1)
    {
        score = score + value;

        if(score == maxScore)
        {
            FinishGameWithCheckScore();
        }
    }

    public bool FinishGameWithCheckScore()
    {
        if (maxScore-score<=5)
        {
            GameController.instance.FinishGame(true);
            validateStage(currentStage.stageNo + 1);

            return true;
        }
        else
        {
            GameController.instance.FinishGame(false);
            return false;
        }
        
    }

    private void RemoveCurrentStage()
    {
        score = 0;
        Destroy(currentStagePrefab);
        currentStage = null;
    }

    public bool isAvailableStage(int stageNo)
    {
        if (PlayerPrefs.HasKey("stage" + stageNo))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void validateStage(int stageNo, int score = 1)
    {
        PlayerPrefs.SetInt("stage" + stageNo, score);
    }

    #region Editor Methods
    private void OnDrawGizmos()
    {
        if (stageStartTransform != null)
        {
            Gizmos.color = new Color(0, 0, 1, .5f);
            Gizmos.DrawSphere(stageStartTransform.position, 1);
        }
    }
    #endregion
}
