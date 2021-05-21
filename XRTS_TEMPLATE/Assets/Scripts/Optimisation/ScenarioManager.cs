using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public MouseSpawner m_MouseSpawner;

    public EnvironmentGenerator m_EnvironmentGenerator;
    //public PathfindingGrid m_PathfindingGrid; // <--- REMOVE
    
    public StatsManager m_StatsManager;

    public MouseSpawner.SpawnOption m_WallSpawn;
    public MouseSpawner.SpawnOption m_ConstructSpawn;
    public MouseSpawner.SpawnOption m_Enemy1Spawn;
    public MouseSpawner.SpawnOption m_Enemy2Spawn;
    public MouseSpawner.SpawnOption m_Enemy3Spawn;

    public LayerMask m_ClearLayerMask; 

    private void Reset()
    {
        OnValidate();
        m_ClearLayerMask = LayerMask.GetMask("Environment", "Enemy", "Target");
    }

    private void OnValidate()
    {
        if (!m_MouseSpawner)
        {
            m_MouseSpawner = FindObjectOfType<MouseSpawner>();
        } 
        if (!m_EnvironmentGenerator)
        {
            m_EnvironmentGenerator = FindObjectOfType<EnvironmentGenerator>();
        } 
        if (!m_StatsManager)
        {
            m_StatsManager = FindObjectOfType<StatsManager>();
        } 
    }

    private void ClearMap()
    {
        Vector2 scale = m_MouseSpawner.m_Extents * 2.0f;
        scale *= m_EnvironmentGenerator.m_TileSize;
        scale.x += 0.75f;
        scale.y += 0.75f;
        Collider2D[] collisions = Physics2D.OverlapBoxAll(m_EnvironmentGenerator.transform.position, scale, 0.0f, m_ClearLayerMask);
        for (int i = collisions.Length - 1; i >= 0; i--)
        {
            Destroy(collisions[i].gameObject);
        }
    }

    private void Spawn(GameObject _prefab, float _x, float _y, float _z, float _randomScale)
    {
        float xOffset = Random.Range(-_randomScale, _randomScale);
        float yOffset = Random.Range(-_randomScale, _randomScale);
        Instantiate(_prefab, new Vector3(_x + xOffset, _y + yOffset, _z), Quaternion.identity);
    }

    private void Spawn(GameObject _prefab, float _x, float _y, float _z, float _randomScale, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            Spawn(_prefab, _x, _y, _z, _randomScale);
        }
    }

    private void Spawn(MouseSpawner.SpawnOption _spawnOption, float _x, float _y)
    {
        Spawn(_spawnOption.m_Prefab, _x, _y, _spawnOption.m_SpawnZ, 0.0f);
    }

    private void Spawn(MouseSpawner.SpawnOption _spawnOption, float _x, float _y, float _randomScale)
    {
        Spawn(_spawnOption.m_Prefab, _x, _y, _spawnOption.m_SpawnZ, _randomScale);
    }

    private void Spawn(MouseSpawner.SpawnOption _spawnOption, float _x, float _y, float _randomScale, int _count)
    {
        Spawn(_spawnOption.m_Prefab, _x, _y, _spawnOption.m_SpawnZ, _randomScale, _count);      
    }

    private void Update()
    {
        int chosenScenario = -1;

        chosenScenario = Input.GetKeyDown(KeyCode.Alpha0) ? 0 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha1) ? 1 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha2) ? 2 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha3) ? 3 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha4) ? 4 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha5) ? 5 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha6) ? 6 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha7) ? 7 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha8) ? 8 : chosenScenario;
        chosenScenario = Input.GetKeyDown(KeyCode.Alpha9) ? 9 : chosenScenario;

        if (chosenScenario >= 0)
        {
            ClearMap();
            switch (chosenScenario)
            {
                case 0: {} break; // Clear
                case 1: // Basic enemies 1 (100E, 1C)
                {
                    Spawn(m_Enemy1Spawn, -3.5f, 3.5f, 0.1f, 100);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                case 2: // Basic enemies 2 (100E, 1C) 
                {
                    Spawn(m_Enemy2Spawn, -3.5f, 3.5f, 0.1f, 100);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                case 3: // Basic enemies 3 (100E, 1C)
                {
                    Spawn(m_Enemy3Spawn, -3.5f, 3.5f, 0.1f, 100);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                case 4: // Stress test divided enemies and targets (80E, 8C)
                {
                    Spawn(m_Enemy1Spawn, -3.0f, -3.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, -4.0f, 0.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, -3.0f, 3.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, 0.0f, 4.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, 3.0f, 3.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, 4.0f, 0.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, 3.0f, -3.0f, 0.1f, 10);
                    Spawn(m_Enemy1Spawn, 0.0f, -4.0f, 0.1f, 10);

                    Spawn(m_ConstructSpawn, -8.5f, -2.5f, 0.0f);
                    Spawn(m_ConstructSpawn, -6.5f, -4.5f, 0.0f);
                    Spawn(m_ConstructSpawn, -8.5f, 2.5f, 0.0f);
                    Spawn(m_ConstructSpawn, -6.5f, 4.5f, 0.0f);
                    Spawn(m_ConstructSpawn, 8.5f, 2.5f, 0.0f);
                    Spawn(m_ConstructSpawn, 6.5f, 4.5f, 0.0f);
                    Spawn(m_ConstructSpawn, 8.5f, -2.5f, 0.0f);
                    Spawn(m_ConstructSpawn, 6.5f, -4.5f, 0.0f);
                } break;
                case 5: // Stress test a row run (100E, 10C)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Spawn(m_Enemy1Spawn, -8.5f, 4.5f - (float)i, 0.1f, 10);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        Spawn(m_ConstructSpawn, 8.5f, 4.5f - (float)i, 0.0f);
                    }
                } break;
                case 6: // Annoying pathfinding test w/ 100 enemies (100E, 1C)
                {
                    Spawn(m_Enemy1Spawn, 5.5f, 4.5f, 0.1f, 25);
                    Spawn(m_Enemy1Spawn, 5.5f, 1.5f, 0.1f, 25);
                    Spawn(m_Enemy1Spawn, 5.5f, -0.5f, 0.1f, 25);
                    Spawn(m_Enemy1Spawn, 5.5f, -2.5f, 0.1f, 25);

                    for (int i = 0; i < 9; i++)
                    {
                        Spawn(m_WallSpawn, 6.5f, 4.5f - (float)i, 0.0f);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Spawn(m_WallSpawn, 6.5f - (float)i, 2.5f, 0.0f);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Spawn(m_WallSpawn, 6.5f - (float)i, 0.5f, 0.0f);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Spawn(m_WallSpawn, 6.5f - (float)i, -1.5f, 0.0f);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Spawn(m_WallSpawn, 6.5f - (float)i, -3.5f, 0.0f);
                    }
                    Spawn(m_WallSpawn, 7.5f, 4.5f, 0.0f);
                    Spawn(m_WallSpawn, 7.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 7.5f, -3.5f, 0.0f);
                    Spawn(m_WallSpawn, 8.5f, 2.5f, 0.0f);
                    Spawn(m_WallSpawn, 8.5f, -1.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 8.5f, 4.5f, 0.0f);
                } break;
                case 7: // Stress test 500 enemies (500E, 1C)
                {
                    Spawn(m_Enemy1Spawn, -3.5f, 3.5f, 0.1f, 500);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                case 8: // Stress test 1000 enemies (1000E, 1C)
                {
                    Spawn(m_Enemy1Spawn, -3.5f, 3.5f, 0.1f, 1000);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                case 9: // Bound to break most projects, 5000 enemies (5000E, 1C)
                {
                    Spawn(m_Enemy1Spawn, -3.5f, 3.5f, 0.1f, 5000);

                    Spawn(m_WallSpawn, 0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, 0.5f, -0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, 0.5f, 0.0f);
                    Spawn(m_WallSpawn, -0.5f, -0.5f, 0.0f);

                    Spawn(m_ConstructSpawn, 3.5f, -3.5f, 0.0f);
                } break;
                default: {} break;
            }

            m_EnvironmentGenerator.CalculateSprites();


            
            m_StatsManager.ResetStats();

            m_StatsManager.SetupScenario(chosenScenario);
        }
    }
}
