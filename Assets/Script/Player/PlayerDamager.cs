using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
public int playerdamage; // 現在のダメージ値


void Update()
{
    if (Input.GetKeyDown(KeyCode.B)) 
    {
        playerdamage = playerdamage + 31;
    }

    if (Input.GetKeyUp(KeyCode.B))
    {
        playerdamage = playerdamage - 30;
    }

    
}
    
}
