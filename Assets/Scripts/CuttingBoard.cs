using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour, IInteractable
{
    //bool isCutting = false;

    public GameObject foodToCutPrefab;
    public Transform spawnPosition;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        if (UIManager.Instance.isCuttingGameStarted == false)
        {
            Debug.Log("Click sur la planche à découper");

            FeedbackManager.Instance.MoveCameraToCuttingBoard();
            UIManager.Instance.CuttingBoardSetupOn();
        }
        else
        {
            return;
        }
    }

    public void DefiningCuttingChallenge()
    {
        UIManager.Instance.CuttingBoardSetupOn();

        //Instantiate(Un Prefab aléatoire de la liste foodToCutPrefab)
        //UIManager.Instance.CuttingBoardInterface();

        /*Elément à découper apparaît au centre de l'écran
        Un rond apparaît (opacité >> et scale >> jusqu'à être parfaitement visible, plutôt rapidement)
        Une fois présent, le rond se remplit (jauge comme la cuisson) rapidement, il faut appuyer dessus quand la jauge est pleine (ou avant, mais moins bon score)
        Score obtenu = maxPossibleScore * fillAmount (entre 0 et 1) arrondi en Int (ex : si score max = 100, 0.3 fill amout au clic = 30 points)
        Si le bouton n'est pas appuyé quand la jauge est pleine : timer de sauvegarde (très court, challenge), si timer de sauvegarde à 0, score = 0; 4.25  15.89 -3.16
        */
    }

    public void InstantiateFoodToCut()
    {
        GameObject go = Instantiate(foodToCutPrefab, spawnPosition.position, Quaternion.Euler(90f,0,-75));
        go.transform.DOScale(new Vector3(166, 166, 166), 0.1f);
    }
}
