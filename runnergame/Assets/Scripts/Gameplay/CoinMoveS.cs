using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMoveS : MonoBehaviour
{
    public Transform target;

    AudioSource sfx;
    Collider2D coll2D;
    GameObject graph;
    ParticleSystem hitFx;
    // Start is called before the first frame update
    void Start()
    {
        graph = transform.GetChild(0).gameObject;
        sfx = GetComponent<AudioSource>();
        coll2D = GetComponent<Collider2D>();
        hitFx = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void GetHit()
    {
        sfx.Play();
        hitFx.time = 0f;
        hitFx.Play();

        UIS.Instance.updateCoin();

        coll2D.enabled = false;
        graph.SetActive(false);
        StartCoroutine(TimerToPool());
    }

    IEnumerator TimerToPool()
    {
        yield return new WaitForSeconds(1f);

        PoolCoinS.Instance.BackCoinToPool(gameObject);
    }

    private void OnEnable()
    {
        if (coll2D != null)
        {
            coll2D.enabled = true;
        }
        if (graph != null)
        {
            graph.SetActive(true);
        }
    }
    private void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, 30f * Time.deltaTime);
    }
}
