using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoomTrigger : MonoBehaviour
{
    public String sceneToLoad; 
    [Tooltip("The amount of time that is expected for the player to complete this particular level")]
    public int expectedSecondsToCompleteLevel = 60; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<ScoreController>().LevelCompleted(expectedSecondsToCompleteLevel);
            FindObjectOfType<GameController>().LoadScene(sceneToLoad);
        }
        
    }
}
