using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuM : MonoBehaviour
{
    public static MenuM Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }  
    }

    #region menu event
    public void GotoGame()
    {
        SceneManager.LoadScene("Gameplay"); 
    }
    #endregion
}
