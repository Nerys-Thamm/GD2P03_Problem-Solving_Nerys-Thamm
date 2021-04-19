using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    

    public Transform m_goal;

    public Vector2 m_velocity;

    Collider2D m_collider;
    Motor m_motor;
    public Collider2D GetCollider() => m_collider;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_motor = GetComponent<Motor>();
    }

    // Update is called once per frame
    void Update()
    {
        m_velocity = m_motor.m_velocity;
        Vector2 cohesion = ((GetMeanPosition(GetAINeighbors()) - (Vector2)transform.position)).normalized * m_neighbor_seek_multiplier;
        Vector2 alignment = (GetMeanVelocity(GetAINeighbors()).normalized) * m_neighbor_align_multiplier;
        Vector2 avoidance = GetAvoidanceVelocity(GetAINeighbors()).normalized * m_neighbor_avoid_multiplier;
        Vector2 wallavoidance = GetAvoidanceVelocity(GetWalls()).normalized * m_wall_avoid_multiplier;
        Vector2 seeking;
        if(m_goal != null)
        {
            seeking = ((Vector2)m_goal.position - (Vector2)transform.position).normalized * m_goal_seek_multiplier;
        }
        else
        {
            seeking = Vector2.zero;
        }
        Debug.DrawRay(transform.position, cohesion, Color.blue);
        Debug.DrawRay(transform.position, alignment, Color.white);
        Debug.DrawRay(transform.position, avoidance.normalized, Color.red);
        Debug.DrawRay(transform.position, wallavoidance.normalized, Color.red);
        Debug.DrawRay(transform.position, seeking, Color.green);
        m_motor.m_direction += cohesion + alignment + avoidance + seeking + wallavoidance;

        
    }

    List<Transform> GetAINeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius);
        foreach(Collider2D c in neighborColliders)
        {
            if(c != m_collider && c.gameObject.CompareTag("Enemy"))
            {

                neighbors.Add(c.transform);
            }
            else if(c.gameObject.CompareTag("Construct"))
            {
                m_goal = c.transform;
            }
        }
        return neighbors;
    }

    List<Transform> GetNeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius);
        foreach (Collider2D c in neighborColliders)
        {
            if (c != m_collider && !c.gameObject.CompareTag("Construct"))
            {

                neighbors.Add(c.transform);
            }
        }
        return neighbors;
    }

    List<Transform> GetWalls()
    {
        List<Transform> walls = new List<Transform>();
        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(transform.position, m_neighbor_detect_radius);
        foreach (Collider2D c in wallColliders)
        {
            if (c.gameObject.CompareTag("Wall"))
            {

                walls.Add(c.transform);
            }
        }
        return walls;
    }

    Vector2 GetMeanPosition(List<Transform> transforms)
    {
        if(transforms.Count == 0)
        {
            return Vector2.zero;
        }
        Vector2 mean = Vector2.zero;

        foreach(Transform t in transforms)
        {
            
             mean += (Vector2)t.position;
            
        }
        return (mean / transforms.Count);
    }

    Vector2 GetMeanVelocity(List<Transform> transforms)
    {
        if (transforms.Count == 0)
        {
            return Vector2.zero;
        }
        Vector2 mean = Vector2.zero;

        foreach (Transform t in transforms)
        {


            mean += (Vector2)t.gameObject.GetComponent<Motor>().m_direction;
            
            
        }
        return (mean / transforms.Count);
    }

    Vector2 GetAvoidanceVelocity(List<Transform> transforms)
    {
        if (transforms.Count == 0)
        {
            return Vector2.zero;
        }
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
