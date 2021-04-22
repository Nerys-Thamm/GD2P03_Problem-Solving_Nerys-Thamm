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
/// The swarm class for flocking-based AI pathfinding and behaviour.
/// </summary>

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Motor))]
public class Swarm : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float m_neighbor_detect_radius;
    [Range(0.0f, 10.0f)]
    public float m_neighbor_seek_multiplier;
    [Range(0.0f, 1.0f)]
    public float m_neighbor_avoid_radius;
    [Range(0.0f, 10.0f)]
    public float m_neighbor_avoid_multiplier;
    [Range(0.0f, 100.0f)]
    public float m_wall_avoid_multiplier;
    [Range(0.0f, 10.0f)]
    public float m_neighbor_align_multiplier;
    [Range(0.0f, 10.0f)]
    public float m_goal_seek_multiplier;
    

    public Transform m_goal; //"Targeted" Transform the enemy is trying to move towards

    public Vector2 m_velocity; //Speed and direction of the enemy

    Collider2D m_collider; //Collider of the enemy

    Motor m_motor; //Motor of the enemy

    /// <summary>
    /// Gets the collider.
    /// </summary>
    /// <returns>A Collider2D.</returns>
    public Collider2D GetCollider() => m_collider;
    // ********************************************************************************
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    // ********************************************************************************
    void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_motor = GetComponent<Motor>();
    }

    // ********************************************************************************
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    // ********************************************************************************
    void Update()
    {
        m_velocity = m_motor.m_velocity; //Set velocity to be equal to motor velocity

        //-------------------Getting Behaviour Vectors-----------------------------------
        Vector2 cohesion = ((GetMeanPosition(GetAINeighbors()) - (Vector2)transform.position)).normalized * m_neighbor_seek_multiplier; //Cohesion, try to move towards the average position of nearby agents
        Vector2 alignment = (GetMeanVelocity(GetAINeighbors()).normalized) * m_neighbor_align_multiplier; //Alignment, try to align direction with the average direction of nearby agents
        Vector2 avoidance = GetAvoidanceVelocity(GetAINeighbors()).normalized * m_neighbor_avoid_multiplier; //Avoidance, try to move away from agents within the avoidance radius
        Vector2 wallavoidance = GetAvoidanceVelocity(GetWalls()).normalized * m_wall_avoid_multiplier; //Wall avoidance, try to move away from environment obstacles
        Vector2 seeking; //Seeking, try to move towards the transform of the target

        if(m_goal != null)//If a goal is targeted find the vector for seeking behaviour
        {
            seeking = ((Vector2)m_goal.position - (Vector2)transform.position).normalized * m_goal_seek_multiplier;
        }
        else //Else disable seeking behaviour
        {
            seeking = Vector2.zero;
        }
        //-------------------------------------------------------------------------------

        //--------------Drawing Debugging lines for behaviour vectors--------------------
        Debug.DrawRay(transform.position, cohesion, Color.blue);
        Debug.DrawRay(transform.position, alignment, Color.white);
        Debug.DrawRay(transform.position, avoidance.normalized, Color.red);
        Debug.DrawRay(transform.position, wallavoidance.normalized, Color.red);
        Debug.DrawRay(transform.position, seeking, Color.green);
        //-------------------------------------------------------------------------------
        
        //Combine the behavior vectors and send the result to the motor
        m_motor.m_direction += cohesion + alignment + avoidance + seeking + wallavoidance;

        
    }


    /// <summary>
    /// Called when Gizmos are drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        //Draw a circle showing the detection radius of the agent
        Gizmos.DrawWireSphere(transform.position, m_neighbor_detect_radius);
    }


    /// <summary>
    /// Gets the ai neighbors.
    /// </summary>
    /// <returns>A list of Transforms.</returns>
    List<Transform> GetAINeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius); //Get colliders in range
        foreach(Collider2D c in neighborColliders)
        {
            if(c != m_collider && c.gameObject.CompareTag("Enemy")) //Check if the collider is an enemy
            {

                neighbors.Add(c.transform); //add it to the list
            }
            else if(c.gameObject.CompareTag("Construct")) //Check if the collider is a construct
            {
                m_goal = c.transform; //target it
            }
        }
        return neighbors; //return all AI agents found
    }


    /// <summary>
    /// Gets the neighbors.
    /// </summary>
    /// <returns>A list of Transforms.</returns>
    List<Transform> GetNeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius); //Get colliders in range
        foreach (Collider2D c in neighborColliders)
        {
            if (c != m_collider && !c.gameObject.CompareTag("Construct")) //Check that the collider is not a construct
            {

                neighbors.Add(c.transform); //add to the list
            }
        }
        return neighbors;
    }


    /// <summary>
    /// Gets the walls.
    /// </summary>
    /// <returns>A list of Transforms.</returns>
    List<Transform> GetWalls()
    {
        List<Transform> walls = new List<Transform>();
        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius); //get colliders in range
        foreach (Collider2D c in wallColliders)
        {
            if (c.gameObject.CompareTag("Wall")) //check if collider is a wall
            {

                walls.Add(c.transform); //Add to list
            }
        }
        return walls;
    }


    /// <summary>
    /// Gets the mean position.
    /// </summary>
    /// <param name="transforms">The transforms.</param>
    /// <returns>A Vector2.</returns>
    Vector2 GetMeanPosition(List<Transform> transforms)
    {
        if(transforms.Count == 0) //If no AI agents are nearby return Zeroed vector
        {
            return Vector2.zero;
        }
        //Find mean position vector
        Vector2 mean = Vector2.zero;
        foreach(Transform t in transforms)
        {
            
             mean += (Vector2)t.position;
            
        }
        return (mean / transforms.Count);
    }


    /// <summary>
    /// Gets the mean velocity.
    /// </summary>
    /// <param name="transforms">The transforms.</param>
    /// <returns>A Vector2.</returns>
    Vector2 GetMeanVelocity(List<Transform> transforms)
    {
        if (transforms.Count == 0)//If no AI agents are nearby return zeroed vector
        {
            return Vector2.zero;
        }
        //Find mean direction vector
        Vector2 mean = Vector2.zero;
        foreach (Transform t in transforms)
        {


            mean += (Vector2)t.gameObject.GetComponent<Motor>().m_direction;
            
            
        }
        return (mean / transforms.Count);
    }


    /// <summary>
    /// Gets the avoidance velocity.
    /// </summary>
    /// <param name="transforms">The transforms.</param>
    /// <returns>A Vector2.</returns>
    Vector2 GetAvoidanceVelocity(List<Transform> transforms)
    {
        if (transforms.Count == 0)//If no AI agents are in range return zeroed vector
        {
            return Vector2.zero;
        }
        //Find vector pointing away from the mean position of detected agents
        Vector2 mean = Vector2.zero;
        int num = 0;
        foreach (Transform t in transforms)
        {
            if (Vector2.SqrMagnitude(t.position - transform.position) < m_neighbor_avoid_radius)
            {
                num++;
                mean += (Vector2)(transform.position - t.position);
            }

        }
        if(num > 0)
        {
            mean /= num;
        }
        return mean;
    }

    

}
