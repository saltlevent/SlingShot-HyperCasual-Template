using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region enums

    public enum CharacterPhase { None, Waiting, Preparing, ReadyToFly, Stretching, Flying, }

    #endregion

    public static CharacterManager instance;


    public CharacterPhase characterPhase
    {
        get
        {
            return _CharacterPhase;
        }
        private set
        {
            _CharacterPhase = value;
            OnCharacterPhaseChanged?.Invoke(value);
        }
    }

    public event Action<CharacterPhase> OnCharacterPhaseChanged;

    public GameObject currentCharacter { get; private set; }
    public List<GameObject> availableCharacters { get; private set; }
    public int characterOrder { get; private set; }
    public int availableCount { get; private set; }
    private GameController.GamePhase GameController_gamePhase => GameController.instance.gamePhase;

    private CharacterPhase _CharacterPhase = CharacterPhase.Waiting;

    public Transform characterStartPoint;

    private void Awake()
    {
        instance = this;
        availableCharacters = new List<GameObject>();
        characterOrder = 0;
        availableCount = 0;

        if (characterStartPoint == null)
        {
            Debug.LogError("sahnede herhangi bir characterStartPoint yok.");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    private void Start()
    {
        GameController.instance.OnNewStageOpen += OnNewStageOpen;
        GameController.instance.OnGameFinished += OnGameFinished;
    }

    private void OnGameFinished(bool success)
    {
        RemoveCharacters();
    }

    float elapsedTime = 0;
    private void Update()
    {
        if (characterPhase == CharacterPhase.Flying)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 5)
            {
                elapsedTime = 0;
                ChangeCharacter();
                characterPhase = CharacterPhase.Preparing;
            }
        }

    }

    private void OnNewStageOpen(Stage stage)
    {
        GetNewCharacters(stage.manCount);
    }

    public void setCharacterPhase(CharacterPhase phase)
    {
        characterPhase = phase;
    }

    public void ChangeCharacter()
    {
        
        if (characterOrder == availableCharacters.Count-1)
        {
            Debug.Log($"Change Character:order {characterOrder}, count{availableCharacters.Count}");
            StageController.instance.FinishGameWithCheckScore();
        }
        else
        {
            characterOrder++;
            availableCount--;
            currentCharacter = availableCharacters[characterOrder];
        }
    }

    #region Private Methods

    private void RemoveCharacters()
    {
        if (availableCharacters.Count > 0)
        {
            for (int i = availableCharacters.Count - 1; i >= 0; i--)
            {
                Destroy(availableCharacters[i]);
            }
            
            availableCharacters.Clear();
            characterOrder = 0;
        }
    }
    private void GetNewCharacters(int count)
    {
        RemoveCharacters();
        
        if (availableCharacters.Count == 0)
        {
            var man = Resources.Load<GameObject>("Man");

            for (int i = 0; i < count; i++)
            {
                var tempMan = Instantiate(man);
                tempMan.GetComponent<Character>().orderNumber = i;
                availableCharacters.Add(tempMan);
                
            }
            if (availableCharacters.Count > 0)
            {
                currentCharacter = availableCharacters[0];
                availableCount = availableCharacters.Count;
            }
        }
    }

    #endregion

    #region Editor Methods
    private void OnDrawGizmos()
    {
        if (characterStartPoint != null)
        {
            Gizmos.color = new Color(0, 1, 0, .5f);
            Gizmos.DrawSphere(characterStartPoint.position, 1);
        }
    }
    #endregion
}
