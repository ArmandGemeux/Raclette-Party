using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform[] cheeseButtons;

    [Space]
    public float slideInDuration = 0.6f;
    public float slideOutDuration = 0.4f;
    public float delayBetweenButtons = 0.2f;

    private Vector2[] targetPos;

    public bool UIOnScreen;

    public Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = new Vector2[cheeseButtons.Length];

        for (int i = 0; i < cheeseButtons.Length; i++)
        {
            RectTransform btn = cheeseButtons[i];

            targetPos[i] = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(-1500f, targetPos[i].y);
        }
    }

    public void SlideInButtons()
    {
        UIOnScreen = true;

        for (int i = 0; i < cheeseButtons.Length; i++)
        {
            RectTransform btn = cheeseButtons[i];

            btn.DOAnchorPos(targetPos[i], slideInDuration).SetEase(Ease.OutBack).SetDelay(i * delayBetweenButtons);
        }
    }
    public void SlideOutButtons()
    {
        UIOnScreen = false;

        for (int i = 0; i < cheeseButtons.Length; i++)
        {
            RectTransform btn = cheeseButtons[i];

            btn.DOAnchorPos(new Vector2(-1500f, targetPos[i].y), slideOutDuration).SetEase(Ease.InOutBack).SetDelay(i * delayBetweenButtons);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(new Vector3(1.85f, 1f, 0f), 0.5f))
            .Join(mainCamera.transform.DORotate(new Vector3(25f, -90f, 0f), 0.5f));
    }
}
