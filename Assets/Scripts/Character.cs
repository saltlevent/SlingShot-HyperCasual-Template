using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int orderNumber;

    private bool canMove = false;

    private Animator m_Animator;
    private Collider m_Collider;
    private Vector3 lastPosition;
    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Collider = GetComponentInChildren<Collider>();
        m_Collider.enabled = false;

        transform.position = CharacterManager.instance.characterStartPoint.position + Vector3.right * orderNumber;
        lastPosition = transform.position;
        CharacterManager.instance.OnCharacterPhaseChanged += OnCharacterPhaseChange;
        m_Animator.transform.rotation = Quaternion.Euler(0, -90, 0);

        if (CharacterManager.instance.currentCharacter.GetComponent<Character>().orderNumber == orderNumber)
        {
            Prepare();
        }
        else
        {
            canMove = true;
            m_Animator.CrossFade("Run", .1f);
        }
    }
    private void Update()
    {
        if (canMove)
        {
            Debug.Log($"degisme{transform.position.x - lastPosition.x}");
            if (lastPosition.x-transform.position.x < 1.0f)
            {
                transform.position += Vector3.left * Time.deltaTime;

                Debug.Log("Buraya girdi");
            }
            else
            {
                m_Animator.CrossFade("Idle", .1f);
                canMove = false;
            }
        }
        else
        {
            lastPosition = transform.position;
        }
    }

    private void OnDestroy()
    {
        CharacterManager.instance.OnCharacterPhaseChanged -= OnCharacterPhaseChange;
    }

    private void OnCharacterPhaseChange(CharacterManager.CharacterPhase obj)
    {
        if (CharacterManager.instance.availableCharacters.Count > 0)
        {
            if (obj == CharacterManager.CharacterPhase.Preparing)
            {
                if (CharacterManager.instance.currentCharacter.GetComponent<Character>().orderNumber == orderNumber)
                {
                    Prepare();
                }
                else
                {
                    canMove = true;
                    m_Animator.CrossFade("Run", .2f);
                }
            }
        }
    }

    private void Prepare()
    {
        m_Animator.CrossFade("Climbing", .2f);

    }

   
    public void OnClimbed()
    {
        m_Animator.enabled = false;
        transform.position = TouchController.instance.characterPoint;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        CharacterManager.instance.setCharacterPhase(CharacterManager.CharacterPhase.ReadyToFly);

        Debug.Log("sett");
    }
}
