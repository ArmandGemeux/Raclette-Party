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
    public bool intermediateGrillScoreReached;
    public bool maxGrillScoreReached;
    bool savingGrillScore = false;
    [Space]

    [Header("Grill Values")]
    public int grillCurrentScore;
    [Space]
    public int grillMinimumScore;
    public int grillIntermediateScore;
    public int grillMaximumScore;
    public int currentMaximumScore;

    public GameObject grillPrefab;
    private GameObject instantiatedGrillPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grillCurrentScore >= grillIntermediateScore && savingGrillScore == false && intermediateGrillScoreReached == false)
        {
            intermediateGrillScoreReached = true;
            IntermediatePerfectGrillSavingDelay();
        }

        if (grillCurrentScore >= grillMaximumScore && savingGrillScore == false)
        {
            maxGrillScoreReached = true;
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
            instantiatedGrillPrefab = Instantiate(grillPrefab, gameObject.transform.position, Quaternion.identity);
            //instantiatedCheesePrefab.transform.SetParent(gameObject.transform);
            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, grillIntermediateScore, cookingTime).SetId("fisrtGrillScoreIncrease"); ;

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            UIManager.Instance.grillInterface[0].gameObject.SetActive(false);
            UIManager.Instance.grillInterface[1].gameObject.SetActive(true);

            //AddCheeseButton == Desactivé, on active juste l'autre

            //Quand le score max est atteint, lance un chrono de perfectScoreSavingTime. Quand celui-ci est complet, lance CheeseBurning();
        }
        else
        {
            Debug.Log("Y'a déjà de la viande gros gourmand");
            return;
        }
    }
    public void FlipSideGrillBaking()
    {
        if (hasGrill)
        {
            //DoTween : Retourner la viande
            DOTween.Pause("grillScoreDecrease");

            //Trouver logique pour additionner la valeur au currentScore plutôt que de l'y emmener

            currentMaximumScore = grillMaximumScore + grillCurrentScore;

            DOTween.To(() => grillCurrentScore, x => grillCurrentScore = x, currentMaximumScore, cookingTime).SetId("secondGrillScoreIncrease"); ;

            //ajouter un délai léger ici, pour que les boutons se désactivent hors champ
            UIManager.Instance.grillInterface[1].gameObject.SetActive(false);
            UIManager.Instance.grillInterface[2].gameObject.SetActive(true);

            //Quand le score max est atteint, lance un chrono de perfectScoreSavingTime. Quand celui-ci est complet, lance CheeseBurning();
        }
        else
        {
            Debug.Log("Y'a déjà de la viande gros gourmand");
            return;
        }
    }
    public void SendToPlate()
    {
        //Caméra assiette + attends un clic dans la bonne position (bool isChoosingSpot, et dans assiette Interactable attends que ce bool soit validé ?)

        //GameManager.Instance.InstantiateCheesePrefab();

        DOTween.Pause("fisrtGrillScoreIncrease");
        DOTween.Pause("secondGrillScoreIncrease");

        DOTween.Pause("firstGrillScoreDecrease");
        DOTween.Pause("secondGrillScoreDecrease");

        GameManager.Instance.lookingForGrillSpot = true;

        GameManager.Instance.GetGrillData(grillCurrentScore);

        FeedbackManager.Instance.MoveCameraToPlate();
        UIManager.Instance.SlideOutGrillButtons();

        UIManager.Instance.cheeseInterface[0].gameObject.SetActive(true);
        UIManager.Instance.cheeseInterface[1].gameObject.SetActive(false);
        UIManager.Instance.cheeseInterface[2].gameObject.SetActive(false);
    }

    private void IntermediateGrillBurning()
    {
        if (intermediateGrillScoreReached && savingGrillScore)
        {
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
        DOTween.Pause("fisrtGrillScoreIncrease");
        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde");

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            IntermediateGrillBurning();
        });
    }
    private void MaxPerfectGrillSavingDelay()
    {
        savingGrillScore = true;
        DOTween.Pause("secondGrillScoreIncrease");
        /*float timer = 0f;
        DOTween.To(() => timer, x => timer = x, perfectScoreSavingTime, perfectScoreSavingTime); //Ajouter un délai ici*/
        Debug.Log("On lance le timer de sauvegarde");

        DOVirtual.DelayedCall(perfectScoreSavingTime, () =>
        {
            MaxGrillBurning();
        });
    }

    public void ResetGrillScore()
    {
        Destroy(instantiatedGrillPrefab);
        hasGrill = false;
        maxGrillScoreReached = false;
        intermediateGrillScoreReached = false;

        DOTween.Restart("fisrtGrillScoreIncrease");
        DOTween.Kill("fisrtGrillScoreIncrease");

        DOTween.Restart("secondGrillScoreIncrease");
        DOTween.Kill("secondGrillScoreIncrease");

        DOTween.Kill("grillScoreDecrease");

        grillCurrentScore = 0;
    }
}
