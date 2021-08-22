using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuS : MonoBehaviour
{
    [SerializeField] Button playBtn;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(ClickPlay);    
    }

    #region Click
    private void ClickPlay()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        MenuM.Instance.GotoGame();
    }
    #endregion
}
