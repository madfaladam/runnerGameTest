using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinS : MonoBehaviour
{
    [SerializeField] Vector2 defaultPos;
    AudioSource sfx;
    Collider2D coll2D;
    GameObject graph;
    ParticleSystem hitFx;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
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
    }

    public void GetHitByCoinDetector(Transform targetTrans)
    {
        coll2D.enabled = false;
        graph.SetActive(false);
        //get pool
        GameObject coin = PoolCoinS.Instance.GetCoin();
        coin.SetActive(true);
        coin.transform.position = transform.position;
        CoinMoveS coinMoveS = coin.GetComponent<CoinMoveS>();
        coinMoveS.target = targetTrans;
    }
    private void OnEnable()
    {
        if (defaultPos != Vector2.zero)
        {
            transform.localPosition = defaultPos;
        }
        if (coll2D != null)
        {
            coll2D.enabled = true;
        }
        if (graph != null)
        {
            graph.SetActive(true);
        }
    }

}
