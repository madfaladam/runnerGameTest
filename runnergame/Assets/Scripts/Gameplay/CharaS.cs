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
    public Collider2D[] coll2d;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    [Header("move")]
    [SerializeField] Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;
    public bool isGrounded;
    public bool isSlide = false;
    [SerializeField] float moveSpeed = 5f;
    Vector3 m_Velocity = Vector3.zero;
    float movementX = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("getHit by " + other.gameObject.name);
            if (status == CharStatus.FIT)
            {
                status = CharStatus.DIZZY;
                StartCoroutine(TimerDizzy());
            }
            else if (status == CharStatus.DIZZY)
            {
                StopAllCoroutines();
                status = CharStatus.DIE;
                StartCoroutine(TimerDie());
            }
        }
        else if (other.gameObject.CompareTag("coin"))
        {
            GameM.Instance.totCoin++;
            other.gameObject.SetActive(false);
            UIS.Instance.updateCoin();
        }
        else if (other.gameObject.CompareTag("powerup"))
        {

        }
    }

    IEnumerator TimerDizzy()
    {
        anim.SetTrigger("hit");
        rb.AddForce(new Vector2(0f, 400f));
        float speedTemp = moveSpeed;
        moveSpeed = 20f;
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("run");
        moveSpeed = speedTemp;
        yield return new WaitForSeconds(2.5f);
        status = CharStatus.FIT;
    }

    IEnumerator TimerDie()
    {
        anim.SetTrigger("hit");
        rb.AddForce(new Vector2(0f, 400f));
        moveSpeed = 0f;
        GameM.Instance.isEnd = true;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        GameM.Instance.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (GameM.Instance.isEnd)
        {
            return;
        }
        if (GameM.Instance.isPause)
        {
            movementX = 0f;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            return;
        }
        if (!rb.simulated)
        {
            rb.simulated = true;
            movementX = 1f;
        }

        Move();

        checkGrounded();
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2((movementX * moveSpeed * Time.fixedDeltaTime) * 10f, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, 0.05f);
    }

    private void checkGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //Debug.Log("isGrounded: " + isGrounded + ", wasGrounded: " + wasGrounded);
                isGrounded = true;
                if (!wasGrounded)
                {
                }
                else
                {
                    if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HeroAnim_run")
                    {
                        anim.SetTrigger("run");
                    }
                }
            }
        }
    }
}
