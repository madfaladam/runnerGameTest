﻿using UnityEngine;

public class GlobalVarS : MonoBehaviour
{
    public static GlobalVarS Instance;

    public bool isNewPlayer = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

}