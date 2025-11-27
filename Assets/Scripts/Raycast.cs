using UnityEngine;

public class Raycast : MonoBehaviour
{
    [Header("Raycast")]
    public Camera mainCamera;
    public float maxDistance = 100f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameStarted) // clic gauche
        {
            CastRay();
        }
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
                Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}");

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
            Debug.Log("Impossible d'intéragir avec de l'UI à l'écran enfin !");
            return;
        }
    }
}
