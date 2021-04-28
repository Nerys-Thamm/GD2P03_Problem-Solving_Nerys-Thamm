// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2021 Media Design School
//
// File Name   : EnemyManager
// Description : Manager for Enemies.
// Author      : Nerys Thamm (Partial, specific authorship specified)
// Mail        : nerys.thamm@mds.ac.nz

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy manager.
/// </summary>

public class EnemyManager : MonoBehaviour
{
    public ConstructManager m_ConstructManager;
    public List<Enemy> m_Enemies;

    //--Nerys--
    public bool m_toggleAI;
    public bool m_isAI = true;
    //---------

    /// <summary>
    /// Gets the target.
    /// </summary>
    /// <returns>A Transform.</returns>
    public Transform GetTarget()
    {
        return m_ConstructManager.GetConstruct();
    }

    int framecount = 0;
    /// <summary>
    /// Update is called every frame.
    /// </summary>
    private void Update()
    {
        int distributionmod = 1 + (m_Enemies.Count / 20);
        if (framecount > distributionmod) framecount = 0;

        // Constantly checks and removes any null enemies (ones destroyed or cleared somehow)
        for (int i = m_Enemies.Count - 1; i >= 0; i--)
        {
            if (!m_Enemies[i])
            {
                m_Enemies.RemoveAt(i);
            }
            else
            {
                if(i % distributionmod == framecount)
                {
                    m_Enemies[i].GetComponent<Swarm>().UpdateAI();
                }
            }
        }
        //--Nerys--
        //Toggle the AI for enemies on keypress or bool enabled
        if (m_toggleAI || Input.GetKeyDown(KeyCode.E))
        {
            m_toggleAI = false;
            ToggleAI();
        }
        //---------

        framecount++;
    }

    //--Nerys--
    /// <summary>
    /// Toggles the ai.
    /// </summary>
    private void ToggleAI()
    {
        m_isAI = !m_isAI;
        foreach (Enemy e in m_Enemies)
        {
            e.GetComponent<Swarm>().enabled = m_isAI;
            e.GetComponent<ManualControl>().enabled = !m_isAI;
        }
    }
    //---------
}