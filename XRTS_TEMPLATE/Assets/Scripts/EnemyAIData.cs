// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2021 Media Design School
//
// File Name   : EnemyAIData.cs
// Description : ScriptableObject holding Data for enemy AI behaviour.
// Author      : Nerys Thamm
// Mail        : nerys.thamm@mds.ac.nz
using UnityEngine;


/// <summary>
/// ScriptableObject holding Data for enemy AI behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Data/EnemyAIData", fileName = "New Enemy AI Data")]
public class EnemyAIData : ScriptableObject
{
    [Range(0.0f, 10.0f)]
    public float m_neighbor_detect_radius;

    [Range(0.0f, 1.0f)]
    public float m_neighbor_seek_multiplier;

    [Range(0.0f, 2.0f)]
    public float m_neighbor_avoid_radius;

    [Range(0.0f, 1.0f)]
    public float m_neighbor_avoid_multiplier;

    [Range(0.0f, 1.0f)]
    public float m_wall_avoid_multiplier;

    [Range(0.0f, 1.0f)]
    public float m_neighbor_align_multiplier;

    [Range(0.0f, 1.0f)]
    public float m_goal_seek_multiplier;

    [Range(0.0f, 1.0f)]
    public float m_flow_field_multiplier;

}