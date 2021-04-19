using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        m_direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_velocity.sqrMagnitude > m_max_speed*m_max_speed || true)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }
        
        Vector3 newDirection = Vector3.RotateTowards(m_velocity, m_direction.normalized, Time.deltaTime*2, 1.0f);
        Vector3 previousPosition = transform.position;
        transform.position += ((Vector3)newDirection * m_speed_multiplier) * Time.deltaTime;
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
