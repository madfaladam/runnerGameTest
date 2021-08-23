using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum CharStatus
{
    FIT, DIZZY, DIE
}

public class CharaS : MonoBehaviour
{
    [SerializeField] CharStatus status;

    [HideInInspector] CapsuleCollider2D coll2d;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform graph;

    [Header("[ move ]")]
    [SerializeField] Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;
    [Header("[ var ]")]
    public bool isGrounded;
    public bool isSlide = false;
    bool isImmune = false;

    Vector3 m_Velocity = Vector3.zero;
    float movementX = 1f;

    public delegate void DoJumpDelegate();
    public DoJumpDelegate Dojump;
    public delegate void DoSlideDelegate();
    public DoSlideDelegate DoSlide;

    [Header("[ sfx ]")]
    [SerializeField] AudioSource hitSfx;
    [Header("[ fx ]")]
    [SerializeField] ParticleSystem dizzyFx;
    [SerializeField] ParticleSystem hitFx;
    [SerializeField] ParticleSystem shieldFx;


    Coroutine dizzyCoroutine;
    Coroutine dieCoroutine;
    Coroutine puShieldCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        graph = transform.GetChild(0);
        coll2d = GetComponent<CapsuleCollider2D>();

        //controller
        EventTrigger trigger = UIS.Instance.touctAreaImg.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void changeCollSize(int i)
    {
        if (i == 0)
        {
            coll2d.offset = Vector2.zero;
            coll2d.size = new Vector2(0.92f, 1.85f);
        }
        else
        {
            coll2d.offset = new Vector2(0f, -0.32f);
            coll2d.size = new Vector2(1.1f, 1.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameM.Instance.isEnd)
        {
            return;
        }
        if (other.gameObject.CompareTag("triggerStart"))
        {
            GameM.Instance.isStart = true;
        }
        else if (other.gameObject.CompareTag("obstacle"))
        {
            if (isImmune)
            {
                return;
            }
            //sound
            hitSfx.Play();

            Debug.Log("getHit by " + other.gameObject.name);

            if (status == CharStatus.FIT)
            {
                status = CharStatus.DIZZY;
                dizzyCoroutine = StartCoroutine(TimerDizzy());
            }
            else if (status == CharStatus.DIZZY)
            {
                StopCoroutine(dizzyCoroutine);
                status = CharStatus.DIE;
                dieCoroutine = StartCoroutine(TimerDie());
            }
            other.gameObject.SetActive(false);
            GameM.Instance.CameraShake();
        }
        else if (other.gameObject.CompareTag("coin"))
        {
            GameM.Instance.totCoin++;
            CoinS coinS = other.GetComponent<CoinS>();
            coinS.GetHit();
        }
        else if (other.gameObject.CompareTag("powerup"))
        {
            PowerUpS pu = other.GetComponent<PowerUpS>();
            pu.GetHit();
            if (pu.powerType == PowerType.SHIELD)
            {
                puShieldCoroutine = StartCoroutine(TimerShield());
            }
        }
    }

    IEnumerator TimerDizzy()
    {
        //fx
        hitFx.time = 0f;
        hitFx.Play();
        dizzyFx.time = 0f;
        dizzyFx.Play();

        anim.SetTrigger("hit");
        rb.AddForce(new Vector2(0f, 400f));

        float speedTemp = GameM.Instance.moveSpeed;
        GameM.Instance.moveSpeed *= 0.8f;

        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("run");
        GameM.Instance.moveSpeed = speedTemp;

        yield return new WaitForSeconds(2.5f);
        status = CharStatus.FIT;

    }

    IEnumerator TimerDie()
    {
        anim.SetTrigger("hit");
        rb.AddForce(new Vector2(0f, 400f));
        GameM.Instance.moveSpeed = 0f;
        GameM.Instance.isEnd = true;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        //rb.simulated = false;
        GameM.Instance.GameOver();
    }

    IEnumerator TimerShield()
    {
        //fx
        shieldFx.time = 0f;
        shieldFx.Play();

        isImmune = true;

        yield return new WaitForSeconds(3f);

        isImmune = false;
        shieldFx.Stop();

        GameM.Instance.showPowerUp = false;

    }
    #region controller
    public void OnEndDragDelegate(PointerEventData data)
    {
        if (!GameM.Instance.isStart)
        {
            return;
        }
        if (GameM.Instance.isEnd)
        {
            return;
        }
        //Debug.Log("delta: "+data.delta.y);
        //Debug.Log("isDrag: " + data.dragging);
        if (data.delta.y < 0)
        {
            if (!isGrounded)
            {
                return;
            }
            //slide
            DoSlide.Invoke();
        }
        else if (data.delta.y > 0)
        {
            //jump
            Dojump.Invoke();
        }
    }
    #endregion
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isGrounded)
            {
                return;
            }
            //slide
            DoSlide.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump
            Dojump.Invoke();
        }
#endif
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
        Vector3 targetVelocity = new Vector2((movementX * GameM.Instance.moveSpeed * Time.fixedDeltaTime) * 10f, rb.velocity.y);

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
                    changeCollSize(0);
                }
                else
                {
                    if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HeroAnim_run" && !isSlide)
                    {
                        anim.SetTrigger("run");
                    }
                }
            }
        }
    }
}
