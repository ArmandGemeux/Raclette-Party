using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI totalScore;

    [Header("References Cheese")]
    //public RectTransform[] cheeseInterface;
    public Image cheeseGauge;
    public RectTransform[] wholeCheeseInterface;

    [Header("References Grill")]
    public RectTransform[] grillInterface;

    [Header("References Cutting Board")]
    public RectTransform cuttingAmountSliderRT;
    public RectTransform startCuttingButton;
    public RectTransform returnCuttingButton;
    public TextMeshProUGUI cuttingAmountText;
    public Slider cuttingAmountSlider;

    [Space]
    public int countdown;
    public bool isCuttingGameStarted;

    [Space]

    public RectTransform spawnArea;     // La zone dans laquelle on spawn (RectTransform dans le WorldSpace Canvas)
    public RectTransform prefab;        // Ton UI prefab
    public float margin = 20f;
    private int cuttingAmount;

    public Transform parent;

    float minX;
    float maxX;
    float minY;
    float maxY;

    [Space]
    public float slideInDuration = 0.3f;
    public float slideOutDuration = 0.2f;
    public float delayBetweenButtons = 0.01f;

    public bool UIOnScreen;

    public Camera mainCamera;
    public Poelon[] poelons;

    [Header("References Opening Menu")]
    public Image menuBackground;
    public RectTransform[] menuInterface;

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
        //cheeseGauge.fillAmount = 0f;

        grillInterface[1].gameObject.SetActive(false);
        grillInterface[2].gameObject.SetActive(false);

        SlideOutGrillButtons();
        CuttingBoardSetupOff();
        ButtonSpawnArea();
    }

    private void Update()
    {
        cuttingAmount = (int)cuttingAmountSlider.value;
        cuttingAmountText.text = cuttingAmountSlider.value.ToString() + (" x");
    }

    public void SlideInCheeseButtons(RectTransform[] cheeseInterface, Vector2[] cheeseInterfaceTargetPos)
    {
        UIOnScreen = true;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i].GetComponent<RectTransform>();
            btn.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        }
    }
    public void SlideOutCheeseButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < wholeCheeseInterface.Length; i++)
        {
            RectTransform btn = wholeCheeseInterface[i].GetComponent<RectTransform>();
            btn.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        }
    }

    public void SetPoelonToOriginalPosition()
    {
        foreach (Poelon poelon in poelons)
        {
            poelon.SetInitialPosition();
        }
    }

    public void SlideInGrillButtons()
    {
        UIOnScreen = true;

        for (int i = 0; i < grillInterface.Length; i++)
        {
            RectTransform btn = grillInterface[i];

            btn.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        }
    }
    public void SlideOutGrillButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < grillInterface.Length; i++)
        {
            RectTransform btn = grillInterface[i];

            btn.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        }
    }
    public void CuttingBoardSetupOn()
    {
        UIOnScreen = true;

        cuttingAmountSliderRT.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        startCuttingButton.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        returnCuttingButton.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
        cuttingAmountText.GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutExpo);
    }
    public void CuttingBoardSetupOff()
    {
        UIOnScreen = false;

        cuttingAmountSliderRT.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        startCuttingButton.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        returnCuttingButton.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
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
            MouseClickCut.Instance.isCutting = true;
            Vector2 randomLocalPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            RectTransform uiInstance = Instantiate(prefab, spawnArea);
            uiInstance.anchoredPosition = randomLocalPos;

            countdown--;
        }
        else
        {
            MouseClickCut.Instance.isCutting = false;
            isCuttingGameStarted = false;

            CuttingBoardSetupOn();

            int targetLayer = LayerMask.NameToLayer("CuttedPartLayer");

            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                if (go.layer == targetLayer)
                {
                    go.transform.SetParent(parent);
                }
            }

            foreach (Transform child in parent)
            {
                child.gameObject.transform.DOScale(new Vector3(0, 0, 0), .2f).SetEase(Ease.InOutQuart).OnComplete(() => {
                    child.gameObject.SetActive(false);
                });
            }

            Debug.Log("Mini Jeu terminé");
            //Envoie le score total des boutons appuyés
        }
    }

    public void OpenGameScreen()
    {
        menuBackground.DOFade(0f, 0.15f).OnComplete(() => {
            menuBackground.gameObject.SetActive(false);
        });

        for (int i = 0; i < menuInterface.Length; i++)
        {
            RectTransform btn = menuInterface[i];
            btn.DOScale(0f, 0.3f).SetEase(Ease.InOutBack).SetDelay(0.1f).OnComplete(() => {
                btn.gameObject.SetActive(false);
            });
        }
    }
}
