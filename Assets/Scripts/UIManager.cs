using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("References")]
    public TextMeshProUGUI currentCheeseScore;
    public TextMeshProUGUI currentPlateScore;
    public RectTransform[] cheeseInterface;

    [Space]
    public float slideInDuration = 0.6f;
    public float slideOutDuration = 0.4f;
    public float delayBetweenButtons = 0.2f;

    private Vector2[] targetPos;

    public bool UIOnScreen;

    public Camera mainCamera;

    [Space]
    public Transform mainCameraPos;
    public Transform middlePoelonCamPos;

    //public Camera leftPoelonCamera;
    //public Camera rightPoelonCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        // Vérifie qu’il n’y a qu’un seul GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre les scènes
    }

    void Update()
    {

    }

    void Start()
    {
        targetPos = new Vector2[cheeseInterface.Length];

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            targetPos[i] = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(-1500f, targetPos[i].y);
        }

        cheeseInterface[1].gameObject.SetActive(false);
    }

    public void SlideInCheeseButtons()
    {
        UIOnScreen = true;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            btn.DOAnchorPos(targetPos[i], slideInDuration).SetEase(Ease.OutBack).SetDelay(i * delayBetweenButtons);
        }
    }
    public void SlideOutCheeseButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            btn.DOAnchorPos(new Vector2(-1500f, targetPos[i].y), slideOutDuration).SetEase(Ease.InOutBack).SetDelay(i * delayBetweenButtons);
        }
    }
}
