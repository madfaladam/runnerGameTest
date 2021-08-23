using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    DASH, SHIELD
}
public class PowerUpS : MonoBehaviour
{
    public PowerType powerType;

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

        coll2D.enabled = false;
        graph.SetActive(false);
    }

    private void OnEnable()
    {
        if (!GameM.Instance.showPowerUp)
        {
            return;
        }
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
