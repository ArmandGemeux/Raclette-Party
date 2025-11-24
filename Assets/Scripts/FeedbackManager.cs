using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }


    [Header ("Camera and positions")]
    public Camera mainCamera;

    [Space]
    public Transform mainCameraPos;
    public Transform middleRightCamPos;
    public Transform grillCamPos;
    public Transform plateCamSpot;
    public Transform cuttingBoardCamPos;

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

    public void MoveCameraToInitialPosition()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(mainCameraPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(mainCameraPos.eulerAngles, 0.5f));
    }

    public void MoveCameraToMiddlePoelon()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(middleRightCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(middleRightCamPos.eulerAngles, 0.5f));
    }
    public void MoveCameraToGrill()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(grillCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(grillCamPos.eulerAngles, 0.5f));
    }
    public void MoveCameraToPlate()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(plateCamSpot.position, 0.5f))
            .Join(mainCamera.transform.DORotate(plateCamSpot.eulerAngles, 0.5f));
    }
    public void MoveCameraToCuttingBoard()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(cuttingBoardCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(cuttingBoardCamPos.eulerAngles, 0.5f));
    }
}
