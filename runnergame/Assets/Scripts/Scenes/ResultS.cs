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
        GameM.Instance.ReplayGame();
    }

    private void ClickHome()
    {
        GameM.Instance.BackToMenu();
    }
    #endregion
}
