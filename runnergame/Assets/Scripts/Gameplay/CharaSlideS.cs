using System;
using System.Collections;
using UnityEngine;

public class CharaSlideS : MonoBehaviour
{
    CharaS charaS;
    [SerializeField] AudioSource slideSfx;

    // Start is called before the first frame update
    void Start()
    {
        charaS = GetComponent<CharaS>();
        charaS.DoSlide += DoSlide;
    }

    private void OnDestroy()
    {
        charaS.DoSlide -= DoSlide;
    }

    // Update is called once per frame
    public void DoSlide()
    {
        StartCoroutine(DoSlideIE());
    }

    IEnumerator DoSlideIE()
    {
        //sound
        slideSfx.Play();

        charaS.changeCollSize(1); 

        charaS.anim.SetTrigger("slide");
        charaS.graph.localPosition = Vector2.one * -0.3f;
        charaS.isSlide = true;

        yield return new WaitForSeconds(1f);

        charaS.changeCollSize(0); 

        charaS.anim.SetTrigger("run");
        charaS.graph.localPosition = Vector2.zero;
        charaS.isSlide = false;
    }
}
