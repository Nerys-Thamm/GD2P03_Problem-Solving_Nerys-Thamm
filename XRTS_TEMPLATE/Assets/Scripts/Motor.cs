// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
// 
// (c) 2021 Media Design School
//
// File Name   : 
// Description : 
// Author      : Nerys Thamm
// Mail        : nerys.thamm@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The motor.
/// </summary>

public class Motor : MonoBehaviour
{
    public Vector2 m_velocity;
    public Vector2 m_direction;
    [Range(0.0f, 10.0f)]
    public float m_speed_multiplier;
    [Range(0.0001f, 20.0f)]
    public float m_max_speed;
    [Range(0.0f, 1.0f)]
    public float m_collision_radius;
    public AnimatedCharacterSpriteHelper m_animhelper;
    // Start is called before the first frame update

    
    // ********************************************************************************
    /// <summary>
    /// 
    /// </summary>
    // ********************************************************************************
    void Start()
    {
        m_direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        
    }

    // ********************************************************************************
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    // ********************************************************************************
    void Update()
    {
        if(m_velocity.sqrMagnitude > m_max_speed*m_max_speed)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }
        
        Vector3 newDirection = Vector3.RotateTowards(m_velocity, m_direction.normalized, Time.deltaTime*2 , 1.0f);
        
        Vector3 previousPosition = transform.position;
        m_velocity = ((Vector3)newDirection * m_speed_multiplier) * Time.deltaTime;
        if (m_velocity.sqrMagnitude > m_max_speed * m_max_speed)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }
        if (!m_animhelper.m_AttachedSprite.m_IsAttacking)
        {
            transform.position += (Vector3)m_velocity;
        }
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_collision_radius);
        List<Collider2D> colliders = new List<Collider2D>();
        foreach(Collider2D c in collisions)
        {
            colliders.Add(c);
        }
        if(colliders.Count < 0)
        {
            transform.position = previousPosition;
        }
        m_animhelper.m_Velocity = newDirection * m_speed_multiplier;
    }
}
