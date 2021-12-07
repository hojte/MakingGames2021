using TMPro;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    public string levelName;
    private GameController _gameController;
    void Start()
    {
        /*_gameController = FindObjectOfType<GameController>();
        GetComponentInChildren<NextRoomTrigger>().sceneToLoad = levelName;
        transform.Find("LevelText").GetComponentInChildren<TextMeshPro>().text = levelName;
        _gameController.levelToScore.TryGetValue(levelName, out var highScore); 
        transform.Find("HighScore").GetComponentInChildren<TextMeshPro>().text = "High score: " + highScore;*/
    }
}
