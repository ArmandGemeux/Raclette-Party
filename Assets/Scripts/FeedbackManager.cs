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
    //public Transform middleRightCamPos;
    public Transform grillCamPos;
    public Transform plateCamSpot;
    public Transform cuttingBoardCamPos;
    public Transform finalCamPos;

    public float cameraShakeForce;
    public float cameraShakeDuration;
    public int cameraShakeVibrato;

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
    public void MoveCameraToFinalPosition()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(finalCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(finalCamPos.eulerAngles, 0.5f));
    }

    public void MoveCameraToPoelon(Transform poelonCamPos)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(poelonCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(poelonCamPos.eulerAngles, 0.5f));
    }
    public void MoveCameraToGrill()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(grillCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(grillCamPos.eulerAngles, 0.5f));
    }

    /*public void MoveCameraToPlate()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(plateCamSpot.position, 0.5f))
            .Join(mainCamera.transform.DORotate(plateCamSpot.eulerAngles, 0.5f));
    }*/

    public void MoveCameraToCuttingBoard()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(cuttingBoardCamPos.position, 0.5f))
            .Join(mainCamera.transform.DORotate(cuttingBoardCamPos.eulerAngles, 0.5f));
    }

    public void shakeCamera()
    {
        mainCamera.DOShakePosition(cameraShakeDuration, cameraShakeForce, cameraShakeVibrato, 90, true);
    }
}
