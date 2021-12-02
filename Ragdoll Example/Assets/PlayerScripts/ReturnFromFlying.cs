using System;
using UnityEngine;

public class ReturnFromFlying : MonoBehaviour
{
    private BetterMovement _betterMovement;
    private int _smoothCounter;
    
    void Start()
    {
        _betterMovement = FindObjectOfType<BetterMovement>();
    }

    private void Update()
    {
        if(_betterMovement == null) _betterMovement = FindObjectOfType<BetterMovement>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (_betterMovement.isFlying)
        {
            print("head collision: " + other.transform.name);
            if (!other.transform.GetComponent<CharacterJoint>()) // body parts
            {
                print("correct collision: "+other.transform.name);
                if (++_smoothCounter > 20)
                {
                    _betterMovement.returnFromStun();
                }
            } 

        }
        else _smoothCounter = 0;
    }
}
