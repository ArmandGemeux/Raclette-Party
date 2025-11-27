using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class Poelon : MonoBehaviour, IInteractable
{
    [Header("Cooking Timings")]
    public float cookingTime = 10f;
    public float burningTime = 10f;
    [Space]
    public float perfectScoreSavingTime = 15f;


    [Header("State checkers")]
    public string ID;
    public bool hasCheese = false;
    public bool maxScoreReached = false;
    bool savingCheeseScore = false;

    [Header("Cheese Values")]
    public int cheeseCurrentScore;
    [Space]
    public int cheeseStartingScore;
    [Space]
    public int cheeseMinimumScore;
    public int cheeseMaximumScore;

    [Header("Cheese Objects")]

    public GameObject cheesePrefab;
    public Transform cheesePrefabPos;
    private GameObject instantiatedCheesePrefab;
    [Space]
    public Image cheeseGauge;

    private Vector3 initialPos;
    private Vector3 initialRot;
    public Transform movedPosition;
    public Transform cameraPosition;
    public RectTransform[] cheeseInterface;
    private Vector2[] cheeseInterfaceTargetPos;

    /*[Header("UI & Feedback")]
    public UIManager UIManager;
    public Camera mainCamera;

    public Canvas cheeseButtonsCanvas;

    public Button addCheeseButton;
    public Button sendToPlateButton;
    [Space]
    public Button returnButton;*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation.eulerAngles;

        cheeseGauge.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        cheeseGauge.fillAmount = 0f;
        cheeseGauge.color = Color.darkRed;

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i].GetComponent<RectTransform>();
            btn.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutExpo);
        }

        cheeseInterface[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cheeseCurrentScore >= cheeseMaximumScore && savingCheeseScore == false)
        {
            maxScoreReached = true;
            PerfectCheeseSavingDelay();
        }

        //UIManager.Instance.currentCheeseScore.text = cheeseCurrentScore.ToString();

        /*if (hasCheese == true && maxScoreReached == false)
        {
            //currentScore += (int)Mathf.Lerp(startingScore, maximumScore, intensity * intensityMultiplier);
            //DOTween.To(currentScore, currentScore, maximumScore, 15);
        }

        if (hasCheese == true && currentScore >= maximumScore)
            maxScoreReached = true;

        /*if (hasCheese && maxScoreReached == true)
        {
            currentScore -= (int)Mathf.Lerp(currentScore, minimumScore, intensity * intensityMultiplier);
        }

        /*Fait un lerp entre le StartScore d'un fromage et son MaximumScore.
        Si le maximumScore est atteint, il est maintenu un instant (fourchette de score ? Timer ?) avant de déclencher un nouveau lerp (entre maximumScore et minimumScore, vitesse = intensity)
        Le changement de lerp se fait grâce à un changement d'état ("Cuit = true")*/
    }

    public void OnInteract()
    {
        //Debug.Log("Oh oh, je suis un poélon coquinou ! :) ");

        //if (maxScoreReached == false)
        //CheeseBaking();

        //Feedbacks à déplacer dans un Feedback Manager (récupérer position du poélon) ?

        UIManager.Instance.SlideInCheeseButtons(cheeseInterface, cheeseInterfaceTargetPos);
        FeedbackManager.Instance.MoveCameraToPoelon(cameraPosition);

        transform.DOMove(movedPosition.position, 0.5f);
        transform.DORotate(movedPosition.rotation.eulerAngles, 0.5f);

        //Draw UI here


        //Feedback doit fonctionner comme : récupère référence de l'interact object (via raycast) et possède des références à tout (camera, UI, etc). C'est lui qui envoie les animations à droite à gauche, et qui gère l'ensemble des feedbacks.


        //DOTween.To(() => currentScore, x => currentScore = x, maximumScore, 10);

        /*Rapproche le poelon pour en examiner le contenu
        fait apparaître l'interface du fromage en cours de cuisson : Jauge de cuisson avec des crans dedans pour indiquer les états, et le score associé
        Affiche un slider de puissance entre 1 et 10, modifiable
        Un bouton sous le poélon -> Assiette
        Provoque un dézoom, on doit ensuite cliquer sur le spot désiré dans l'assiette (état selection du spot, on ne peut cliquer que sur ça).
        Quand clic fait, on instancie le fromage dans le spot, on le retire du poélon, il retourne dans l'appareil*/
    }

    public void SetInitialPosition()
    {
        transform.DOMove(initialPos, 0.5f);
        transform.DORotate(initialRot, 0.5f);
    }

    public void CheeseBaking()
    {
        if (maxScoreReached == false && hasCheese == false)
        {
            hasCheese = true;
            instantiatedCheesePrefab = Instantiate(cheesePrefab, cheesePrefabPos.position, Quaternion.Euler(new Vector3 (0f, 270, 20.69f)));
            instantiatedCheesePrefab.transform.SetParent(gameObject.transform);


            AudioManager.Instance.PlaySFX(AudioManager.Instance.cheese);
            DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMaximumScore, cookingTime).SetId("currentScoreIncrease" + ID);

            Sequence seq = DOTween.Sequence();

            seq.Append((cheeseGauge.GetComponent<RectTransform>().DOScale(1f, 0.15f)))
               .Join(cheeseGauge.DOFillAmount(1, cookingTime).SetEase(Ease.InQuad))
               .OnComplete(() =>
               {
                   cheeseGauge.fillAmount = 1f;
               })
               .Join(cheeseGauge.DOColor(Color.yellowGreen, cookingTime).SetEase(Ease.InQuad)).SetId("firstCheeseGauge" + ID);

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            cheeseInterface[0].gameObject.SetActive(false);
            cheeseInterface[1].gameObject.SetActive(true);

            //AddCheeseButton == Desactivé, on active juste l'autre

            //Quand le score max est atteint, lance un chrono de perfectScoreSavingTime. Quand celui-ci est complet, lance CheeseBurning();
        }
        else
        {
            Debug.Log("Y'a déjà du fromage gros gourmand");
            return;
        }
    }

    public void SendToPlate()
    {
        //Caméra assiette + attends un clic dans la bonne position (bool isChoosingSpot, et dans assiette Interactable attends que ce bool soit validé ?)

        //GameManager.Instance.InstantiateCheesePrefab();

        DOTween.Pause("currentScoreIncrease" + ID);
        DOTween.Pause("firstCheeseGauge" + ID);

        GameManager.Instance.GetPoelonRef(gameObject.GetComponent<Poelon>());

        GameManager.Instance.AddToScore(cheeseCurrentScore); ;

        ResetCheeseScore();

        FeedbackManager.Instance.MoveCameraToInitialPosition();
        UIManager.Instance.SlideOutCheeseButtons();

        cheeseInterface[0].gameObject.SetActive(true);
        cheeseInterface[1].gameObject.SetActive(false);
    }

    public void ResetCheeseScore()
    {
        Destroy(instantiatedCheesePrefab);
        hasCheese = false;
        maxScoreReached = false;
        DOTween.Restart("currentScoreIncrease" + ID);

        DOTween.Kill("currentScoreIncrease" + ID);
        DOTween.Kill("currentScoreDecrease" + ID);

        DOTween.Restart("firstCheeseGauge" + ID);
        DOTween.Kill("firstCheeseGauge" + ID);

        DOTween.Restart("secondCheeseGauge" + ID);
        DOTween.Kill("secondCheeseGauge" + ID);

        DOTween.Restart("secondCheeseGauge" + ID);
        DOTween.Kill("cheeseGaugeSaving" + ID);

        Sequence seq = DOTween.Sequence();
        seq.Append((cheeseGauge.GetComponent<RectTransform>().DOScale(0f, 0f))).Join(cheeseGauge.DOFillAmount(0f, 0f)).Join(cheeseGauge.DOColor(Color.darkRed, 0f));

        cheeseCurrentScore = 0;
    }

    public void CheeseBurning()
    {
        savingCheeseScore = false;

        DOTween.Kill("cheeseGaugeSaving" + ID);

        Sequence seq = DOTween.Sequence();

        seq.Append((cheeseGauge.GetComponent<RectTransform>().DOScale(1f, 0.3f)))
           .Join(cheeseGauge.DOFillAmount(0, burningTime).SetEase(Ease.InQuad))
           .Join(cheeseGauge.DOColor(Color.darkRed, burningTime).SetEase(Ease.InQuad)).SetId("secondCheeseGauge" + ID);

        DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMinimumScore, burningTime).SetId("currentScoreDecrease");
    }

    private void PerfectCheeseSavingDelay()
    {
        savingCheeseScore = true;
        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde");

        //DOTween.Kill("firstCheeseGauge" + ID);

        cheeseGauge.GetComponent<RectTransform>().DOScale(1.3f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetId("cheeseGaugeSaving" + ID);

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            CheeseBurning();
        });
    }
}
