using System;
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
    private void Awake()
    {
        Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/Crosshair.prefab", typeof(GameObject)));
        Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/ScoreUtil.prefab", typeof(GameObject)));
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _scoreController = FindObjectOfType<ScoreController>();
        levelStartTime = Time.time; // todo move to when move out of startRoom
        AudioUtility.CreateMainSFX(mainTheme);
    }

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