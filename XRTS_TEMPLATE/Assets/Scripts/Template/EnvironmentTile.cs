using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTile : MonoBehaviour
{
    public SpriteRenderer m_LowerRenderer;
    public SpriteRenderer m_WallRenderer;
    public SpriteRenderer m_UpperRenderer;

    public Sprite[] m_LowerSprites;
    public Sprite[] m_WallSprites;
    public Sprite[] m_UpperSprites;

    public LayerMask m_EnvironmentLayerMask;

    public void CalculateTileSprite(float _tileSize)
    {
        bool s = Physics2D.OverlapPoint(transform.position, m_EnvironmentLayerMask);
        
        bool u = Physics2D.OverlapPoint(transform.position + new Vector3(0.0f, 1.0f, 0.0f) * _tileSize, m_EnvironmentLayerMask);
        bool d = Physics2D.OverlapPoint(transform.position + new Vector3(0.0f, -1.0f, 0.0f) * _tileSize, m_EnvironmentLayerMask);
        bool l = Physics2D.OverlapPoint(transform.position + new Vector3(-1.0f, 0.0f, 0.0f) * _tileSize, m_EnvironmentLayerMask);
        bool r = Physics2D.OverlapPoint(transform.position + new Vector3(1.0f, 0.0f, 0.0f) * _tileSize, m_EnvironmentLayerMask);

        int c = 0;
        c += u ? 1 : 0;
        c += d ? 1 : 0;
        c += l ? 1 : 0;
        c += r ? 1 : 0;

        int spriteIndex = -1;
        float angle = 0.0f;
        bool flipX = false;
        bool flipY = false;
        switch (c)
        {
            case 0:
            {
                spriteIndex = 0;
            } break;
            case 1:
            {
                spriteIndex = 1;
                angle = (r) ? 90.0f : angle; 
                angle = (u) ? 180.0f : angle; 
                angle = (l) ? 270.0f : angle; 
            } break;
            case 2:
            {
                if ((u && d) || (l && r))
                {
                    spriteIndex = 2;
                    angle = (l && r) ? 90.0f : angle;
                }
                else
                {
                    spriteIndex = 3;
                    angle = (r && u) ? 90.0f : angle; 
                    angle = (u && l) ? 180.0f : angle;
                    angle = (l && d) ? 270.0f : angle;
                }
            } break;
            case 3:
            {
                spriteIndex = 4;
                angle = (!d) ? 90.0f : angle; 
                angle = (!r) ? 180.0f : angle; 
                angle = (!u) ? 270.0f : angle; 
            } break;
            case 4:
            {
                spriteIndex = 5;
            } break;
            default: {} break;
        }

        if (s)
        {
            m_UpperRenderer.sprite = m_UpperSprites[spriteIndex];
            m_UpperRenderer.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
            m_UpperRenderer.flipX = flipX;
            m_UpperRenderer.flipY = flipY;

            m_WallRenderer.enabled = true;
            int wallIndex = 0;
            bool wallFlipX = false;
            if (l && r)
            {
                wallIndex = 2;
            }
            else if (r)
            {
                wallIndex = 1;
                wallFlipX = true;
            }
            else if (l)
            {
                wallIndex = 1;
            }
            m_WallRenderer.sprite = m_WallSprites[wallIndex];
            m_WallRenderer.flipX = wallFlipX;

            m_LowerRenderer.sprite = null;
            m_LowerRenderer.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            m_LowerRenderer.flipX = false;
            m_LowerRenderer.flipY = false;
        }
        else
        {
            m_UpperRenderer.sprite = null;
            m_UpperRenderer.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            m_UpperRenderer.flipX = false;
            m_UpperRenderer.flipY = false;

            m_WallRenderer.sprite = null;
            m_WallRenderer.flipX = false;

            m_LowerRenderer.sprite = m_LowerSprites[spriteIndex];
            m_LowerRenderer.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
            m_LowerRenderer.flipX = flipX;
            m_LowerRenderer.flipY = flipY;
        }  
    }
}
