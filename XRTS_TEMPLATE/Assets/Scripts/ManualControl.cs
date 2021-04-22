// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2021 Media Design School
//
// File Name   : ManualControl.cs
// Description : Script for controlling enemies with user input, via mouse.
// Author      : Nerys Thamm
// Mail        : nerys.thamm@mds.ac.nz

using UnityEngine;

/// <summary>
/// The manual control class for character motor.
/// </summary>

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Motor))]
public class ManualControl : MonoBehaviour
{
    private Collider2D m_collider;
    private Motor m_motor;

    // ********************************************************************************
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    // ********************************************************************************
    private void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_motor = GetComponent<Motor>();
    }

    // ********************************************************************************
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    // ********************************************************************************
    private void Update()
    {
        //If the middle mouse button is being pressed
        if (Input.GetMouseButton(2))
        {
            //Get the vector between the gameobject and the mouse's world location
            Vector2 mousedirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            //Do a raycast of the vector and collect all collisions
            RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, mousedirection, mousedirection.magnitude);

            //Check if the raycast collided with a wall
            bool pathblocked = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
                {
                    pathblocked = true;
                }
            }

            if (!pathblocked) //If the path is clear, move towards the mouse
            {
                m_motor.SetDirection(mousedirection.normalized);
                Debug.DrawRay(transform.position, mousedirection, Color.green);
            }
            else //Else do not move
            {
                m_motor.SetDirection(Vector2.zero);
            }
        }
        else //If the mouse button is not pressed, do not move
        {
            m_motor.SetDirection(Vector2.zero);
        }
    }
}