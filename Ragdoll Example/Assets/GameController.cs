using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using Interactions;
using PlayerScripts;
using Sound;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played continuously while out of combat")]
    public AudioClip onOutOfCombat;
    [Tooltip("Sound played continuously while in normal combat")]
    public AudioClip onCombat;
    [Tooltip("Sound played continuously while in boss combat")]
    public AudioClip onBossCombat;
    [Tooltip("game win music")]
    public AudioClip onGameWon;
    [Tooltip("Sound played continuously while in normal combat")]
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
    private float _amplifyStep = 0.1f;
    private Image _imageHelp;

    // public List<Pickup> pickedUpPickups = new List<Pickup>();
    public Dictionary<string, int> levelToScore = new Dictionary<string, int>();
    public string playerID;
    

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
        
        Light currentLight = FindObjectOfType<Light>();
        if (!currentLight && forceSun)
            DontDestroyOnLoad(
                Instantiate(Resources.Load<GameObject>("Prefabs/ForceSun")));
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _imageHelp = transform.Find("MainHUD").Find("InfoAndHelp").GetComponent<Image>();
        _imageHelp.enabled = false;
        _doorControllers = FindObjectsOfType<DoorController>().ToList();
        _scoreController = FindObjectOfType<ScoreController>();

        if (_scoreController)
            DontDestroyOnLoad(_scoreController);

        if (FindObjectOfType<PickupDisplay>())
            DontDestroyOnLoad(FindObjectOfType<PickupDisplay>());

        levelStartTime = Time.time; // todo maybe move statement to when player moves out of startRoom
        _audioSource = AudioUtility.CreateSFX(onOutOfCombat, transform, 0, loop: true, volume: 0.04f);
        playerID = PlayerPrefs.GetString("PlayerID");
        if (playerID == "")
        {
            playerID = "" + DateTime.Now.Ticks;
            PlayerPrefs.SetString("PlayerID", playerID);
        }
        print("saved playerid: " + PlayerPrefs.GetString("PlayerID"));

        //Telemetry.SetGameVersion("A");
        //Telemetry.SetPlayerID(PlayerPrefs.GetString("PlayerID"));
    }

    void Update()
    {
        //Debug.Log("enemies in combat" + enemiesInCombat);
        if(Input.GetKeyDown(KeyCode.F4)) // button to reset level
            LoadScene("LevelSelection", true);
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
                var gameWon = PlayerPrefs.GetInt("bossLevel") == 1 && SceneManager.GetActiveScene().name == "LevelSelection";
                transform.Find("MainHUD").Find("WinText").GetComponent<TextMeshProUGUI>().color = gameWon? Color.green : Color.clear;
                if (_audioSource.clip.name != onGameWon.name && gameWon)
                {
                    _audioSource.clip = onGameWon;
                    _audioSource.Play();
                    combatMusicPlaying = false;
                }
                if (!gameWon && _audioSource.clip.name != onOutOfCombat.name)
                {
                    _audioSource.clip = onOutOfCombat;
                    _audioSource.Play();
                    combatMusicPlaying = false;
                }
        }

        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.F6))
        {
            _amplifyStep = AudioUtility.masterAudioAmplify < 1 ? 0.1f : 0.1f;
            AudioUtility.masterAudioAmplify += _amplifyStep;
            AudioUtility.masterAudioAmplify = (float)Math.Round(AudioUtility.masterAudioAmplify, 2);
            if (AudioUtility.masterAudioAmplify >= 40.1f) AudioUtility.masterAudioAmplify = 40f;

            _audioSource.volume = 0.02f * AudioUtility.masterAudioAmplify;
        }

        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.F5))
        {
            _amplifyStep = AudioUtility.masterAudioAmplify < 1 ? 0.1f : 0.1f; 
            AudioUtility.masterAudioAmplify -= _amplifyStep;
            AudioUtility.masterAudioAmplify = (float)Math.Round(AudioUtility.masterAudioAmplify, 2);
            if (AudioUtility.masterAudioAmplify <= -0.01f) AudioUtility.masterAudioAmplify = 0.0f;
            _audioSource.volume = 0.02f * AudioUtility.masterAudioAmplify;
        }

        Time.timeScale = _imageHelp.enabled ? 0 : 1;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            _imageHelp.enabled = !_imageHelp.enabled;
        }
    }

    public void LoadScene(string sceneName, bool isNewLevel)
    {
        if (isNewLevel)
        {
            levelToScore.TryGetValue(SceneManager.GetActiveScene().name, out var highScore);
            if (_scoreController && highScore < _scoreController.playerScore) // is new high score?
                levelToScore[SceneManager.GetActiveScene().name] = _scoreController.playerScore;
        }

        if (_scoreController)
            _scoreController.ResetScore();

        DestroyPickups(isNewLevel);
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

            if (_scoreController)
                _scoreController.EnemyKilled();
            
        }
        Debug.Log("enemy killed");
    }

    public int getEnemiesInCombat()
    {
        return enemiesInCombat;
    }
    
    private void DestroyPickups(bool onlyActive = true)
    {
        List<Pickup> toRemove = new List<Pickup>();
        var pickupDisplay = FindObjectOfType<PickupDisplay>();
        if (FindObjectOfType<PickupDisplay>())
        {
            FindObjectOfType<PickupDisplay>().pickups.ForEach(pickup =>
            {
                if (pickup.timeOfActivation > 0 || !onlyActive)
                {
                    toRemove.Add(pickup); // cant alter list while iterating thought it
                    Destroy(pickup.buttonController.gameObject);
                    Destroy(pickup.gameObject);
                }
            });

            toRemove.ForEach(pickup => pickupDisplay.RemovePickup(pickup));
        }
    }
}