using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Vector3[] plateSpots;

    public int totalScore;

    public GameObject cheesePrefab;
    public GameObject grillPrefab;

    public bool lookingForCheeseSpot = false;
    public bool lookingForGrillSpot = false;

    [Header("FoodMakers")]
    public Poelon poelon;
    public Grill grill;

    [Header("Raycast")]
    public Camera mainCamera;
    public float maxDistance = 100f;
    public LayerMask interactableLayer;

    private Poelon currentPoelon;
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

        //DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(2000, 100);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plateSpots = new Vector3[plateSpots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && lookingForCheeseSpot == false && lookingForGrillSpot == false) // clic gauche
        {
            CastRay();
        }
        else if (Input.GetMouseButtonDown(0) && lookingForCheeseSpot)
        {
            LookForCheeseSpot();
        }
        else if (Input.GetMouseButtonDown(0) && lookingForGrillSpot)
        {
            LookForGrillSpot();
        }

        //UIManager.Instance.currentPlateScore.text = spotsScore.ToString();
    }

    public void InstantiateCheesePrefab(Vector3 instantiationPos)
    {
        Instantiate(cheesePrefab, instantiationPos, Quaternion.identity);
        lookingForCheeseSpot = false;

        currentPoelon.ResetCheeseScore();
        currentPoelon = null;

        Debug.Log("Fromage dans l'assiette !");
    }
    public void InstantiateGrillPrefab(Vector3 instantiationPos)
    {
        Instantiate(grillPrefab, instantiationPos, Quaternion.identity);
        lookingForGrillSpot = false;

        grill.ResetGrillScore();

        Debug.Log("Viande dans l'assiette !");
    }

    void CastRay()
    {
        if (!UIManager.Instance.UIOnScreen)
        {
            // Si la caméra n’est pas assignée, on prend celle de la scène
            if (mainCamera == null)
                mainCamera = Camera.main;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, interactableLayer))
            {
                //Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}");

                var interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
            else
            {
                //Debug.Log("No object hit.");
            }
        }
        else
        {
            //Debug.Log("Impossible d'intéragir avec de l'UI à l'écran enfin !");
            return;
        }
    }
    public void LookForCheeseSpot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, interactableLayer))
        {
            //Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}");

            var interactable = hitInfo.collider.GetComponent<Transform>();
            InstantiateCheesePrefab(interactable.position);
            FeedbackManager.Instance.MoveCameraToInitialPosition();
        }
        else
        {
            //Debug.Log("No object hit.");
        }
    }

    public void GetPoelonRef(Poelon poelon)
    {
        currentPoelon = poelon;
    }

    public void LookForGrillSpot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, interactableLayer))
        {
            //Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}");

            var interactable = hitInfo.collider.GetComponent<Transform>();
            InstantiateGrillPrefab(interactable.position);
            FeedbackManager.Instance.MoveCameraToInitialPosition();
        }
        else
        {
            //Debug.Log("No object hit.");
        }
    }

    public void AddToScore(int scoreReceived)
    {
        totalScore += scoreReceived;
    }
}


public interface IInteractable
{
     void OnInteract();
}
