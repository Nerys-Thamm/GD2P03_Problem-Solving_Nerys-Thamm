using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public Vector2 m_velocity;
    public Vector2 m_direction;
    public float m_max_speed;
    public AnimatedCharacterSpriteHelper m_animhelper;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_velocity.sqrMagnitude > m_max_speed*m_max_speed || true)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }
        
        Vector3 newDirection = Vector3.RotateTowards(m_velocity, m_direction.normalized, Time.deltaTime*2, 1.0f);
        transform.position += (Vector3)newDirection * Time.deltaTime;
        m_animhelper.m_Velocity = newDirection;
    }
}
