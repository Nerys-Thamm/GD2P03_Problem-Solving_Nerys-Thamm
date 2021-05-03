using UnityEngine;

public class Construct : MonoBehaviour
{
    public int m_MaxHealth = 100;
    public int m_CurrentHealth = 10;

    public TemporaryAnimation m_SpawnAnimation;

    public int m_BuildingSpawnVisibilityIndex = 9;

    public GameObject m_HitPrefab;
    public GameObject m_ExplosionPrefab;

    private void Awake()
    {
        // Hooks us up to the construct manager by default so that we can find it later without searching the scene
        FindObjectOfType<ConstructManager>().m_Constructs.Add(this);
        FindObjectOfType<ConstructManager>().GenerateThisFrame = true;
        m_CurrentHealth = m_MaxHealth;
    }

    // Use this to do incremental damage to the construct!
    public void TakeDamage(Vector3 _origin, int _damage)
    {
        _origin.z = transform.position.z;
        Vector3 hitSpawnPos = _origin - transform.position;
        float angle = Mathf.Atan2(hitSpawnPos.x, -hitSpawnPos.y) * Mathf.Rad2Deg;

        hitSpawnPos = hitSpawnPos.normalized * 0.5f + transform.position;
        hitSpawnPos.z -= 5.0f;

        Instantiate(m_HitPrefab, hitSpawnPos, Quaternion.Euler(0.0f, 0.0f, Mathf.Round(angle / 90.0f) * 90.0f));

        m_CurrentHealth -= _damage;
        if (m_CurrentHealth <= 0)
        {
            Explode();
        }
    }

    // Use this to destroy the construct in a nicer fashion!
    public void Explode()
    {
        Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void Update()
    {
        // Checking to see if we should adjust our actual building's z depth for layering
        if (!m_SpawnAnimation || m_SpawnAnimation.m_CurrentIndex > m_BuildingSpawnVisibilityIndex)
        {
            m_SpawnAnimation.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}