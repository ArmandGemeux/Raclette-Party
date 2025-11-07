using UnityEngine;

public class Poelon : MonoBehaviour, IInteractable
{
    [Range(0, 10)]
    public int intensity;
    [Range(0, 1f)]
    public float intensityMultiplier;
    public bool hasCheese = false;
    public bool maxScoreReached = false;

    [Header("Cheese Values")]

    public int currentScore;
    [Space]
    public int startingScore;
    [Space]
    public int minimumScore;
    public int maximumScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCheese == true && maxScoreReached == false)
        {
            currentScore += (int)Mathf.Lerp(startingScore, maximumScore, intensity * intensityMultiplier);
        }

        if (hasCheese == true && currentScore >= maximumScore)
            maxScoreReached = true;

        if (hasCheese && maxScoreReached == true)
        {
            currentScore -= (int)Mathf.Lerp(currentScore, minimumScore, intensity * intensityMultiplier);
        }

        /*Fait un lerp entre le StartScore d'un fromage et son MaximumScore.
        Si le maximumScore est atteint, il est maintenu un instant (fourchette de score ? Timer ?) avant de déclencher un nouveau lerp (entre maximumScore et minimumScore, vitesse = intensity)
        Le changement de lerp se fait grâce à un changement d'état ("Cuit = true")*/
    }

    public void OnInteract()
    {
        Debug.Log("Oh oh, je suis un poélon coquinou ! :) ");
        hasCheese = true;

        /*Rapproche le poelon pour en examiner le contenu
        fait apparaître l'interface du fromage en cours de cuisson : Jauge de cuisson avec des crans dedans pour indiquer les états, et le score associé
        Affiche un slider de puissance entre 1 et 10, modifiable
        Un bouton sous le poélon -> Assiette
        Provoque un dézoom, on doit ensuite cliquer sur le spot désiré dans l'assiette (état selection du spot, on ne peut cliquer que sur ça).
        Quand clic fait, on instancie le fromage dans le spot, on le retire du poélon, il retourne dans l'appareil*/
    }
}
