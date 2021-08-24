using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningObS : MonoBehaviour
{
    [SerializeField] GameObject[] projectilePrefs;
    Image img;
    AudioSource sfx;
    RectTransform rectTransform;

    private void OnEnable()
    {
        if (img == null)
        {
            img = GetComponent<Image>();
        }
        img.enabled = true;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        if (sfx == null)
        {
            sfx = GetComponent<AudioSource>();
        }
        sfx.Play();

        StartCoroutine(TimerWarning());
    }

    IEnumerator TimerWarning()
    {
        sfx.time = 0f;
        sfx.Play();

        yield return new WaitForSeconds(2f);

        img.enabled = false;
        sfx.Stop();

        //pos
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, rectTransform.position, Camera.main, out pos);

        int rand = 0;
        if (UnityEngine.Random.Range(0, 10) > 4)
        {
            rand = 1;
        }
        GameObject projectile = Instantiate(projectilePrefs[rand], new Vector3(Camera.main.transform.position.x + 10f, pos.y, 0f), Quaternion.identity);
        Debug.Log("add projectile: " + projectile.transform.position);
    }
}
