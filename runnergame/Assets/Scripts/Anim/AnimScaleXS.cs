using UnityEngine;
using DG.Tweening;

public class AnimScaleXS : MonoBehaviour
{
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target.DOScaleX(0f, 0.35f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
