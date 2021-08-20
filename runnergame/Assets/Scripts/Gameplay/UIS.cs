using System;
using UnityEngine;
using UnityEngine.UI;

public class UIS : MonoBehaviour
{
    [SerializeField] Button settingBtn;

    // Start is called before the first frame update
    void Start()
    {
        settingBtn.onClick.AddListener(ClickSetting);
    }

    #region click
    private void ClickSetting()
    {
        throw new NotImplementedException();
    }
    #endregion
}
