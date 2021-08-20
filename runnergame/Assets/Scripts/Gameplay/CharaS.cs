using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharStatus
{
    FIT, DIZZY, DIE
}

public class CharaS : MonoBehaviour
{
    [SerializeField] CharStatus status;
    [SerializeField] Collider2D[] coll2d;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            if (status == CharStatus.FIT)
            {
                status = CharStatus.DIZZY;
                StartCoroutine(TimerDizzy());
            }
            else if (status == CharStatus.DIZZY)
            {
                StopAllCoroutines();
                status = CharStatus.DIE;
                GameM.Instance.GameOver();
            }
        }
    }

    IEnumerator TimerDizzy()
    {
        yield return new WaitForSeconds(3f);
        status = CharStatus.FIT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
