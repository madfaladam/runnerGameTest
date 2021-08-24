using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCoinS : MonoBehaviour
{
    public static PoolCoinS Instance;

    [SerializeField] GameObject coinPref;
    [SerializeField] int maxCount = 100;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        for (int i = 0; i < maxCount; i++)
        {
            GameObject coinObj = Instantiate(coinPref, transform);
            coinObj.SetActive(false);
        }
    }

    public GameObject GetCoin()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                return transform.GetChild(i).gameObject;
            }
        }

        Debug.Log("no obj found");
        return null;
    }

    public void BackCoinToPool(GameObject coinObj)
    {
        coinObj.transform.SetParent(transform);
        coinObj.SetActive(false);
    }
}
