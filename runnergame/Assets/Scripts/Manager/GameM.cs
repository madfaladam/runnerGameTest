using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameM : MonoBehaviour
{
    public static GameM Instance;

    [Header("canvas")]
    [SerializeField] GameObject settingCanvas;
    [SerializeField] GameObject resultCanvas;
    [Header("var")]
    public int totCoin = 0;
    public bool isPause = false;
    public bool isEnd = false;
    [Header("parallax bg")]
    [SerializeField] Transform groundCont;
    [SerializeField] Transform bg0Cont;
    [SerializeField] Transform bg1Cont;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        checkParallaxBg();
    }

    private void checkParallaxBg()
    {
        //grounds
        for (int i = 0; i < groundCont.childCount; i++)
        {
            if (groundCont.GetChild(i).GetChild(1).transform.position.x < Camera.main.transform.position.x-12f)
            {
                //Debug.Log(groundCont.GetChild(i).GetChild(1).transform.position.x + " " + Camera.main.transform.position.x);

                groundCont.GetChild(i).transform.position = new Vector3(groundCont.GetChild(i + 1).GetChild(1).transform.position.x, groundCont.GetChild(i).transform.position.y, groundCont.GetChild(i).transform.position.z);
                groundCont.GetChild(i).SetAsLastSibling();
            }
        }

        //bg0
        for (int i = 0; i < bg0Cont.childCount; i++)
        {
            if (bg0Cont.GetChild(i).GetChild(1).transform.position.x < Camera.main.transform.position.x - 17f)
            {
                //Debug.Log(bg0Cont.GetChild(i).GetChild(1).transform.position.x + " " + Camera.main.transform.position.x);

                bg0Cont.GetChild(i).transform.position = new Vector3(bg0Cont.GetChild(i + 1).GetChild(1).transform.position.x, bg0Cont.GetChild(i).transform.position.y, bg0Cont.GetChild(i).transform.position.z);
                bg0Cont.GetChild(i).SetAsLastSibling();
            }
        }
        //bg1
        for (int i = 0; i < bg1Cont.childCount; i++)
        {
            if (bg1Cont.GetChild(i).GetChild(1).transform.position.x < Camera.main.transform.position.x - 24f)
            {
                //Debug.Log(bg1Cont.GetChild(i).GetChild(1).transform.position.x + " " + Camera.main.transform.position.x);

                bg1Cont.GetChild(i).transform.position = new Vector3(bg1Cont.GetChild(i + 1).GetChild(1).transform.position.x, bg1Cont.GetChild(i).transform.position.y, bg1Cont.GetChild(i).transform.position.z);
                bg1Cont.GetChild(i).SetAsLastSibling();
            }
        }
    }

    #region menu event
    public void ShowSetting(bool v)
    {
        settingCanvas.SetActive(v);
        isPause = v;
    }

    public void GameOver()
    {
        isEnd = true;
        resultCanvas.SetActive(true);
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
