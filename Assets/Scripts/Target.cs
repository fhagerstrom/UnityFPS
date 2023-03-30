using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{

    private float health = 100.0f;
    // private float respawnTimer = 2.0f;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0) 
        {
            gameObject.SetActive(false);
        }
    }
}
