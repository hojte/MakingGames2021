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
    [Tooltip("Sound played continuously while out of combat")]
    public AudioClip onOutOfCombat;
    [Tooltip("Sound played continuously while in normal combat")]
    public AudioClip onCombat;
    [Tooltip("Sound played continuously while in boss combat")]
    public AudioClip onBossCombat;
    [Tooltip("Mute all sound")]
    public bool muteSound;
    [Header("MISC")]
    [Tooltip("Toggle global debug for entire game to show/do various things")]
    public bool debugMode;
    [Tooltip("Will use force sun - no shadows (if no other lights in the scene)")]
    public bool forceSun = true;
    
    public int enemiesInCombat = 0;

    public float levelStartTime;
    public float levelCompleteTime;
    public Vector3 checkPoint;

    private ScoreController _scoreController;
    private List<DoorController> _doorControllers;
    public CinemachineVirtualCamera _cinemachineVirtualCamera;
    public Transform _camLookAtMe;
    private bool combatMusicPlaying = false;
    public bool bossCombat = false; // todo set this in the boss fight
    private AudioSource _audioSource;
    private void Awake()
    {
        // QuickFix for duplicate Controllers:
        if (FindObjectsOfType<GameController>().Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _camLookAtMe = FindObjectOfType<PlayerController>().transform.Find("CamLookAtMe").transform; 
        _cinemachineVirtualCamera.m_Follow = _camLookAtMe;
        _cinemachineVirtualCamera.m_LookAt = _camLookAtMe;
        
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/Crosshair.prefab", typeof(GameObject))));
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/ScoreUtil.prefab", typeof(GameObject))));
        DontDestroyOnLoad(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/PickupCanvas.prefab", typeof(GameObject))));
        Light currentLight = FindObjectOfType<Light>();
        if (!currentLight && forceSun)
            DontDestroyOnLoad(
                Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/ForceSun.prefab", typeof(GameObject))));
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _doorControllers = FindObjectsOfType<DoorController>().ToList();
        _scoreController = FindObjectOfType<ScoreController>();
        levelStartTime = Time.time; // todo move statement to when player moves out of startRoom
        _audioSource = AudioUtility.CreateSFX(onOutOfCombat, transform, 0, loop: true, volume: 0.05f);
    }

    void Update()
    {
        if (enemiesInCombat > 0)
        {
            if (!combatMusicPlaying)
            {
                _audioSource.clip = bossCombat ? onBossCombat : onCombat;
                _audioSource.Play();
                combatMusicPlaying = true;
            }
            if(debugMode) Debug.Log("in combat");
            if (_doorControllers?.Count > 0) _doorControllers.ForEach(door => door.doorLocked = true);
        }
        else if (_doorControllers?.Count > 0)
            _doorControllers.ForEach(door => door.doorLocked = false);
        if(Input.GetKeyDown(KeyCode.L)) _doorControllers = FindObjectsOfType<DoorController>().ToList();
        if (enemiesInCombat == 0)
        {
            if (combatMusicPlaying)
            {
                _audioSource.clip = onOutOfCombat;
                _audioSource.Play();
                combatMusicPlaying = false;
            }
            
        }
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
        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _camLookAtMe = FindObjectOfType<PlayerController>().transform.Find("CamLookAtMe").transform; 
        _cinemachineVirtualCamera.m_Follow = _camLookAtMe;
        _cinemachineVirtualCamera.m_LookAt = _camLookAtMe;
        
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