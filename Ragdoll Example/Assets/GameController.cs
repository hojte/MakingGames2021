using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using Interactions;
using PlayerScripts;
using Sound;
using UI;
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

    private ScoreController _scoreController;
    private PickupDisplay _pickupDisplay;
    private List<DoorController> _doorControllers;
    public CinemachineVirtualCamera _cinemachineVirtualCamera;
    public Transform _camLookAtMe;
    private bool combatMusicPlaying;
    public bool bossCombat;
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
        
        // DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/Crosshair")));
        // DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/ScoreUtil")));
        // DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/PickupCanvas")));
        Light currentLight = FindObjectOfType<Light>();
        if (!currentLight && forceSun)
            DontDestroyOnLoad(
                Instantiate(Resources.Load<GameObject>("Prefabs/ForceSun")));
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _doorControllers = FindObjectsOfType<DoorController>().ToList();
        _scoreController = FindObjectOfType<ScoreController>();
        DontDestroyOnLoad(_scoreController);
        DontDestroyOnLoad(FindObjectOfType<PickupDisplay>());
        levelStartTime = Time.time; // todo maybe move statement to when player moves out of startRoom
        _audioSource = AudioUtility.CreateSFX(onOutOfCombat, transform, 0, loop: true, volume: 0.03f);
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
        if (enemiesInCombat == 0)
        {
            if (combatMusicPlaying)
            {
                _audioSource.clip = onOutOfCombat;
                _audioSource.Play();
                combatMusicPlaying = false;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) AudioUtility.masterAudioAmplify += 0.05f;
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) AudioUtility.masterAudioAmplify -= 0.05f;
    }

    public void LoadScene(string sceneName)
    {
        // FindObjectOfType<Compass>().ResetList(_cinemachineVirtualCamera);

        DestroyActivePickups();
        ((Func<Task>)(async () =>{
            var loadScene = SceneManager.LoadSceneAsync(sceneName);
            while (!loadScene.isDone)
            {
                await Task.Delay(10);
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
    
    private void DestroyActivePickups()
    {
        List<Pickup> toRemove = new List<Pickup>();
        var pickupDisplay = FindObjectOfType<PickupDisplay>();
        FindObjectOfType<PickupDisplay>().pickups.ForEach(pickup =>
        {
            if (pickup.timeOfActivation > 0) {
                toRemove.Add(pickup); // cant alter list while iterating thought it
                Destroy(pickup.buttonController.gameObject);
                Destroy(pickup.gameObject);
            }
        });
        toRemove.ForEach(pickup=>pickupDisplay.RemovePickup(pickup)); 
    }
}