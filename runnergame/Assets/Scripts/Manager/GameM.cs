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
    [Header("[ pool ]")]
    [SerializeField] PoolItemObsS poolItemObs;
    [Header("[ obstacle projectile ]")]
    public int currObsProjectile = 0;
    [Header("[ powerup ]")]
    public bool showPowerUp = false;
    public int currPowerShow = 0;
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
        checkSpeedLevel();
        checkShowPowerUp();
        checkShowObtacleProjectile();
        checkParallaxBg();
    }

    private void checkSpeedLevel()
    {
        if (!isStart || isPause || isEnd)
        {
            return;
        }

        distance ++;
        if (distance > 0 && Mathf.Round(distance) % 25 == 0 && moveSpeed < nextSpeed)
        {
            moveSpeed++;
            nextSpeed++;
        }
    }
    private void checkShowPowerUp()
    {
        if (distance > 0 && Mathf.Round(distance) % 30 == 0 && !showPowerUp)
        {
            Debug.Log("showPowerUp: " + showPowerUp);
            showPowerUp = true;
        }
    }
    private void checkShowObtacleProjectile()
    {
        if (distance > 0 && Mathf.Round(distance) % 250 == 0 && currObsProjectile < 1)
        {
            currObsProjectile++;
            UIS.Instance.ShowWarning();
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

        //check powerup
        for (int i = 0; i < itemObs.childCount; i++)
        {
            if (itemObs.GetChild(i).CompareTag("powerup"))
            {
                if (showPowerUp && currPowerShow < 1)
                {
                    itemObs.GetChild(i).gameObject.SetActive(true);
                    currPowerShow++;
                }
                else
                {
                    itemObs.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void CameraShake(float v)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 1f;
        DOTween.To(() => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, x => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = x, 0f, v);

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
