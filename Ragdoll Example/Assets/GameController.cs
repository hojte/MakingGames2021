﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int enemiesInCombat = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
            enemiesInCombat--;
        Debug.Log("enemy killed");
    }

    public int getEnemiesInCombat()
    {
        return enemiesInCombat;
    }
}