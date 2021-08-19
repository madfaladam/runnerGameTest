using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundS : MonoBehaviour
{
    public static SoundS Instance;

    [SerializeField] AudioMixer audioMixer;

    GameObject audioBgm;
    GameObject audioSFX;
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        audioBgm = transform.GetChild(0).gameObject;
        audioSFX = transform.GetChild(1).gameObject;
    }
    #region get
    public float GetBgmVol()
    {
        float v = 0f;
        audioMixer.GetFloat("BGM_Vol", out v);
        return v;
    }
    public float GetSfxVol()
    {
        float v = 0f;
        audioMixer.GetFloat("SFX_Vol", out v);
        return v;
    }
    #endregion
    public void ChangeBGMVol(float v)
    {
        audioMixer.SetFloat("BGM_Vol", v);
    }
    public void ChangeSFXVol(float v)
    {
        audioMixer.SetFloat("SFX_Vol", v);
    }
    public void SetBGMOn(bool v)
    {

        if (v)
        {
            audioMixer.SetFloat("BGM_Vol", -5f);
        }
        else
        {
            audioMixer.SetFloat("BGM_Vol", -80f);
        }
    }
    public void SetSFXOn(bool v)
    {

        if (v)
        {
            audioMixer.SetFloat("SFX_Vol", 0f);
        }
        else
        {
            audioMixer.SetFloat("SFX_Vol", -80f);
        }
    }
    // Update is called once per frame
    public void PlayBgm(int ke)
    {
        audioBgm.transform.GetChild(ke).GetComponent<AudioSource>().Play();
    }
    public bool IsBGMPlaying(int ke)
    {
        return audioBgm.transform.GetChild(ke).GetComponent<AudioSource>().isPlaying;
    }
    // Update is called once per frame
    public void StopBgm(int ke)
    {
        audioBgm.transform.GetChild(ke).GetComponent<AudioSource>().Stop();
    }
    // Update is called once per frame
    public void PlaySfx(int ke)
    {
        audioSFX.transform.GetChild(ke).GetComponent<AudioSource>().Play();
    }

    public void PlaySfxDelay(int ke, float delay)
    {
        audioSFX.transform.GetChild(ke).GetComponent<AudioSource>().PlayDelayed(delay);
    }
    public bool IsSfxPlaying(int ke)
    {
        return audioSFX.transform.GetChild(ke).GetComponent<AudioSource>().isPlaying;
    }
    public void SetVolBgm(int ke, float vol)
    {
        audioBgm.transform.GetChild(ke).GetComponent<AudioSource>().volume = vol;
    }
}