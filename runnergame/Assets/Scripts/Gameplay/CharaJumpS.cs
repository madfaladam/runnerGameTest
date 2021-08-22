using UnityEngine;

public class CharaJumpS : MonoBehaviour
{
    CharaS charaS;
    [Header("jump")]
    [SerializeField] float jumpVel = 10f;
    [SerializeField] AudioSource jumpSfx;
    // Start is called before the first frame update
    void Start()
    {
        charaS = GetComponent<CharaS>();
        charaS.Dojump += DoJump;
    }

    private void OnDestroy()
    {
        charaS.Dojump -= DoJump;
    }
    // Update is called once per frame
    public void DoJump()
    {
        if (charaS.isGrounded)
        {
            //sound
            jumpSfx.Play();

            charaS.graph.localPosition = Vector2.zero;

            charaS.rb.AddForce(new Vector2(0f, jumpVel));
            charaS.anim.SetTrigger("jump");
        }
    }
}
