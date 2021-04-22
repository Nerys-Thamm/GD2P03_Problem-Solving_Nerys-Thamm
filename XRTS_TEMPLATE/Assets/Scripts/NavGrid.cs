using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public bool makeflowfield = false;
    public class Cell
    {
        public Cell(Vector3 _position, Vector2 _index)
        {
            m_distance = 255;
            m_traversable = true;
            m_direction = Vector2.zero;
            m_position = _position;
            m_index = _index;
        }
        public void SetDistance(float _distance) => m_distance = _distance;
        public void SetTraversable(bool _traversable) => m_traversable = _traversable;


        public float m_distance;
        public bool m_traversable;
        public Vector2 m_index;
        public Vector2 m_direction;
        public Vector3 m_position;
    }

    
    public Cell[,] m_grid;

    public Vector3 m_origin = Vector3.zero;
    public int m_width = 20;
    public int m_height = 20;
    public float m_cellradius = 0.5f;

    public void Create()
    {
        m_grid = new Cell[m_width, m_height];

        for(int i = 0; i < m_width; i++)
        {
            for(int j = 0; j < m_height; j++)
            {
                Vector3 pos = new Vector3(((m_cellradius * 2) * i + m_cellradius)+m_origin.x, ((m_cellradius * 2) * j + m_cellradius)+m_origin.y, 0);
                m_grid[i, j] = new Cell(pos, new Vector2(i, j));
            }
        }
    }

    public void GenerateFlowfield()
    {
        Queue<Cell> cells_to_process = new Queue<Cell>();
        int layermask = LayerMask.GetMask("Target", "Environment");
        foreach(Cell c in m_grid)
        {
            c.m_distance = 255;
            c.m_direction = Vector2.zero;
            c.m_traversable = true;

            Collider2D hit = Physics2D.OverlapBox(c.m_position, Vector3.one * m_cellradius * 2, 0, layermask);
            if(hit != null)
            {
                
                if (hit.CompareTag("Wall"))
                {
                    c.m_traversable = false;
                    c.m_distance = 255;
                }
                else if(hit.CompareTag("Construct"))
                {
                    c.m_distance = 0;
                    cells_to_process.Enqueue(c);
                }
                
            }
        }
        for (int i = 0; i < 20; i++)
        {
            foreach (Cell cell in m_grid)
            {
                Vector2[] offsets = { new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-1, 0), };
                Vector2 newdir = Vector2.zero;
                foreach (Vector2 offset in offsets)
                {
                    Vector2 neighborpos = cell.m_index + offset;
                    if (!((neighborpos.x < 0 || neighborpos.x >= m_width) || (neighborpos.y < 0 || neighborpos.y >= m_height)))
                    {

                        if (m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance > cell.m_distance + 1 && m_grid[(int)neighborpos.x, (int)neighborpos.y].m_traversable)
                        {

                            m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance = cell.m_distance + 1;
                            newdir += offset * -1;
                        }
                        else if (m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance < cell.m_distance)
                        {
                            newdir += offset;
                        }

                    }

                }
                cell.m_direction = newdir;

            }
        }

    }

    public void ResetFlowField()
    {
        foreach(Cell c in m_grid)
        {
            c.m_distance = 255;
            c.m_direction = Vector2.zero;
            c.m_traversable = true;
        }
    }
    public void UpdateEnemyVectors()
    {
        int layermask = LayerMask.GetMask("Enemy");
        foreach (Cell cell in m_grid)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(cell.m_position, Vector3.one * m_cellradius * 2, 0, layermask);
            foreach (Collider2D hit in hits)
            {
                if (hit != null)
                {

                    hit.gameObject.GetComponent<Swarm>().m_flowfieldvector = cell.m_direction.normalized;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        GridGizmo();
    }
    private void GridGizmo()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                Vector3 pos = new Vector3(((m_cellradius * 2) * i + m_cellradius)+m_origin.x, ((m_cellradius * 2) * j + m_cellradius)+m_origin.y, 0);
                Gizmos.DrawWireCube(pos, Vector3.one * m_cellradius * 2);
                if (m_grid != null)
                {
                    Gizmos.DrawRay(pos, m_grid[i, j].m_direction.normalized);
                    
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Create();
        GenerateFlowfield();
    }

    // Update is called once per frame
    void Update()
    {
        
       
       UpdateEnemyVectors();
    }
}


