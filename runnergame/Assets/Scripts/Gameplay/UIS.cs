using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIS : MonoBehaviour
{
    public static UIS Instance;

    [SerializeField] Button settingBtn;
    [SerializeField] TMP_Text coinT;
    int coinTemp = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        settingBtn.onClick.AddListener(ClickSetting);
    }

    public void updateCoin()
    {
        coinTemp++;
        coinT.text = coinTemp.ToString();

        coinT.transform.DOKill();
        coinT.transform.localScale = Vector3.one;
        coinT.transform.DOPunchScale(Vector3.one * 0.9f, 0.3f, 2, 0.2f);
    }

    #region click
    private void ClickSetting()
    {
        GameM.Instance.ShowSetting(true);
    }
    #endregion
}
