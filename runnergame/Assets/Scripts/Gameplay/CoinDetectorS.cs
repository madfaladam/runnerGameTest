using UnityEngine;

public class CoinDetectorS : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            CoinS coinS = collision.GetComponent<CoinS>();
            coinS.GetHitByCoinDetector(transform.parent);
        }
    }
}
