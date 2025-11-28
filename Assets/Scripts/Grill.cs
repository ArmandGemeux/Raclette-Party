using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class Grill : MonoBehaviour, IInteractable
{
    [Header("Cooking Timings")]
    public float cookingTime = 10f;
    public float burningTime = 10f;
    [Space]
    public float perfectScoreSavingTime = 15f;

    [Header("State checkers")]
    public bool hasGrill;
    public bool isFlipped;
    public bool intermediateGrillScoreReached;
    public bool maxGrillScoreReached;
    public bool savingGrillScore;
    [Space]

    [Header("Grill Values")]
    public int grillCurrentScore;
    [Space]
    public int grillMinimumScore;
    public int grillIntermediateScore;
    public int grillMaximumScore;
    public int currentMaximumScore;

    public Image grillGauge;
    public GameObject grillPrefab;
    public Transform grillPrefabPos;
    private GameObject instantiatedGrillPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grillGauge.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        grillGauge.fillAmount = 0f;
        grillGauge.color = Color.darkRed;
    }

    // Update is called once per frame
    void Update()
    {
        if (grillCurrentScore >= grillIntermediateScore && !savingGrillScore && !intermediateGrillScoreReached && !isFlipped)
        {
            intermediateGrillScoreReached = true;
            Debug.Log("Score intermédiaire atteint");
            IntermediatePerfectGrillSavingDelay();
        }

        if (grillCurrentScore >= currentMaximumScore && !savingGrillScore && isFlipped)
        {
            maxGrillScoreReached = true;
            Debug.Log("Score maximum possible atteint");
            MaxPerfectGrillSavingDelay();
        }
    }

    public void OnInteract()
    {
        UIManager.Instance.SlideInGrillButtons();
        FeedbackManager.Instance.MoveCameraToGrill();
    }
    public void AddGrillBaking()
    {
        if (hasGrill == false)
        {
            hasGrill = true;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.meat);
            instantiatedGrillPrefab = Instantiate(grillPrefab, grillPrefabPos.position, Quaternion.identity);
            //instantiatedCheesePrefab.transform.SetParent(gameObject.transform);
            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, grillIntermediateScore, cookingTime).SetId("firstGrillScoreIncrease"); ;

            Sequence seq = DOTween.Sequence();

            seq.Append((grillGauge.GetComponent<RectTransform>().DOScale(1f, 0.15f)))
               .Join(grillGauge.DOFillAmount(1, cookingTime).SetEase(Ease.InQuad))
               .OnComplete(() =>
               {
                   grillGauge.fillAmount = 1f;
               })
               .Join(grillGauge.DOColor(Color.yellowGreen, cookingTime).SetEase(Ease.InQuad)).SetId("firstGrillGauge");

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            UIManager.Instance.grillInterface[0].gameObject.SetActive(false);
            UIManager.Instance.grillInterface[1].gameObject.SetActive(true);

            //AddCheeseButton == Desactivé, on active juste l'autre

            //Quand le score max est atteint, lance un chrono de perfectScoreSavingTime. Quand celui-ci est complet, lance CheeseBurning();
        }
        else
        {
            return;
        }
    }
    public void FlipSideGrillBaking()
    {
        if (hasGrill)
        {
            isFlipped = true;
            savingGrillScore = false;
            //DoTween : Retourner la viande
            DOTween.Pause("firstGrillScoreDecrease");
            DOTween.Restart("TimeSavingDelay");
            DOTween.Pause("TimeSavingDelay");

            DOTween.Kill("firstGrillGauge");
            DOTween.Kill("grillGaugeSaving");

            grillGauge.fillAmount = 0f;
            grillGauge.color = Color.darkRed;

            Sequence seq = DOTween.Sequence();

            seq.Append((grillGauge.GetComponent<RectTransform>().DOScale(1f, 0.15f)))
               .Join(grillGauge.DOFillAmount(1, cookingTime).SetEase(Ease.InQuad))
               .OnComplete(() =>
               {
                   grillGauge.fillAmount = 1f;
               })
               .Join(grillGauge.DOColor(Color.yellowGreen, cookingTime).SetEase(Ease.InQuad)).SetId("secondGrillGauge");

            //Trouver logique pour additionner la valeur au currentScore plutôt que de l'y emmener

            currentMaximumScore = grillMaximumScore + grillCurrentScore;

            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, currentMaximumScore, cookingTime).SetId("secondGrillScoreIncrease"); ;


            Sequence seq2 = DOTween.Sequence();

            seq2.Append((instantiatedGrillPrefab.transform.DOMoveY(19f, 0.4f)).SetEase(Ease.OutBack).SetDelay(0.2f))
                .Join(instantiatedGrillPrefab.transform.DORotate(new Vector3(180, 0, 0), 0.4f).SetEase(Ease.InOutBack).SetDelay(0.5f))
                .Join(instantiatedGrillPrefab.transform.DOMoveY(grillPrefabPos.position.y + 0.1f, 0.2f).SetEase(Ease.OutExpo).SetDelay(0.5f))
                .Join(instantiatedGrillPrefab.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 0.2f, 100, 0.1f).SetDelay(0.1f))
                .OnComplete(() =>
                {
                    UIManager.Instance.SlideOutGrillButtons();
                    FeedbackManager.Instance.MoveCameraToInitialPosition();
                    UIManager.Instance.grillInterface[1].gameObject.SetActive(false);
                    UIManager.Instance.grillInterface[2].gameObject.SetActive(true);
                }).SetDelay(0.3f);

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ

            //Quand le score max est atteint, lance un chrono de perfectScoreSavingTime. Quand celui-ci est complet, lance CheeseBurning();
        }
        else
        {
            return;
        }
    }
    public void SendToPlate()
    {
        //Caméra assiette + attends un clic dans la bonne position (bool isChoosingSpot, et dans assiette Interactable attends que ce bool soit validé ?)

        //GameManager.Instance.InstantiateCheesePrefab();

        DOTween.Pause("firstGrillScoreIncrease");
        DOTween.Pause("secondGrillScoreIncrease");

        DOTween.Pause("firstGrillScoreDecrease");
        DOTween.Pause("secondGrillScoreDecrease");

        GameManager.Instance.AddToScore(grillCurrentScore);
        UIManager.Instance.SlideOutGrillButtons();
        FeedbackManager.Instance.MoveCameraToInitialPosition();

        ResetGrillScore();

        UIManager.Instance.grillInterface[0].gameObject.SetActive(true);
        UIManager.Instance.grillInterface[1].gameObject.SetActive(false);
        UIManager.Instance.grillInterface[2].gameObject.SetActive(false);
    }

    private void IntermediateGrillBurning()
    {
        if (intermediateGrillScoreReached && savingGrillScore)
        {
            DOTween.Kill("grillGaugeSaving");

            Sequence seq = DOTween.Sequence();

            seq.Append((grillGauge.GetComponent<RectTransform>().DOScale(1f, 0.3f)))
               .Join(grillGauge.DOFillAmount(0, burningTime).SetEase(Ease.InQuad))
               .Join(grillGauge.DOColor(Color.darkRed, burningTime).SetEase(Ease.InQuad)).SetId("firstGrillBurningGauge");

            savingGrillScore = false;
            Debug.Log("ça crame 1er du nom");
            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, grillMinimumScore, burningTime).SetId("firstGrillScoreDecrease");
        }
        else
        {
            return;
        }
    }

    private void MaxGrillBurning()
    {
        if (maxGrillScoreReached && savingGrillScore)
        {
            DOTween.Kill("2ndGrillGaugeSaving");

            Sequence seq = DOTween.Sequence();

            seq.Append((grillGauge.GetComponent<RectTransform>().DOScale(1f, 0.3f)))
               .Join(grillGauge.DOFillAmount(0, burningTime).SetEase(Ease.InQuad))
               .Join(grillGauge.DOColor(Color.darkRed, burningTime).SetEase(Ease.InQuad)).SetId("secondGrillBurningGauge");

            savingGrillScore = false;
            Debug.Log("ça crame 2eme du nom");
            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, grillMinimumScore, burningTime).SetId("secondGrillScoreDecrease");
        }
        else
        {
            return;
        }
    }

    private void IntermediatePerfectGrillSavingDelay()
    {
        savingGrillScore = true;
        DOTween.Pause("firstGrillScoreIncrease");

        grillGauge.GetComponent<RectTransform>().DOScale(1.3f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetId("grillGaugeSaving");

        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde n°1");

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            IntermediateGrillBurning();
        }).SetId("TimeSavingDelay");
    }
    private void MaxPerfectGrillSavingDelay()
    {
        grillGauge.GetComponent<RectTransform>().DOScale(1.3f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetId("2ndGrillGaugeSaving");

        savingGrillScore = true;
        DOTween.Pause("firstGrillScoreIncrease");
        DOTween.Pause("secondGrillScoreIncrease");
        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde n°2");

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            MaxGrillBurning();
        }).SetId("TimeSavingDelay");
    }

    public void ResetGrillScore()
    {
        Destroy(instantiatedGrillPrefab);
        hasGrill = false;
        isFlipped = false;
        maxGrillScoreReached = false;
        intermediateGrillScoreReached = false;
        savingGrillScore = false;

        DOTween.Restart("firstGrillScoreIncrease");
        DOTween.Kill("firstGrillScoreIncrease");

        DOTween.Restart("secondGrillScoreIncrease");
        DOTween.Kill("secondGrillScoreIncrease");

        DOTween.Kill("grillScoreDecrease");
        DOTween.Kill("TimeSavingDelay");

        DOTween.Kill("firstGrillGauge");
        DOTween.Kill("secondGrillGauge");

        DOTween.Kill("firstGrillBurningGauge");
        DOTween.Kill("secondGrillBurningGauge");

        DOTween.Kill("2ndGrillGaugeSaving");
        DOTween.Kill("grillGaugeSaving");

        Sequence seq = DOTween.Sequence();
        seq.Append((grillGauge.GetComponent<RectTransform>().DOScale(0f, 0f))).Join(grillGauge.DOFillAmount(0f, 0f)).Join(grillGauge.DOColor(Color.darkRed, 0f));

        grillCurrentScore = 0;
        currentMaximumScore = 0;
    }
}
