using UnityEngine;

public class TemporaryAnimation : MonoBehaviour
{
    public SpriteRenderer m_Display;
    public Sprite[] m_Animation;

    public bool m_DestroyOnFinish = true;

    public int m_CurrentIndex;
    public float m_FrameTime = 0.1f;
    private float m_Timer = 0.0f;

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer > m_FrameTime)
        {
            m_Timer -= m_FrameTime;
            m_CurrentIndex++;
        }

        m_Display.sprite = m_Animation[Mathf.Min(m_CurrentIndex, m_Animation.Length - 1)];

        if (m_DestroyOnFinish && m_CurrentIndex == m_Animation.Length)
        {
            Destroy(gameObject);
        }
    }
}