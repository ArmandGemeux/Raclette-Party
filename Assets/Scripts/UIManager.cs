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
    public RectTransform cuttingAmountSliderRT;
    public RectTransform startCuttingButton;
    public TextMeshProUGUI cuttingAmountText;
    public Slider cuttingAmountSlider;

    public int countdown;
    public bool isCuttingGameStarted;

    [Space]

    public RectTransform spawnArea;     // La zone dans laquelle on spawn (RectTransform dans le WorldSpace Canvas)
    public RectTransform prefab;        // Ton UI prefab
    public float margin = 20f;
    private int cuttingAmount;

    float minX;
    float maxX;
    float minY;
    float maxY;

    [Space]
    public float slideInDuration = 0.3f;
    public float slideOutDuration = 0.2f;
    public float delayBetweenButtons = 0.01f;

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

        CuttingBoardSetupOff();
        ButtonSpawnArea();
    }

    private void Update()
    {
        cuttingAmount = (int)cuttingAmountSlider.value;
        cuttingAmountText.text = cuttingAmountSlider.value.ToString() + (" x");
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
    public void CuttingBoardSetupOn()
    {
        UIOnScreen = true;

        cuttingAmountSliderRT.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        startCuttingButton.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        cuttingAmountText.GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
    }
    public void CuttingBoardSetupOff()
    {
        UIOnScreen = false;

        cuttingAmountSliderRT.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        startCuttingButton.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        cuttingAmountText.GetComponent<RectTransform>().DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
    }

    public void ButtonSpawnArea()
    {
        Vector2 size = spawnArea.rect.size;

        minX = -size.x / 2f + margin;
        maxX = size.x / 2f - margin;
        minY = -size.y / 2f + margin;
        maxY = size.y / 2f - margin;
    }

    public void StartingCuttingChallengeButton()
    {
        //Pensez à désactiver le clic sur la cuttingBoard à ce moment là, sinon fait réapparaître les boutons de setup
        isCuttingGameStarted = true;
        countdown = cuttingAmount;
        CuttingBoardSetupOff();
        CuttingButtonSpawns();
    }

    public void CuttingButtonSpawns()
    {
        if (countdown > 0)
        {
            countdown--;

            MouseClickCut.Instance.isCutting = true;
            Vector2 randomLocalPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            RectTransform uiInstance = Instantiate(prefab, spawnArea);
            uiInstance.anchoredPosition = randomLocalPos;
        }
        else
        {
            isCuttingGameStarted = false;
            Debug.Log("Mini Jeu terminé");
            //Envoie le score total des boutons appuyés
        }
    }
}
