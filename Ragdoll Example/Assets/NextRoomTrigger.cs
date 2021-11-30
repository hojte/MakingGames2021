using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoomTrigger : MonoBehaviour
{
    public String sceneToLoad; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<ScoreController>().LevelCompleted();
            FindObjectOfType<GameController>().LoadScene(sceneToLoad);
        }
        
    }
}
