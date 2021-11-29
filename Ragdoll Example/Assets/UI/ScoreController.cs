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
            m_AccumulatedDeltaTime += Time.deltaTime;
            m_AccumulatedFrameCount++;

            if (m_AccumulatedDeltaTime >= 0.5f) // time to update Score board
            {
                int framerate = Mathf.RoundToInt((float)m_AccumulatedFrameCount / m_AccumulatedDeltaTime);
                uiText.text = "Score: "+playerScore+ "("+shopBalance+")"+
                              "\nFramerate: "+ framerate;

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
        public void LevelCompleted()
        {
            playerScore += 8;
            shopBalance += 8;
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