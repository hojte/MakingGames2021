using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreController : MonoBehaviour
    {
        public int playerScore;
        public int scoreSpent;
        private TextMeshProUGUI uiText;
        float m_AccumulatedDeltaTime = 0f;
        int m_AccumulatedFrameCount = 0;

        private void Start()
        {
            uiText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            m_AccumulatedDeltaTime += Time.deltaTime;
            m_AccumulatedFrameCount++;

            if (m_AccumulatedDeltaTime >= 0.5f) // time to update Score board
            {
                int framerate = Mathf.RoundToInt((float)m_AccumulatedFrameCount / m_AccumulatedDeltaTime);
                uiText.text = "Score: "+playerScore+ "("+GetShopFunds()+")"+
                              "\nFramerate: "+ framerate;

                m_AccumulatedDeltaTime = 0f;
                m_AccumulatedFrameCount = 0;
            }
        }

        public void EnemyKilled()
        {
            playerScore += 5;
        }
        public void PlayerDied()
        {
            playerScore -= 2;
        }
        public void Pickup(bool increment)
        {
            playerScore += increment?5:-3;
        }
        public void LevelCompleted()
        {
            playerScore += 15;
        }

        public int GetShopFunds()
        {
            return playerScore - scoreSpent;
        }

        public bool BuyAmount(int amount)
        {
            if (amount > GetShopFunds()) return false;

            scoreSpent += amount;
            return true;
        }
    }
}