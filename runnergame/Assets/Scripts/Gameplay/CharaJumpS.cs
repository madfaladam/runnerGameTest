using UnityEngine;

public class CharaJumpS : MonoBehaviour
{
    CharaS charaS;
    [Header("jump")]
    [SerializeField] float jumpVel = 10f;

    // Start is called before the first frame update
    void Start()
    {
        charaS = GetComponent<CharaS>();
    }

    // Update is called once per frame
    public void DoJump()
    {
        if (charaS.isGrounded)
        {
            charaS.rb.AddForce(new Vector2(0f, jumpVel));
            charaS.anim.SetTrigger("jump");
        }
    }
}
