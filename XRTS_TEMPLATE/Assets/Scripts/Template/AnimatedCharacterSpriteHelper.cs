using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCharacterSpriteHelper : MonoBehaviour
{
    public Vector2 m_Velocity;
    public AnimatedCharacterSprite m_AttachedSprite;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(m_Velocity.x, m_Velocity.y, 0.0f));
    }

    private void Update()
    {
        m_AttachedSprite.UpdateSpriteVelocity(m_Velocity, Time.deltaTime);
    }
}
