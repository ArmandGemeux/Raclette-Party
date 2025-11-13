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
    }

    // Update is called once per frame
    void Update()
    {
        if (cheeseCurrentScore >= cheeseMaximumScore)
        {
            maxScoreReached = true;
            PerfectCheeseSavingDelay();
        }

        UIManager.Instance.currentCheeseScore.text = cheeseCurrentScore.ToString();

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

    public async void OnInteract()
    {
        //Debug.Log("Oh oh, je suis un poélon coquinou ! :) ");

        //if (maxScoreReached == false)
        //CheeseBaking();

        //Feedbacks à déplacer dans un Feedback Manager (récupérer position du poélon) ?

        UIManager.Instance.SlideInCheeseButtons();
        FeedbackManager.Instance.MoveCameraToMiddlePoelon();

        transform.DOMove(new Vector3(1.02f, 0.45f, 0f), 0.5f);

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
    }

    public void CheeseBaking()
    {
        if (maxScoreReached == false && hasCheese == false)
        {
            hasCheese = true;
            instantiatedCheesePrefab = Instantiate(cheesePrefab, gameObject.transform.position, Quaternion.identity);
            instantiatedCheesePrefab.transform.SetParent(gameObject.transform);
            DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMaximumScore, cookingTime).SetId("currentScoreIncrease"); ;

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            UIManager.Instance.cheeseInterface[0].gameObject.SetActive(false);
            UIManager.Instance.cheeseInterface[1].gameObject.SetActive(true);

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

        GameManager.Instance.lookingForSpot = true;

        GameManager.Instance.GetCheeseData(cheeseCurrentScore);

        FeedbackManager.Instance.MoveCameraToPlate();
        UIManager.Instance.SlideOutCheeseButtons();

        UIManager.Instance.cheeseInterface[0].gameObject.SetActive(true);
        UIManager.Instance.cheeseInterface[1].gameObject.SetActive(false);

        //ResetCheeseScore();
    }

    public void ResetCheeseScore()
    {
        Destroy(instantiatedCheesePrefab);
        hasCheese = false;
        maxScoreReached = false;
        DOTween.Restart("currentScoreIncrease");
        DOTween.Kill("currentScoreIncrease");

        cheeseCurrentScore = 0;
    }

    public void CheeseBurning()
    {
        Debug.Log("ça crame wsh");
        DOTween.To(() => cheeseCurrentScore, x => cheeseCurrentScore = x, cheeseMinimumScore, burningTime);
    }

    private void PerfectCheeseSavingDelay()
    {
        maxScoreReached = false;
        float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici
        CheeseBurning();
        //Chrono de perfectScoreSavingTime durée, qui une fois à zéro, lance CheeseBurning();
    }
}
