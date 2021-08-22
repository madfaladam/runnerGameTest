using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ResultS : MonoBehaviour
{
    [SerializeField] Button homeBtn;
    [SerializeField] Button replayBtn;
    [SerializeField] TMP_Text coinT;
    // Start is called before the first frame update
    void Start()
    {
        homeBtn.onClick.AddListener(ClickHome);
        replayBtn.onClick.AddListener(ClickReplay);
    }

    private void OnEnable()
    {
        //hide
        homeBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(false);

        int coinTemp = 0;

        DOTween.To(() => coinTemp, x => coinTemp = x, GameM.Instance.totCoin, 1f).SetDelay(0.5f).OnUpdate(()=>updateCoin(coinTemp)).OnComplete(FinishAnimCount);
        coinT.transform.parent.DOScale(1.2f, 0.25f).SetDelay(0.5f).SetLoops(2, LoopType.Yoyo);
    }

    private void updateCoin(int x)
    {
        coinT.text = x.ToString();
    }

    private void FinishAnimCount()
    {
        //hide
        homeBtn.gameObject.SetActive(true);
        replayBtn.gameObject.SetActive(true);

    }

    #region Click
    private void ClickReplay()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.ReplayGame();
    }

    private void ClickHome()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.BackToMenu();
    }
    #endregion
}
