using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteZAdjuster : MonoBehaviour
{
    private float m_OriginZ = 0.0f;

    private float m_MoveScale = 0.01f;

    private void Awake()
    {
        m_OriginZ = transform.position.z;
    }

    private void Update()
    {
        Vector3 cachePos = transform.position;
        cachePos.z = m_OriginZ + cachePos.y * m_MoveScale;
        transform.position = cachePos;
    }
}
