using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingS : MonoBehaviour
{
    [SerializeField] Button homeBtn;
    [SerializeField] Button resumeBtn;
    [Space]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        homeBtn.onClick.AddListener(ClickHome);
        resumeBtn.onClick.AddListener(ClickResume);

        musicSlider.onValueChanged.AddListener(MusicValueChange);
        sfxSlider.onValueChanged.AddListener(SfxValueChange);
    }

    private void OnEnable()
    {
        Debug.Log(SoundS.Instance.GetBgmVol() + ", " + SoundS.Instance.GetSfxVol());
        musicSlider.value = SoundS.Instance.GetBgmVol();

        sfxSlider.value = SoundS.Instance.GetSfxVol();
    }
    #region Slider
    private void SfxValueChange(float v)
    {
        SoundS.Instance.ChangeSFXVol(v);
    }

    private void MusicValueChange(float v)
    {
        SoundS.Instance.ChangeBGMVol(v);
    }
    #endregion

    #region Click
    private void ClickResume()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.ShowSetting(false);
    }

    private void ClickHome()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.BackToMenu();
    }
    #endregion
}
