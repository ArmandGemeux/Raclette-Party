using UnityEngine;

public class Raycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Camera mainCamera;
    public float maxDistance = 100f;
    public LayerMask interactableLayer; // Filtre pour éviter de toucher tout et n'importe quoi

    [Header("Debug")]
    public Color hitColor = Color.green;
    public float debugSphereSize = 0.1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            CastRay();
        }
    }

    void CastRay()
    {
        // Si la caméra n’est pas assignée, on prend celle de la scène
        if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, interactableLayer))
        {
            Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}");

            // 👇 Débug visuel dans la scène
            Debug.DrawLine(ray.origin, hitInfo.point, hitColor, 1.5f);
            Debug.DrawRay(hitInfo.point, hitInfo.normal * 0.2f, Color.red, 1.5f);

            // 🎯 Exemple d’interaction :
            var interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
        else
        {
            Debug.Log("No object hit.");
        }
    }
}

public interface IInteractable
{
    void OnInteract();
}
