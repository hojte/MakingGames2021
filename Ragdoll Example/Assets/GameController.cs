using System.Collections;
using System.Collections.Generic;
using Sound;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played continously in 2D")]
    public AudioClip mainTheme;
    [Tooltip("Mute all sound")]
    public bool muteSound;
    [Header("MISC")]
    [Tooltip("Toggle global debug for entire game to show/do various things")]
    public bool debugMode;
    
    public int enemiesInCombat = 0;

    public float levelStartTime;
    public float levelCompleteTime;
    public Vector3 checkPoint;

    private ScoreController _scoreController;
    // Start is called before the first frame update
    void Start()
    {
        levelStartTime = Time.time;
        _scoreController = FindObjectOfType<ScoreController>();
        AudioUtility.CreateMainSFX(mainTheme);
        Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/Crosshair.prefab", typeof(GameObject)));
        Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/ScoreUtil.prefab", typeof(GameObject)));
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInCombat > 0)
            Debug.Log("in combat");
    }

    public void newEnemyInCombat()
    {
        enemiesInCombat++;
        Debug.Log("enemy engaged");
    }

    public void enemySlain()
    {
        if (enemiesInCombat > 0)
        {
            enemiesInCombat--;
            _scoreController.EnemyKilled();
            
        }
        Debug.Log("enemy killed");
    }

    public int getEnemiesInCombat()
    {
        return enemiesInCombat;
    }
}
