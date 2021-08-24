using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    PowerType powerType;
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
    [SerializeField] ParticleSystem shieldFx;
    [SerializeField] Animator dashFx;
    [Space]
    [SerializeField] GameObject magnetPU;

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
            ObtacleS obtacleS = other.GetComponent<ObtacleS>();
            if (obtacleS != null)
            {
                obtacleS.GetHit();
            }
            else
            {
                ObstacleProjectileS obtacleS2 = other.GetComponent<ObstacleProjectileS>();
                obtacleS2.GetHit();
            }

            if (status == CharStatus.FIT)
            {
                dizzyCoroutine = StartCoroutine(TimerDizzy());
            }
            else if (status == CharStatus.DIZZY)
            {
                StopCoroutine(dizzyCoroutine);

                dieCoroutine = StartCoroutine(TimerDie());
            }
            //fx
            GameM.Instance.CameraShake(1f);
        }
        else if (other.gameObject.CompareTag("coin"))
        {
            GameM.Instance.totCoin++;
            CoinS coinS = other.GetComponent<CoinS>();
            coinS.GetHit();
        }
        else if (other.gameObject.CompareTag("coinD"))
        {
            GameM.Instance.totCoin++;
            CoinMoveS coinS = other.GetComponent<CoinMoveS>();
            coinS.GetHit();
        }
        else if (other.gameObject.CompareTag("powerup"))
        {
            PowerUpS pu = other.GetComponent<PowerUpS>();
            pu.GetHit();

            powerType = pu.powerType;
            if (pu.powerType == PowerType.SHIELD)
            {
                puShieldCoroutine = StartCoroutine(TimerShield());
            }
            else if (pu.powerType == PowerType.DASH)
            {
                puShieldCoroutine = StartCoroutine(TimerDash());
            }
            else if (pu.powerType == PowerType.MAGNET)
            {
                puShieldCoroutine = StartCoroutine(TimerMagnet());
            }
        }
    }

    #region getHit
    IEnumerator TimerDizzy()
    {
        //fx
        dizzyFx.time = 0f;
        dizzyFx.Play();

        status = CharStatus.DIZZY;

        anim.SetTrigger("hit");
        rb.AddForce(new Vector2(0f, 400f));

        //float speedTemp = GameM.Instance.moveSpeed;
        GameM.Instance.moveSpeed *= 0.8f;

        yield return new WaitForSeconds(0.5f);

        while (GameM.Instance.isPause)
        {
            yield return null;
        }

        anim.SetTrigger("run");
        //GameM.Instance.moveSpeed = speedTemp;

        yield return new WaitForSeconds(2.5f);

        while (GameM.Instance.isPause)
        {
            yield return null;
        }

        status = CharStatus.FIT;

    }

    IEnumerator TimerDie()
    {
        status = CharStatus.DIE;

        anim.SetTrigger("hit");

        rb.AddForce(new Vector2(0f, 400f));
        GameM.Instance.moveSpeed = 0f;
        GameM.Instance.isEnd = true;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        //rb.simulated = false;
        GameM.Instance.GameOver();
    }
    #endregion

    #region powerupIE
    IEnumerator TimerShield()
    {
        UIS.Instance.ShowPowerUp(powerType);
        yield return new WaitForSeconds(1f);
        UIS.Instance.HidePowerUp();
        //fx
        shieldFx.time = 0f;
        shieldFx.Play();

        isImmune = true;

        yield return new WaitForSeconds(3f);

        while (GameM.Instance.isPause)
        {
            yield return null;
        }

        isImmune = false;
        shieldFx.Stop();

        GameM.Instance.showPowerUp = false;

        GameM.Instance.currPowerShow--;
    }
    IEnumerator TimerDash()
    {
        UIS.Instance.ShowPowerUp(powerType);
        yield return new WaitForSeconds(1f);
        UIS.Instance.HidePowerUp();
        //fx
        dashFx.SetTrigger("play");
        dashFx.GetComponent<AudioSource>().PlayDelayed(0.2f);

        isImmune = true;
        yield return new WaitForSeconds(0.2f);

        float speedTemp = GameM.Instance.moveSpeed;
        GameM.Instance.moveSpeed *= 2f;
        //fx
        GameM.Instance.CameraShake(3f);

        yield return new WaitForSeconds(3f);

        while (GameM.Instance.isPause)
        {
            yield return null;
        }
        GameM.Instance.moveSpeed = speedTemp;

        isImmune = false;
        dashFx.SetTrigger("idle");

        GameM.Instance.showPowerUp = false;

        GameM.Instance.currPowerShow--;
    }

    IEnumerator TimerMagnet()
    {
        UIS.Instance.ShowPowerUp(powerType);
        yield return new WaitForSeconds(1f);
        UIS.Instance.HidePowerUp();

        magnetPU.SetActive(true);

        yield return new WaitForSeconds(3f);

        while (GameM.Instance.isPause)
        {
            yield return null;
        }
        magnetPU.SetActive(false);
    }
    #endregion

    #region controller
    public void OnEndDragDelegate(PointerEventData data)
    {
        if (!GameM.Instance.isStart || GameM.Instance.isEnd || GameM.Instance.isPause)
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
        if (!GameM.Instance.isStart || GameM.Instance.isEnd || GameM.Instance.isPause)
        {
            return;
        }
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
        if (GameM.Instance.isPause || GameM.Instance.isEnd)
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
