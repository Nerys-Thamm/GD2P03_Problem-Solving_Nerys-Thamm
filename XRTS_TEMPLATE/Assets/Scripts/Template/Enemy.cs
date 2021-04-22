using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The enemy.
/// </summary>

public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        // Hooks us up to the enemy manager by default so that we can find it later without searching the scene
        FindObjectOfType<EnemyManager>().m_Enemies.Add(this);
        // Set AI status to that of the manager
        GetComponent<Swarm>().enabled = FindObjectOfType<EnemyManager>().m_isAI;
        GetComponent<ManualControl>().enabled = !FindObjectOfType<EnemyManager>().m_isAI;
    }
    // ********************************************************************************
    /// <summary>
    /// Destroys the gameobject the script is attached to
    /// </summary>
    // ********************************************************************************
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
