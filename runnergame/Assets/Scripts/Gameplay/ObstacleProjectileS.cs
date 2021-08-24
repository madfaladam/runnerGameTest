using System;
using System.Collections;
using UnityEngine;

public class ObstacleProjectileS : MonoBehaviour
{
    Collider2D coll2D;
    GameObject graph;
    ParticleSystem hitFx;
    Rigidbody2D rb;

    Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        graph = transform.GetChild(0).gameObject;
        coll2D = GetComponent<Collider2D>();
        hitFx = transform.GetChild(1).GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(TimerDestroy());
    }

    private void FixedUpdate()
    {
        if (!GameM.Instance.isStart || GameM.Instance.isPause || GameM.Instance.isEnd)
        {
            return;
        }
        Vector3 targetVelocity = new Vector2((-1 * 50f * Time.fixedDeltaTime) * 10f, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, 0.05f);
    }

    public void GetHit()
    {
        rb.simulated = false;
        hitFx.time = 0f;
        hitFx.Play();

        coll2D.enabled = false;
        graph.SetActive(false);

    }

    IEnumerator TimerDestroy()
    {
        yield return new WaitForSeconds(3f);
        UIS.Instance.HideWarning();
        GameM.Instance.currObsProjectile--;

        Destroy(gameObject);
    }
}
