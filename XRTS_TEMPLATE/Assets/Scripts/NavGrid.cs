// Bachelor of Software Engineering
// Media Design School
// Auckland
// New Zealand
//
// (c) 2021 Media Design School
//
// File Name   : NavGrid.cs
// Description : Flowfield-esque pathfinding implementation.
// Author      : Nerys Thamm
// Mail        : nerys.thamm@mds.ac.nz

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The nav grid.
/// </summary>

public class NavGrid : MonoBehaviour
{
    public bool makeflowfield = false;

    /// <summary>
    /// The cell.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="_position">The _position.</param>
        /// <param name="_index">The _index.</param>
        public Cell(Vector3 _position, Vector2 _index)
        {
            m_distance = 255;
            m_traversable = true;
            m_direction = Vector2.zero;
            m_position = _position;
            m_index = _index;
        }

        /// <summary>
        /// Sets the distance.
        /// </summary>
        /// <param name="_distance">The _distance.</param>
        public void SetDistance(float _distance) => m_distance = _distance;

        /// <summary>
        /// Sets the traversable.
        /// </summary>
        /// <param name="_traversable">If true, _traversable.</param>
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

    /// <summary>
    /// Creates the Grid.
    /// </summary>
    public void Create()
    {
        m_grid = new Cell[m_width, m_height];
        //Populate the grid
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                Vector3 pos = new Vector3(((m_cellradius * 2) * i + m_cellradius) + m_origin.x, ((m_cellradius * 2) * j + m_cellradius) + m_origin.y, 0);
                m_grid[i, j] = new Cell(pos, new Vector2(i, j));
            }
        }
    }

    /// <summary>
    /// Generates the flowfield.
    /// </summary>
    public void GenerateFlowfield()
    {
        Queue<Cell> cells_to_process = new Queue<Cell>();
        int layermask = LayerMask.GetMask("Target", "Environment");
        foreach (Cell c in m_grid)
        {
            c.m_distance = 6500;
            c.m_direction = Vector2.zero;
            c.m_traversable = true;

            Collider2D hit = Physics2D.OverlapBox(c.m_position, Vector3.one/2, 0, layermask);
            if (hit != null)
            {
                if (hit.CompareTag("Wall"))
                {
                    c.m_traversable = false;
                    c.m_distance = 6500;
                }
                else if (hit.CompareTag("Construct"))
                {
                    c.m_distance = 0;
                    cells_to_process.Enqueue(c);
                }
            }
        }
        //Recursively populate direction vectors | TODO: Fix bug causing spaces less than 3x3 cells wide to not populate.
        while (cells_to_process.Count > 0)
        {
            Cell cell = cells_to_process.Dequeue();//Get cell from the queue
            //Offsets around the cell to check
            Vector2[] offsets = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(0, 0) };
            //Set best defaults
            float bestdistance = 6500;
            Vector2 bestoffset = Vector2.zero;
            //Iterate through each offset
            foreach (Vector2 offset in offsets)
            {
                Vector2 neighborpos = cell.m_index + offset; //Get index of the offset cell
                
                if (!((neighborpos.x < 0 || neighborpos.x >= m_width) || (neighborpos.y < 0 || neighborpos.y >= m_height))) //Check that the cell is in bounds
                {
                    //If the Neighboring cell has a larger distaance than this one, set the neighboring cells distance to be this plus one
                    if (m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance > cell.m_distance )
                    {
                        if(m_grid[(int)neighborpos.x, (int)neighborpos.y].m_traversable)
                        {
                            m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance = cell.m_distance + 1;
                        }
                    
                        
                        

                    }
                    
                    //if the neighbor distance is smaller than the current best, set it as the best
                    if (m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance < bestdistance)
                    {
                        bestdistance = m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance;
                        bestoffset = offset;
                    }
                   
                }
            }
            foreach (Vector2 offset in offsets)
            {
                Vector2 neighborpos = cell.m_index + offset; //Get index of the offset cell

                if (!((neighborpos.x < 0 || neighborpos.x >= m_width) || (neighborpos.y < 0 || neighborpos.y >= m_height))) //Check that the cell is in bounds
                {
                    
                    if (!m_grid[(int)neighborpos.x, (int)neighborpos.y].m_traversable)
                    {
                        continue;
                    }
                    //If the distance is larger, add it to the queue to be checked
                    if (m_grid[(int)neighborpos.x, (int)neighborpos.y].m_distance > cell.m_distance)
                    {
                        cells_to_process.Enqueue(m_grid[(int)neighborpos.x, (int)neighborpos.y]);
                    }
                }
            }
            //set this cell's direction to be towards the neighboring cell with the lowest distance
            cell.m_direction = bestoffset;
        }
    }

    /// <summary>
    /// Resets the flow field.
    /// </summary>
    public void ResetFlowField()
    {
        foreach (Cell c in m_grid)
        {
            c.m_distance = 255;
            c.m_direction = Vector2.zero;
            c.m_traversable = true;
        }
    }

    /// <summary>
    /// Updates the enemy vectors.
    /// </summary>
    public void UpdateEnemyVectors()
    {
        //Check each cell for enemies and update that enemy with the correct flow field vecotr data
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

    /// <summary>
    /// Gizmos for the Grid
    /// </summary>
    private void GridGizmo()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                Vector3 pos = new Vector3(((m_cellradius * 2) * i + m_cellradius) + m_origin.x, ((m_cellradius * 2) * j + m_cellradius) + m_origin.y, 0);
                
                Gizmos.DrawWireCube(pos, Vector3.one * m_cellradius * 2);
                if (m_grid != null)
                {
                    Gizmos.DrawRay(pos, m_grid[i, j].m_direction.normalized);
                }
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Create();
        GenerateFlowfield();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateEnemyVectors();
    }
}