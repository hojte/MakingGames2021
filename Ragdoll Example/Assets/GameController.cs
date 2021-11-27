using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played continously in 2D")]
    public AudioClip mainTheme;
    int enemiesInCombat = 0;
    // Start is called before the first frame update
    void Start()
    {
        AudioUtility.CreateMainSFX(mainTheme);
        Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/UI/Crosshair.prefab", typeof(GameObject)));
    }

    // Update is called once per frame
    void Update()
    {
        /*if (enemiesInCombat > 0)
            Debug.Log("in combat");*/
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
