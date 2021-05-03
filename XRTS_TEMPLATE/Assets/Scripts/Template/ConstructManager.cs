using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructManager : MonoBehaviour
{
    public UnityEvent OnNavGenerate;

    public bool GenerateThisFrame = false;

    public List<Construct> m_Constructs = new List<Construct>(128);

    // Can be used to quickly get a valid construct (if one exists)
    public Transform GetConstruct()
    {
        if (m_Constructs == null || m_Constructs.Count <= 0 || !m_Constructs[0])
        {
            return null;
        }
        else
        {
            return m_Constructs[0].transform;
        }
    }

    private void Update()
    {
        // Constantly checks and removes any null constructs (ones destroyed or cleared somehow)
        for (int i = m_Constructs.Count - 1; i >= 0; i--)
        {
            if (!m_Constructs[i])
            {
                GenerateThisFrame = true;
                m_Constructs.RemoveAt(i);
            }
        }
    }

    private void LateUpdate()
    {
        if(GenerateThisFrame)
        {
            OnNavGenerate.Invoke();
            GenerateThisFrame = false;
        }
    }
}