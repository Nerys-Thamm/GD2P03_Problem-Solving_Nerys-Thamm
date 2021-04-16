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
        if(Input.GetMouseButtonDown(2))
        {
            Vector2 mousedirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position);
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, mousedirection, mousedirection.magnitude);
            if(!(hit.collider != null && hit.collider.CompareTag("Wall")))
            {
                m_motor.m_direction += mousedirection.normalized;
            }
            else
            {
                m_motor.m_direction = Vector2.zero;
            }

        }
    }
}
