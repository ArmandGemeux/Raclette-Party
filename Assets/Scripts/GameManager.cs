using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Vector3[] plateSpots;

    public int spotsScore;

    public GameObject cheesePrefab;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plateSpots = new Vector3[plateSpots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateCheesePrefab()
    {
        Instantiate(cheesePrefab, new Vector3(0,0,0), Quaternion.identity);
        //ne s'instantie pas au bon endroit
    }
}
