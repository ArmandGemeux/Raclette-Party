using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class Poelon : MonoBehaviour, IInteractable
{
    [Range(0, 10)]
    public int intensity;
    [Range(0, 1f)]
    public float intensityMultiplier;
    public bool hasCheese = false;
    public bool maxScoreReached = false;

    public float perfectScoreSavingTime = 15f;

    [Header("Cheese Values")]

    public int currentScore;
    [Space]
    public int startingScore;
    [Space]
    public int minimumScore;
    public int maximumScore;

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
        if (currentScore >= maximumScore)
        {
            maxScoreReached = true;
        }

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
            DOTween.To(() => currentScore, x => currentScore = x, maximumScore, 10 / intensity);

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            UIManager.Instance.cheeseButtons[0].gameObject.SetActive(false);
            UIManager.Instance.cheeseButtons[1].gameObject.SetActive(true);

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
        GameManager.Instance.InstantiateCheesePrefab();

        UIManager.Instance.cheeseButtons[0].gameObject.SetActive(true);
        UIManager.Instance.cheeseButtons[1].gameObject.SetActive(false);

        Debug.Log("Fromage dans l'assiette !");
    }

    async Task CheeseBurning()
    {
        DOTween.To(() => currentScore, x => currentScore = x, minimumScore, 10/intensity);
    }

    private void PerfectCheeseSavingDelay()
    {
        //Chrono de perfectScoreSavingTime durée, qui une fois à zéro, lance CheeseBurning();
    }
}
