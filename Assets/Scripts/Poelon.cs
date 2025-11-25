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

    public GameObject cheesePrefab;
    private GameObject instantiatedCheesePrefab;

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

        cheeseInterfaceTargetPos = new Vector2[cheeseInterface.Length];

        for (int i = 0; i < cheeseInterface.Length; i++)
        {
            RectTransform btn = cheeseInterface[i];

            cheeseInterfaceTargetPos[i] = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(-1500f, cheeseInterfaceTargetPos[i].y);
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
            instantiatedCheesePrefab = Instantiate(cheesePrefab, gameObject.transform.position, Quaternion.identity);
            instantiatedCheesePrefab.transform.SetParent(gameObject.transform);
            DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMaximumScore, cookingTime).SetId("currentScoreIncrease");
            UIManager.Instance.cheeseGauge.DOFillAmount(1, cookingTime).SetId("CheeseGaugeIncrease");
            UIManager.Instance.cheeseGauge.DOColor(Color.green, cookingTime);

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

        DOTween.Pause("currentScoreIncrease");
        DOTween.Pause("CheeseGaugeIncrease");

        GameManager.Instance.lookingForCheeseSpot = true;

        GameManager.Instance.GetPoelonRef(gameObject.GetComponent<Poelon>());

        GameManager.Instance.AddToScore(cheeseCurrentScore); ;

        FeedbackManager.Instance.MoveCameraToPlate();
        UIManager.Instance.SlideOutCheeseButtons();

        cheeseInterface[0].gameObject.SetActive(true);
        cheeseInterface[1].gameObject.SetActive(false);
    }

    public void ResetCheeseScore()
    {
        Destroy(instantiatedCheesePrefab);
        hasCheese = false;
        maxScoreReached = false;
        DOTween.Restart("currentScoreIncrease");

        DOTween.Kill("currentScoreIncrease");
        DOTween.Kill("currentScoreDecrease");

        DOTween.Restart("CheeseGaugeIncrease");
        DOTween.Kill("CheeseGaugeIncrease");

        DOTween.Restart("CheeseGaugeDecrease");
        DOTween.Kill("CheeseGaugeDecrease");

        //Ajouter reset de couleur

        UIManager.Instance.cheeseGauge.fillAmount = 0f;
        cheeseCurrentScore = 0;
    }

    public void CheeseBurning()
    {
        savingCheeseScore = false;
        Debug.Log("ça crame wsh");
        DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMinimumScore, burningTime).SetId("currentScoreDecrease");
        UIManager.Instance.cheeseGauge.DOFillAmount(0, burningTime).SetId("CheeseGaugeDecrease");
        UIManager.Instance.cheeseGauge.DOColor(Color.red, burningTime);
    }

    private void PerfectCheeseSavingDelay()
    {
        savingCheeseScore = true;
        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde");

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            CheeseBurning();
        });
    }
}
