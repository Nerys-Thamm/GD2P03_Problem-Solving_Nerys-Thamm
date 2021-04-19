using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Motor))]
public class ManualControl : MonoBehaviour
{
    Collider2D m_collider;
    Motor m_motor;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_motor = GetComponent<Motor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(2))
        {
            Vector2 mousedirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, mousedirection, mousedirection.magnitude);
            
            bool pathblocked = false;
            foreach(RaycastHit2D hit in hits)
            {
                if(hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
                {
                    pathblocked = true;
                }
            }
            if(!pathblocked)
            {
                m_motor.m_direction = mousedirection.normalized;
                Debug.DrawRay(transform.position, mousedirection, Color.green);
            }
            else
            {
                m_motor.m_direction = Vector2.zero;
            }

        }
        else
        {
            m_motor.m_direction = Vector2.zero;
        }
    }
}
