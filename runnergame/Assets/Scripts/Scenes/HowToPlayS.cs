using UnityEngine;
using UnityEngine.UI;

public class HowToPlayS : MonoBehaviour
{
    [SerializeField] Button closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(ClickClose);
    }

    #region click
    private void ClickClose()
    {
        //sound
        SoundS.Instance.PlaySfx(0);

        GameM.Instance.ShowHowToPlay(false);
    }
    #endregion
}
