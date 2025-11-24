using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.CuttingButtonSpawns();
        gameObject.SetActive(false);
    }
}
