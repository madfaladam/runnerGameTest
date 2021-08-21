using System;
using System.Collections;
using UnityEngine;

public class CharaSlideS : MonoBehaviour
{
    CharaS charaS;

    // Start is called before the first frame update
    void Start()
    {
        charaS = GetComponent<CharaS>();
    }

    // Update is called once per frame
    public void DoSlide()
    {
        StartCoroutine(DoSlideIE());
    }

    IEnumerator DoSlideIE()
    {

        charaS.coll2d[0].enabled = false;
        charaS.coll2d[1].enabled = true;
        charaS.anim.SetTrigger("slide");
        yield return new WaitForSeconds(2f);

        charaS.coll2d[0].enabled = true;
        charaS.coll2d[1].enabled = false;
        charaS.anim.SetTrigger("run");
    }
}
