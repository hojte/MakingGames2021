using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using Interactions;
using PlayerScripts;
using Sound;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private List<DoorController> _doorControllers;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private void Awake()
    {
        // QuickFix for duplicate Controllers:
        if (FindObjectsOfType<GameController>().Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Transform camLookAtMe = FindObjectOfType<PlayerController>().transform.Find("CamLookAtMe"); 
        _cinemachineVirtualCamera.m_Follow = camLookAtMe;
        _cinemachineVirtualCamera.m_LookAt = camLookAtMe;
        
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/Crosshair.prefab", typeof(GameObject))));
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/ScoreUtil.prefab", typeof(GameObject))));
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/PickupCanvas.prefab", typeof(GameObject))));
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _doorControllers = FindObjectsOfType<DoorController>().ToList();
        _scoreController = FindObjectOfType<ScoreController>();
        levelStartTime = Time.time; // todo move statement to when player moves out of startRoom
        AudioUtility.CreateMainSFX(mainTheme);
    }

    void Update()
    {
        if (enemiesInCombat > 0)
        {
            if(debugMode) Debug.Log("in combat");
            if (_doorControllers?.Count > 0) _doorControllers.ForEach(door => door.doorLocked = true);
        }
        else if (_doorControllers?.Count > 0)
            _doorControllers.ForEach(door => door.doorLocked = false);
        if(Input.GetKeyDown(KeyCode.L)) _doorControllers = FindObjectsOfType<DoorController>().ToList();

    }

    public void LoadScene(string sceneName)
    {
        ((Func<Task>)(async () =>{ // Async call to restore prev conditions
            await Task.Delay(5000);
            var loadScene = SceneManager.LoadSceneAsync(sceneName);
            while (!loadScene.isDone)
            {
                await Task.Delay(20);
            }

            Debug.Log("Game Reloaded");
            UpdateReferences();
        }))();
    }

    private void UpdateReferences()
    {
        enemiesInCombat = 0;
        _doorControllers = FindObjectsOfType<DoorController>().ToList();
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