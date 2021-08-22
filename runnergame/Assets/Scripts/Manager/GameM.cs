using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;

public class GameM : MonoBehaviour
{
    public static GameM Instance;

    [Header("[ cam ]")]
    [SerializeField] CinemachineVirtualCamera vcam;
    [Header("[ canvas ]")]
    [SerializeField] GameObject htpCanvas;
    [SerializeField] GameObject settingCanvas;
    [SerializeField] GameObject resultCanvas;
    [Header("[ obj ]")]
    [SerializeField] PoolItemObsS poolItemObs;
    [Header("[ var ]")]
    public float moveSpeed = 5f;
    [SerializeField] float nextSpeed = 0;
    [Space]
    public int totCoin = 0;
    public bool isStart = false;
    public bool isPause = false;
    public bool isEnd = false;
    [Space]
    [SerializeField] int distance = 0;
    [Header("[ parallax bg ]")]
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

        nextSpeed = moveSpeed + 1;

        //set #1 item & obs
        poolItemObs.CreateItemObs();
        GetItemObstacles(groundCont.GetChild(1));

        //check tut
        if (GlobalVarS.Instance.isNewPlayer)
        {
            GlobalVarS.Instance.isNewPlayer = false;
            ShowHowToPlay(true);
        }

        //bgm
        SoundS.Instance.StopBgm(0);
        SoundS.Instance.PlayBgm(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause || isEnd)
        {
            return;
        }
        checkSpeedLevel();
        checkParallaxBg();
    }

    private void checkSpeedLevel()
    {
        distance ++;
        if (Mathf.Round(distance) % 20 == 0 && moveSpeed < nextSpeed)
        {
            moveSpeed++;
            nextSpeed++;
        }
    }

    private void checkParallaxBg()
    {
        //grounds
        for (int i = 0; i < groundCont.childCount; i++)
        {
            if (groundCont.GetChild(i).GetChild(1).transform.position.x < Camera.main.transform.position.x-12f)
            {
                //Debug.Log(groundCont.GetChild(i).GetChild(1).transform.position.x + " " + Camera.main.transform.position.x);
                Transform groundTrans = groundCont.GetChild(i);
                //check if any items in child
                if (groundTrans.childCount > 2)
                {
                    poolItemObs.BackItemObsToPool(groundTrans.GetChild(groundTrans.childCount - 1).gameObject);
                }
                groundTrans.transform.position = new Vector3(groundCont.GetChild(i + 1).GetChild(1).transform.position.x, groundTrans.transform.position.y, groundTrans.transform.position.z);
                groundTrans.SetAsLastSibling();

                //getitem & obs
                GetItemObstacles(groundTrans);
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

    private void GetItemObstacles(Transform groundTrans)
    {
        Transform itemObs = poolItemObs.GetItemObs().transform;
        itemObs.SetParent(groundTrans);
        itemObs.localPosition = Vector3.zero;
    }

    public void CameraShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 1f;
        DOTween.To(() => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, x => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = x, 0f, 1f);

    }

    #region menu event
    public void ShowHowToPlay(bool v)
    {
        htpCanvas.SetActive(v);
        isPause = v;
    }
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
