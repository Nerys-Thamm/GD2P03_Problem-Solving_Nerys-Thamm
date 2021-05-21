using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseSpawner : MonoBehaviour
{
    public UnityEvent OnChange;

    [Header("Internal Components")]
    public Transform m_TileHighlight;

    public Transform m_MouseTarget;

    [Header("External Components")]
    public EnvironmentGenerator m_EnvironmentGenerator;

    [Header("Spawning")]
    public Vector2 m_Extents = new Vector2(18.0f, 10.0f);

    public float m_TileSize = 1.0f;
    public float m_DeleteSize = 0.95f;
    public float m_SelfOccludeSize = 0.05f;

    private Vector3 m_MouseCursorPosition;
    private Vector3 m_TilePosition;

    [System.Serializable]
    public struct SpawnOption
    {
        public bool m_Blocking;
        public LayerMask m_LayerMask;
        public GameObject m_Prefab;
        public float m_SpawnZ;
        public Sprite m_UISprite;

        public SpawnOption(bool _blocking, LayerMask _layerMask, GameObject _prefab, float _spawnZ, Sprite _uiSprite)
        {
            m_Blocking = _blocking;
            m_LayerMask = _layerMask;
            m_Prefab = _prefab;
            m_SpawnZ = _spawnZ;
            m_UISprite = _uiSprite;
        }
    }

    [Header("Spawn Options")]
    private float m_CurrentScrollWheel = 0.0f;

    private int m_CurrentSpawnIndex = 0;
    public SpawnOption[] m_SpawnOptions;

    [Header("UI")]
    public SpriteRenderer[] m_SpawnOptionDisplays;

    private void HandleScrollWheelInput()
    {
        m_CurrentScrollWheel += Input.mouseScrollDelta.y;
        if (m_CurrentScrollWheel <= -1.0f)
        {
            m_CurrentScrollWheel += 1.0f;
            m_CurrentSpawnIndex -= 1;
        }
        if (m_CurrentScrollWheel >= 1.0f)
        {
            m_CurrentScrollWheel -= 1.0f;
            m_CurrentSpawnIndex += 1;
        }
        m_CurrentSpawnIndex = (m_CurrentSpawnIndex + m_SpawnOptions.Length) % m_SpawnOptions.Length;
    }

    private void UpdateUISlots()
    {
        int prior = (m_CurrentSpawnIndex + m_SpawnOptions.Length - 1) % m_SpawnOptions.Length;
        int next = (m_CurrentSpawnIndex + m_SpawnOptions.Length + 1) % m_SpawnOptions.Length;

        m_SpawnOptionDisplays[0].sprite = m_SpawnOptions[prior].m_UISprite;
        m_SpawnOptionDisplays[1].sprite = m_SpawnOptions[m_CurrentSpawnIndex].m_UISprite;
        m_SpawnOptionDisplays[2].sprite = m_SpawnOptions[next].m_UISprite;
    }

    private void CalculateMousePositions()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0.0f;
        m_MouseCursorPosition = mousePosition;

        mousePosition.x = Mathf.Clamp(mousePosition.x, -m_Extents.x, m_Extents.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -m_Extents.y, m_Extents.y);

        mousePosition.x = Mathf.Round(mousePosition.x / m_TileSize + 0.5f * m_TileSize) * m_TileSize - 0.5f * m_TileSize;
        mousePosition.y = Mathf.Round(mousePosition.y / m_TileSize + 0.5f * m_TileSize) * m_TileSize - 0.5f * m_TileSize;
        m_TilePosition = mousePosition;
    }

    private bool PlaceObject()
    {
        
        bool blockedPlacement = false;
        List<Collider2D> deleteColliders = new List<Collider2D>();
        for (int i = 0; i < m_SpawnOptions.Length; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(m_TilePosition, new Vector2(m_DeleteSize, m_DeleteSize), 0.0f, m_SpawnOptions[i].m_LayerMask);
            if (colliders != null && colliders.Length > 0)
            {
                if (m_SpawnOptions[i].m_Blocking)
                {
                    blockedPlacement = true;
                }
                else
                {
                    deleteColliders.AddRange(colliders);
                }
            }
        }

        Collider2D duplicateCollider = Physics2D.OverlapBox(m_TilePosition, new Vector2(m_SelfOccludeSize, m_SelfOccludeSize), 0.0f, m_SpawnOptions[m_CurrentSpawnIndex].m_LayerMask);
        if (!blockedPlacement && !duplicateCollider)
        {
            for (int i = 0; i < deleteColliders.Count; i++)
            {
                Destroy(deleteColliders[i].gameObject);
            }

            m_TilePosition.z = m_SpawnOptions[m_CurrentSpawnIndex].m_SpawnZ;
            GameObject newSpawn = Instantiate(m_SpawnOptions[m_CurrentSpawnIndex].m_Prefab, m_TilePosition, Quaternion.identity);
            return true;
        }
        return false;
    }

    private bool DeleteObject()
    {
        OnChange.Invoke();
        List<Collider2D> deleteColliders = new List<Collider2D>();
        for (int i = 0; i < m_SpawnOptions.Length; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(m_TilePosition, new Vector2(m_DeleteSize, m_DeleteSize), 0.0f, m_SpawnOptions[i].m_LayerMask);
            if (colliders != null && colliders.Length > 0)
            {
                for (int ii = 0; ii < colliders.Length; ii++)
                {
                    deleteColliders.Add(colliders[ii]);
                }
            }
        }

        if (deleteColliders.Count > 0)
        {
            for (int i = 0; i < deleteColliders.Count; i++)
            {
                Destroy(deleteColliders[i].gameObject);
            }
            return true;
        }
        return false;
    }

    private void Update()
    {
        HandleScrollWheelInput();
        UpdateUISlots();

        CalculateMousePositions();

        m_TileHighlight.position = m_TilePosition;
        m_MouseTarget.position = m_MouseCursorPosition;

        bool changed = false;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            changed = PlaceObject() ? true : changed;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            changed = DeleteObject() ? true : changed;
        }

        // Checks for any changes before regenerating the environment (an expensive operation)
        if (changed)
        {
            m_EnvironmentGenerator.CalculateSprites();
        }
    }
}