using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingS : MonoBehaviour
{
    [SerializeField] Button homeBtn;
    [SerializeField] Button resumeBtn;

    // Start is called before the first frame update
    void Start()
    {
        homeBtn.onClick.AddListener(ClickHome);
        resumeBtn.onClick.AddListener(ClickResume);
    }
    #region Click
    private void ClickResume()
    {
        GameM.Instance.ShowSetting(false);
    }

    private void ClickHome()
    {
        GameM.Instance.BackToMenu();
    }
    #endregion
}
