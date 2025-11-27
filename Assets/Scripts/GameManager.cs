using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Parameters")]
    public float remainingTime;
    public bool gameStarted;
    public TextMeshProUGUI remainingTimeText;

    public Vector3[] plateSpots;

    public int totalScore;

    public GameObject cheesePrefab;
    public GameObject grillPrefab;

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
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            CastRay();
        }

        if (gameStarted)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            remainingTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (remainingTime <= 0)
        {
            GameEnding();
        }
        //UIManager.Instance.currentPlateScore.text = spotsScore.ToString();
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

    public void GetPoelonRef(Poelon poelon)
    {
        currentPoelon = poelon;
    }


    public void AddToScore(int scoreReceived)
    {
        totalScore += scoreReceived;
    }

    public void GameEnding()
    {
        gameStarted = false;
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


public interface IInteractable
{
     void OnInteract();
}
