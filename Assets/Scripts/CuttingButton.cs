using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class CuttingButton : MonoBehaviour, IPointerDownHandler
{
    public int scoreOnClick;
    public int maximumScore;
    public float clickWindowDuration;
    public float perfectScoreSaving;

    public Image jaugeImage;

    public void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(perfectScoreSaving).Append(jaugeImage.DOFillAmount(0, clickWindowDuration).SetEase(Ease.InSine))
           .Join(jaugeImage.DOColor(Color.darkRed, clickWindowDuration).SetEase(Ease.InSine)).OnComplete(() => {
               ButtonFinished();
               }).SetId("SequenceBoutton");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonFinished();
    }

    public void ButtonFinished()
    {
        DOTween.Pause("SequenceBoutton");
        scoreOnClick = (int)(jaugeImage.fillAmount * maximumScore);
        GameManager.Instance.AddToScore(scoreOnClick);

        gameObject.transform.DOScale(new Vector3(0, 0, 0), .2f).SetEase(Ease.InOutQuart).OnComplete(() => {
            UIManager.Instance.CuttingButtonSpawns();
            gameObject.SetActive(false);
            });
    }
}
