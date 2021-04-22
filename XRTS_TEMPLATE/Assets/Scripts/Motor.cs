// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2021 Media Design School
//
// File Name   : Motor.cs
// Description : Character Motor for enemies
// Author      : Nerys Thamm
// Mail        : nerys.thamm@mds.ac.nz

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Motor for enemies.
/// </summary>

public class Motor : MonoBehaviour
{
    //Declare direction and velocity members
    Vector2 m_velocity;
    Vector2 m_direction;

    //Declare Inspector-Tweakable values
    [Range(0.0f, 10.0f)]
    public float m_speed_multiplier;

    [Range(0.0001f, 20.0f)]
    public float m_max_speed;

    [Range(0.0f, 1.0f)]
    public float m_collision_radius;

    public AnimatedCharacterSpriteHelper m_animhelper;

    /// <summary>
    /// Sets the direction.
    /// </summary>
    /// <param name="_dir">The direction.</param>
    public void SetDirection(Vector2 _dir) => m_direction = _dir;
    /// <summary>
    /// Gets the direction.
    /// </summary>
    /// <returns>A Vector2.</returns>
    public Vector2 GetDirection() => m_direction;

    /// <summary>
    /// Gets the velocity.
    /// </summary>
    /// <returns>A Vector2.</returns>
    public Vector2 GetVelocity() => m_velocity;

    // ********************************************************************************
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    // ********************************************************************************
    private void Start()
    {
        //Randomises starting direction
        m_direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    // ********************************************************************************
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    // ********************************************************************************
    private void Update()
    {
        //Get the new direction using velocity and the inputted direction
        Vector3 newDirection = Vector3.RotateTowards(m_velocity, m_direction.normalized, Time.deltaTime * 2, 1.0f);

        //Get the new velocity from the new direction and speed
        m_velocity = ((Vector3)newDirection * m_speed_multiplier) * Time.deltaTime;

        //Clamp speed below max speed
        if (m_velocity.sqrMagnitude > m_max_speed * m_max_speed)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }

        //Check that the enemy is not in the attacking state
        if (!m_animhelper.m_AttachedSprite.m_IsAttacking)
        {
            //Move the enemy
            transform.position += (Vector3)m_velocity;
        }
        //Set the animation based on the movement of the enemy
        m_animhelper.m_Velocity = newDirection * m_speed_multiplier;
    }
}