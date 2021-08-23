using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIS : MonoBehaviour
{
    public static UIS Instance;

    [Header("[ powerup ]")]
    [SerializeField] GameObject framePowerUpCont;
    [SerializeField] Image powerUpImg;
    [SerializeField] TMP_Text powerUpT;
    [SerializeField] Sprite[] powerUpSp;
    [Header("[ ui ]")]
    [SerializeField] Button settingBtn;
    [SerializeField] TMP_Text coinT;
    public Image touctAreaImg;

    int coinTemp = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
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

    #region powerup
    public void ShowPowerUp(PowerType powerType)
    {
        GameM.Instance.isPause = true;

        framePowerUpCont.SetActive(true);
        powerUpImg.sprite = powerUpSp[(int)powerType];
        powerUpT.text = powerType.ToString() + " !";
    }
    public void HidePowerUp()
    {
        GameM.Instance.isPause = false;

        framePowerUpCont.SetActive(false);
    }
    #endregion

    #region click
    private void ClickSetting()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.ShowSetting(true);
    }
    #endregion
}
