using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactions
{
    public class DoorController : MonoBehaviour
    {
        public bool doorLocked;
        public bool doorClosed = true;
        public bool isLockedOnCombat = true;
        private GameController _gameController;
        
        public bool isLookedAt;
        private Outline _outline;
        private string levelName;
        public string linkedLevelName;

        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            SetClosed(doorClosed);
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineAll;
            _outline.OutlineColor = Color.blue;
            _outline.enabled = false;
            _outline.OutlineWidth = 15f;

            levelName = SceneManager.GetActiveScene().name;
        }

        public void SetClosed(bool close)
        {
            if (doorLocked) return;
            doorClosed = close;
            if (!close && GetClosed())
                transform.parent.rotation = Quaternion.Euler(0, -90, 0);
            if (close && !GetClosed())
                transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }

        public bool GetClosed()
        {
            return transform.parent.rotation == Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {
            if (_outline == null)
            {
                _outline = gameObject.GetComponent<Outline>();
            }

            if (levelName != "LevelSelection")
            {
                if (_gameController == null) _gameController = FindObjectOfType<GameController>();
                if (isLockedOnCombat && _gameController.getEnemiesInCombat() > 0) doorLocked = true;
                else
                {
                    if (levelName == "bossLevel")
                    {
                        doorLocked = false;
                        transform.parent.position = new Vector3(900, 900, 900);
                    }
                    else
                        doorLocked = false;
                }
                SetClosed(doorClosed);
                if (doorLocked) GetComponent<Renderer>().material.color = Color.red;
                else GetComponent<Renderer>().material.color = Color.green;
            }
            else
                levelSelectorDoor();
        }

        private void OnMouseOver()
        {
            if (levelName != "bossLevel")
            {
                if (Vector3.Distance(Camera.main.transform.position, transform.position) < 30 && !doorLocked)
                {
                    isLookedAt = true;
                    _outline.enabled = true;
                }
                else
                {
                    isLookedAt = false;
                    _outline.enabled = false;
                }
            }
        }
        private void OnMouseExit()
        {
            if (levelName != "bossLevel")
            {
                isLookedAt = false;
                _outline.enabled = false;
            }
        }

        void levelSelectorDoor()
        {
            doorLocked = true;

            if (PlayerPrefs.GetInt(linkedLevelName) == 1 || linkedLevelName == "zero")
            { // Linked level has been completed by the player
                transform.parent.rotation = transform.parent.parent.rotation * Quaternion.Euler(0, -90, 0);
            }
            else
                transform.parent.rotation = transform.parent.parent.rotation;
        }
    }

    
}
