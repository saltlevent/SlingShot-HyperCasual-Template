using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trajectory), typeof(LineRenderer))]
public class TouchController : MonoBehaviour
{
    public static TouchController instance;
    [SerializeField]
    private Camera m_Camera;
    public Transform slingRubber;

    #region Public Editor Variables

    [Header("Options")]

    [Tooltip("Ekran�n hangi alan�n�n kullanabilece�ini belirtir. .5 alt yar�s�, 1 tamam�.")]
    [Range(0.0f, 1.0f)]
    public float availableScreenY = .5f;

    [Space()]

    [Tooltip("G�receli f�rlatma g�c�.")]
    public float StretchForceMultiplier = 100;

    [Tooltip("G�receli maxsimum f�rtaltma de�eri.")]
    public float StretchMaxForce = 50;

    [Space()]

    [Tooltip("Gerilmi� olan lasti�in eski pozisyonuna d�nme h�z�.")]
    public float releaseSpeed = 15;

    [Tooltip("Gerilmi� olan lasti�in yatayda gerilebilece�i max de�er. (g�receli)")]
    [Range(0.0f, 5.0f)]
    public float rubberStretchMultiplierX = 5;

    [Tooltip("Gerilmi� olan lasti�in d��eyde gerilebilece�i max de�er. (g�receli)")]
    public float rubberStretchMultiplierZ = 0.05f;

    #endregion

    public float currentStretchForce { get; private set; }
    public Vector3 currentDirection { get; private set; }

    private Vector2 m_FirstTouchPosition;

    private Vector3 m_slingRubberStartPos;
    
    public Vector3 characterPoint => m_slingRubberStartPos + Vector3.forward * .5f + Vector3.down;

    private Vector3 m_slingRubberCurrentPos;

    private Trajectory m_trajectory;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (m_Camera == null)
        {
            m_Camera = Camera.main;
        }

#if UNITY_EDITOR
        if (slingRubber == null)
        {
            Debug.Log("Oyunda SlingRubber tan�mlanmam��");
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        m_slingRubberStartPos = slingRubber.position;

        m_trajectory = GetComponent<Trajectory>();
    }

    private void Update()
    {
        if (GameController.instance.gamePhase == GameController.GamePhase.Game)
        {
            switch (CharacterManager.instance.characterPhase)
            {
                case CharacterManager.CharacterPhase.ReadyToFly:
                    DetectToStartStretch(out m_FirstTouchPosition);
                    break;
                case CharacterManager.CharacterPhase.Stretching:
                    StretchControl(m_FirstTouchPosition);
                    break;
            }
        }
    }

    private void DetectToStartStretch(out Vector2 viewportTouchPosition)
    {
        viewportTouchPosition = Vector3.zero;
        if (Input.touchCount > 0 && CharacterManager.instance.currentCharacter != null)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                m_trajectory.enabled = true;

                Vector2 rawTouchPosition = touch.position;
                viewportTouchPosition = m_Camera.ScreenToViewportPoint(rawTouchPosition);

                //TODO: ekran�n alt yar�s�n� kullanmak ad�na bir ko�ullama
                if (viewportTouchPosition.y < .5)
                {
                    CharacterManager.instance.setCharacterPhase(CharacterManager.CharacterPhase.Stretching);
                    Debug.Log("Stretching");
                }
            }
        }
    }

    private void StretchControl(Vector2 firstTouchPos)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 rawTouchPosition = touch.position;
            Vector2 viewportTouchPosition = m_Camera.ScreenToViewportPoint(rawTouchPosition);

            //Oyuncu geriye do�ru f�rlatmas�n diye bir ko�ul.
            if (firstTouchPos.y < viewportTouchPosition.y) return;

            //Burada sapan gerdirildi�inde olan hesaplamalar
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                SoundManager.instance.PlayStretching();

                var grabbedChar = CharacterManager.instance.currentCharacter;

                var distanceTouch = (firstTouchPos.y - viewportTouchPosition.y) * StretchForceMultiplier;

                currentStretchForce = distanceTouch >= 50 ? 50 : (int)distanceTouch;

                m_slingRubberCurrentPos = m_slingRubberStartPos
                                        - (Vector3.forward * (currentStretchForce * rubberStretchMultiplierZ))
                                        + (Vector3.right * (viewportTouchPosition.x - firstTouchPos.x) * rubberStretchMultiplierX);

                currentDirection = Vector3.Normalize(m_slingRubberStartPos - m_slingRubberCurrentPos);

                slingRubber.position = m_slingRubberCurrentPos;
                grabbedChar.transform.position = m_slingRubberCurrentPos + Vector3.forward * .5f + Vector3.down;

                //Trajectory hesaplamas�
                m_trajectory.SetVelocity(currentDirection * currentStretchForce);
                m_trajectory.RenderArc();

            }
        }
        else if (currentStretchForce > .25f)
        {
            Debug.Log("Throw");
            ReleaseSling();
            CharacterManager.instance.setCharacterPhase(CharacterManager.CharacterPhase.Flying);
            CharacterManager.instance.currentCharacter.GetComponent<Collider>().enabled = true;
            CharacterManager.instance.currentCharacter.GetComponent<Rigidbody>().isKinematic = false;
            CharacterManager.instance.currentCharacter.GetComponent<Rigidbody>().AddForce(currentDirection * currentStretchForce, ForceMode.Impulse);

            m_trajectory.enabled = false;
            SoundManager.instance.StopStretching();
        }
        else
        {
            Debug.Log("Canceled");
            ReleaseSling();
            CharacterManager.instance.setCharacterPhase(CharacterManager.CharacterPhase.ReadyToFly);

            m_trajectory.enabled = false;
            SoundManager.instance.StopStretching();
        }

    }

    public static Quaternion FromDirection(Vector2 direction)
    {
        direction.Normalize();

        float theta = Mathf.Atan2(direction.y, direction.x);

        return Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.forward);
    }

    private void ReleaseSling()
    {
        StartCoroutine(ReleaseSlingIE());
    }
    private IEnumerator ReleaseSlingIE()
    {


        while (Vector3.Distance(slingRubber.position, m_slingRubberStartPos) > .4f)
        {
            yield return null;
            slingRubber.Translate(Vector3.Normalize(-m_slingRubberCurrentPos + m_slingRubberStartPos) * releaseSpeed * Time.deltaTime, Space.World);

        }
        slingRubber.position = m_slingRubberStartPos;
        m_slingRubberCurrentPos = m_slingRubberStartPos;
    }
}
