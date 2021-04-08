using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public Vector2Int m_Extents = new Vector2Int(25, 25);
    public float m_TileSize = 1.0f;

    public GameObject m_TilePrefab;
    private EnvironmentTile[] m_Tiles;

    private void OnDrawGizmos()
    {
        Vector2[] points = new Vector2[4]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, -0.5f)
        };

        for (int i = 0; i < 4; i++)
        {
            points[i].x = transform.position.x + points[i].x * m_Extents.x;
            points[i].y = transform.position.y + points[i].y * m_Extents.y;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }

    private void Awake()
    {
        RegenerateTiles();
        CalculateSprites();
    }

    private void ClearTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void RegenerateTiles()
    {
        float originX = transform.position.x - (float)m_Extents.x * 0.5f + m_TileSize * 0.5f;
        float originY = transform.position.y - (float)m_Extents.y * 0.5f + m_TileSize * 0.5f; 
        
        m_Tiles = new EnvironmentTile[m_Extents.x * m_Extents.y];
        for (int x = 0; x < m_Extents.x; x++)
        {
            for (int y = 0; y < m_Extents.y; y++)
            {
                Vector3 pos = new Vector3(originX + (float)x * m_TileSize, originY + (float)y * m_TileSize, transform.position.z);
                
                GameObject tile = Instantiate(m_TilePrefab, transform);
                tile.name = m_TilePrefab.name + " [" + x.ToString() + ", " + y.ToString() + "]";
                tile.transform.localPosition = pos;

                m_Tiles[x * m_Extents.y + y] = tile.GetComponent<EnvironmentTile>();      
            }
        }      
    }

    public void CalculateSprites()
    {
        for (int i = 0; i < m_Tiles.Length; i++)
        {
            m_Tiles[i].CalculateTileSprite(m_TileSize);
        }
    }
}
