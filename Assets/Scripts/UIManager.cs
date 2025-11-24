using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI currentPlateScore;

    [Header("References Cheese")]
    public TextMeshProUGUI currentCheeseScore;
    public RectTransform[] cheeseInterface;
    public Image cheeseGauge;

    [Header("References Grill")]
    public TextMeshProUGUI currentGrillScore;
    public RectTransform[] grillInterface;

    [Header("References Cutting Board")]

    [Space]
    public float slideInDuration = 0.6f;
    public float slideOutDuration = 0.4f;
    public float delayBetweenButtons = 0.05f;

    private Vector2[] cheeseInterfaceTargetPos;
    private Vector2[] grillInterfaceTargetPos;

    public bool UIOnScreen;

    public Camera mainCamera;

    [Space]
    public Transform mainCameraPos;
    public Transform middlePoelonCamPos;
    public Transform grillCamPos;

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

    void Start()
    {
        cheeseGauge.fillAmount = 0f;
        cheeseInterfaceTargetPos = new Vector2[cheeseInterface.Length];

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            cheeseInterfaceTargetPos[i] = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(-1500f, cheeseInterfaceTargetPos[i].y);
        }

        cheeseInterface[1].gameObject.SetActive(false);


        grillInterfaceTargetPos = new Vector2[grillInterface.Length];

        for (int i = 0; i < grillInterface.Length; i++)
        {
            RectTransform btn = grillInterface[i];

            grillInterfaceTargetPos[i] = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(-1500f, grillInterfaceTargetPos[i].y);
        }

        grillInterface[1].gameObject.SetActive(false);
        grillInterface[2].gameObject.SetActive(false);
    }

    public void SlideInCheeseButtons()
    {
        UIOnScreen = true;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            btn.DOAnchorPos(cheeseInterfaceTargetPos[i], slideInDuration).SetEase(Ease.OutBack).SetDelay(i * delayBetweenButtons);
        }
    }
    public void SlideOutCheeseButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            btn.DOAnchorPos(new Vector2(-1500f, cheeseInterfaceTargetPos[i].y), slideOutDuration).SetEase(Ease.InOutBack).SetDelay(i * delayBetweenButtons);
        }
    }
    public void SlideInGrillButtons()
    {
        UIOnScreen = true;

        for (int i = 0; i < grillInterface.Length; i++)
        {
            RectTransform btn = grillInterface[i];

            btn.DOAnchorPos(grillInterfaceTargetPos[i], slideInDuration).SetEase(Ease.OutBack).SetDelay(i * delayBetweenButtons);
        }
    }
    public void SlideOutGrillButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < grillInterface.Length; i++)
        {
            RectTransform btn = grillInterface[i];

            btn.DOAnchorPos(new Vector2(-1500f, grillInterfaceTargetPos[i].y), slideOutDuration).SetEase(Ease.InOutBack).SetDelay(i * delayBetweenButtons);
        }
    }
}
