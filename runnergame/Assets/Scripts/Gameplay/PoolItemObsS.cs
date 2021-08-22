using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItemObsS : MonoBehaviour
{
    [SerializeField] int totItemObs;

    // Start is called before the first frame update
    public void CreateItemObs()
    {
        //add
        for (int i = 0; i < totItemObs; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("objects/ObjectCont" + i), transform);
            obj.SetActive(false);
        }
    }

    public GameObject GetItemObs()
    {
        int rand = Random.Range(0, transform.childCount);
        for (int j = 0; j < transform.childCount; j++)
        {
            if (rand == j)
            {
                Transform objRand = transform.GetChild(rand);
                objRand.gameObject.SetActive(true);
                return objRand.gameObject;
            }

        }

        Debug.Log("not itemobs found!");
        return null;
    }
    public void BackItemObsToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        //set default item

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).gameObject.name.Contains("tunnel"))
            {
                obj.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                obj.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
