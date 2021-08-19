using UnityEngine;
using DG.Tweening;

public class AnimPopUpS : MonoBehaviour
{
    [SerializeField] GameObject frameObj;

    private void OnEnable()
    {
        //anim
        frameObj.transform.localScale = Vector2.one * 0.2f;
        frameObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
}