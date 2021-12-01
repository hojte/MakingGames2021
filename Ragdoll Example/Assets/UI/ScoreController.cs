using Sound;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreController : MonoBehaviour
    {
        public int playerScore;
        public int shopBalance;
        private TextMeshProUGUI uiText;
        float m_AccumulatedDeltaTime = 0f;
        int m_AccumulatedFrameCount = 0;

        private void Start()
        {
            uiText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (playerScore < 0) playerScore = 0; // never negative score
            if (shopBalance < 0) shopBalance = 0; // never negative bal
            m_AccumulatedDeltaTime += Time.deltaTime;
            m_AccumulatedFrameCount++;

            if (m_AccumulatedDeltaTime >= 0.5f) // time to update Score board
            {
                int framerate = Mathf.RoundToInt((float)m_AccumulatedFrameCount / m_AccumulatedDeltaTime);
                uiText.text = "Score: " + playerScore + "(" + shopBalance + ")" +
                              "\nFramerate: " + framerate +
                              "\n-Volume " + AudioUtility.masterAudioAmplify + "+";

                m_AccumulatedDeltaTime = 0f;
                m_AccumulatedFrameCount = 0;
            }
        }

        public void EnemyKilled()
        {
            playerScore += 3;
            shopBalance += 3;
        }
        public void PlayerDied()
        {
            playerScore -= 2;
            shopBalance -= 2;
        }
        public void Pickup(bool increment)
        {
            playerScore += increment?4:-3;
            shopBalance += increment?4:-3;
        }
        public void LevelCompleted(int expectedTimeForCompletion)
        {
            var levelStartTime = FindObjectOfType<GameController>().levelStartTime;
            var completionTime = Time.time - levelStartTime;
            var ratio = expectedTimeForCompletion / completionTime;
            
            int scoreToAdd = (int)(8 * ratio);
            print("level completed in "+completionTime+ "secs! Expected time is "+expectedTimeForCompletion+"secs which gives a ratio of "+ratio+" final score to add is "+scoreToAdd);
            playerScore += scoreToAdd;
            shopBalance += scoreToAdd;
        }

        public bool BuyAmount(int amount)
        {
            if (amount > shopBalance) return false;

            shopBalance-=amount;
            return true;
        }

        public void PlayerStunned()
        {
            playerScore -= 2;
            shopBalance -= 2;
        }
    }
}