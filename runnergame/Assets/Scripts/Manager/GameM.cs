using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameM : MonoBehaviour
{
    public static GameM Instance;

    [Header("canvas")]
    [SerializeField] GameObject settingCanvas;
    [SerializeField] GameObject resultCanvas;
    [Header("var")]
    public bool isPause = false;
    public bool isEnd = false;

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
        
    }

    #region menu event
    public void ShowSetting()
    {
        settingCanvas.SetActive(true);
        isPause = true;
    }

    public void GameOver()
    {
        isEnd = true;
        resultCanvas.SetActive(true);
    }
    #endregion
}
