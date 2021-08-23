using UnityEngine;

public class ObtacleS : MonoBehaviour
{
    Collider2D coll2D;
    GameObject graph;
    ParticleSystem hitFx;
    // Start is called before the first frame update
    void Start()
    {
        graph = transform.GetChild(0).gameObject;
        coll2D = GetComponent<Collider2D>();
        hitFx = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void GetHit()
    {
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
