using UnityEngine;

public class AnimatedCharacterSprite : MonoBehaviour
{
    [Header("Core Components")]
    public SpriteRenderer m_Renderer;

    public CharacterSpriteData m_SpriteData;

    [Header("Running")]
    public int m_CurrentRunAnimationIndex = 0;

    public int m_MaxRunAnimationIndex = 2;

    private float m_DistanceTravelled = 0.0f;
    public float m_AnimationStepDistance = 0.15f;

    private Vector2 m_CacheVelocity = new Vector2(0.0f, -1.0f);
    public float m_HaltVelocityMagnitude = 0.1f;

    [Header("Attacking")]
    public bool m_IsAttacking;

    public Vector3 m_AttackPosition;

    public int m_CurrentAttackAnimationIndex = 0;
    public int m_MaxAttackAnimationIndex = 2;

    public float m_AttackFrameTime = 0.2f;
    private float m_AttackTimer = 0.0f;

    public void UpdateSpriteVelocity(Vector2 _velocity, float _dT)
    {
        m_DistanceTravelled += _velocity.magnitude * _dT;
        while (m_DistanceTravelled > m_AnimationStepDistance)
        {
            m_DistanceTravelled -= m_AnimationStepDistance;
            m_CurrentRunAnimationIndex += 1;
        }
        m_CurrentRunAnimationIndex = m_CurrentRunAnimationIndex % m_MaxRunAnimationIndex;

        if (_velocity.magnitude < m_HaltVelocityMagnitude)
        {
            m_CurrentRunAnimationIndex = 0;
        }
        else
        {
            m_CacheVelocity = _velocity;
        }
    }

    private void UpdateSpriteState()
    {
        if (m_IsAttacking)
        {
            Vector2 attackDir = m_AttackPosition - transform.position;

            float angle = Mathf.Atan2(attackDir.x, -attackDir.y) * Mathf.Rad2Deg;
            int angleIndex = Mathf.RoundToInt(angle / 45.0f);
            angleIndex = (angleIndex + 8) % 8;

            m_AttackTimer -= Time.deltaTime;
            if (m_AttackTimer <= 0.0f)
            {
                m_CurrentAttackAnimationIndex++;
                m_AttackTimer += m_AttackFrameTime;
            }
            m_CurrentAttackAnimationIndex = m_CurrentAttackAnimationIndex % m_MaxAttackAnimationIndex;

            m_Renderer.sprite = m_SpriteData.m_AttackSpriteSheet[angleIndex * 2 + m_CurrentAttackAnimationIndex];
        }
        else
        {
            m_AttackTimer = m_AttackFrameTime;

            float angle = Mathf.Atan2(m_CacheVelocity.x, -m_CacheVelocity.y) * Mathf.Rad2Deg;
            int angleIndex = Mathf.RoundToInt(angle / 45.0f);
            angleIndex = (angleIndex + 8) % 8;

            m_Renderer.sprite = m_SpriteData.m_RunSpriteSheet[angleIndex * 2 + m_CurrentRunAnimationIndex];
        }
    }

    private void Update()
    {
        UpdateSpriteState();
    }
}