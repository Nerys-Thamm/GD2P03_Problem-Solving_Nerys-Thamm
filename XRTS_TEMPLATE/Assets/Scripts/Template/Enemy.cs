using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        // Hooks us up to the enemy manager by default so that we can find it later without searching the scene
        FindObjectOfType<EnemyManager>().m_Enemies.Add(this);
        GetComponent<Swarm>().enabled = FindObjectOfType<EnemyManager>().m_isAI;
        GetComponent<ManualControl>().enabled = !FindObjectOfType<EnemyManager>().m_isAI;
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
