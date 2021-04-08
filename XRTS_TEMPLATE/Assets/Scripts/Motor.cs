using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public Vector2 m_velocity;
    public float m_max_speed;
    public AnimatedCharacterSpriteHelper m_animhelper;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_velocity.sqrMagnitude > m_max_speed*m_max_speed)
        {
            m_velocity = m_velocity.normalized * m_max_speed;
        }
        transform.position += (Vector3)m_velocity * Time.deltaTime;
        m_animhelper.m_Velocity = m_velocity;
    }
}
