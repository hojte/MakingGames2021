using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class replayableItems : MonoBehaviour
{
    string linkedLevelName;
    // Start is called before the first frame update
    void Start()
    {
        linkedLevelName = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.GetInt(linkedLevelName) == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
