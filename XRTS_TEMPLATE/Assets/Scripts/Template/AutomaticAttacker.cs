using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticAttacker : MonoBehaviour
{
    public AnimatedCharacterSprite m_AttachedSprite;

    public int m_DamageDealt = 25;
    public float m_AttackRadius = 0.25f;
    public float m_AttackCooldown = 0.25f;
    private float m_Timer = 0.0f;
    [SerializeField]
    public UnityEvent OnAttack;

    public LayerMask m_AttackLayerMask;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 pointA = new Vector3(0.0f, m_AttackRadius, 0.0f);
        Vector3 pointB = pointA;

        int stepCount = 36;
        float angle = 360.0f / (float)stepCount;
        for (int i = 0; i <= stepCount; i++)
        {
            pointA = Quaternion.Euler(0.0f, 0.0f, angle) * pointA;
            Gizmos.DrawLine(transform.position + pointA, transform.position + pointB);
            pointB = pointA;
        }
    }

    private void Update()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, m_AttackRadius, m_AttackLayerMask);
        if (hitCollider)
        {
            
            m_AttachedSprite.m_IsAttacking = true;
            m_AttachedSprite.m_AttackPosition = hitCollider.transform.position;
            
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0.0f)
            {
                Construct target = hitCollider.GetComponent<Construct>();
                OnAttack.Invoke();
                if (target)
                {
                    target.TakeDamage(transform.position, m_DamageDealt);
                    
                }
                m_Timer = m_AttackCooldown;
            }            
        }
        else
        {
            m_AttachedSprite.m_IsAttacking = false;

            m_Timer = m_AttackCooldown;
        }
    }
}
