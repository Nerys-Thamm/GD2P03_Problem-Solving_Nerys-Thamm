using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Attached Components")]
    public EnemyManager m_EnemyManager;
    public ConstructManager m_ConstructManager;
    public ScenarioManager m_ScenarioManager;

    [Header("Controls")]
    public KeyCode m_ToggleRecordingKey = KeyCode.X;
    public KeyCode m_ClearKey = KeyCode.C;
    public KeyCode m_ToggleVisibilityKey = KeyCode.V;

    [Header("Core Values")]
    public bool m_Visible = true;
    public bool m_Recording = true;

    [Header("Presentation")]
    public Vector2 m_BoxOrigin = new Vector2(0.235f, 0.865f);
    public Vector2 m_BoxScale = new Vector2(0.45f, 0.25f);
    public float m_TextScale = 1.25f;

    public float m_TextUpdateLength = 0.25f;
    private float m_TextUpdateTime = float.MinValue;

    private Material m_Material;

    [Header("Colours")]
    public Color m_TextColour =         new Color(255.0f / 255.0f,      255.0f / 255.0f,    255.0f / 255.0f,    255.0f / 255.0f);
    public Color m_GraphPointsColour =  new Color(248.0f / 255.0f,      211.0f / 255.0f,    32.0f / 255.0f,     128.0f / 255.0f);
    public Color m_GraphBoxColour =     new Color(255.0f / 255.0f,      255.0f / 255.0f,    255.0f / 255.0f,    255.0f / 255.0f);
    public Color m_GraphLineColour =    new Color(255.0f / 255.0f,      255.0f / 255.0f,    255.0f / 255.0f,    64.0f / 255.0f);
    public Color m_LowLineColour =      new Color(255.0f / 255.0f,      28.0f / 255.0f,     28.0f / 255.0f,     255.0f / 255.0f);
    public Color m_AverageLineColour =  new Color(248.0f / 255.0f,      211.0f / 255.0f,    32.0f / 255.0f,     255.0f / 255.0f);
    public Color m_HighLineColour =     new Color(39.0f / 255.0f,       214.0f / 255.0f,    29.0f / 255.0f,     255.0f / 255.0f);

    [Header("Data")]
    public int m_FramesRecorded = 1000;
    public int m_FramesDisplayedSampleRange = 100;
    public int m_MinFramesDisplayed = 30;
    public float m_SecondsDisplayed = 3.0f;

    [Header("Core Data")]
    private int m_DataIndex = 0;
    private int m_FramesDisplayed = -1;
    
    [Header("General Statistics")]
    private float[] m_Data;
    private float m_CurrentFPS;
    private float m_CurrentAverageFPS;
    private Vector2Int m_CurrentFPSRangeIndices = new Vector2Int(-1, -1);
    private Vector2 m_CurrentFPSRange = new Vector2(-1.0f, -1.0f);
    private Vector2 m_AllTimeFPSRange = new Vector2(-1.0f, -1.0f);

    [Header("Scenario Statistics")]
    private int m_ScenarioLoaded = -1;
    private bool m_ScenarioInSession = false;
    private int m_ScenariosCompleted = 0;
    private int m_ScenarioFramesRecorded = 0;
    private float m_ScenarioRunningAverage = 0.0f;
    private Vector2 m_ScenarioFPSRange = new Vector2(-1.0f, -1.0f);
    private float m_AverageScenarioRunningAverage = 0.0f;
    private Vector2 m_AverageScenarioFPSRange = new Vector2(-1.0f, -1.0f);

    [Header("Statistic Readouts")]
    private string m_CurrentFPSText = "";
    private string m_CurrentAverageFPSText = "";
    private string m_CurrentFPSRangeText = "";
    private string m_AllTimeFPSRangeText = "";
    private string m_CurrentEnemiesText = "";
    private string m_CurrentConstructsText = "";

    private string m_ScenarioText = "";
    private string m_ScenariosCompletedText = "";
    private string m_ScenarioAverageFPSText = "";
    private string m_ScenarioFPSRangeText = "";

    [Header("Stopwatch")]
    private System.Diagnostics.Stopwatch m_Stopwatch;
    private bool m_StopwatchInitialized = false;

    private void Reset()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (!m_Material)
        {
            m_Material = new Material(Shader.Find("Sprites/Default"));
            m_Material.color = Color.white;
            m_Material.mainTexture = null;
            m_Material.name = string.Format("(Instanced) {0}", m_Material.name);
        }

        if (!GetComponent<Camera>())
        {
            Debug.LogError("The Stats Manager must be attached to a camera in the scene to be able to draw.");
        }

        if (!m_EnemyManager)
        {
            m_EnemyManager = FindObjectOfType<EnemyManager>();
        } 
        if (!m_ConstructManager)
        {
            m_ConstructManager = FindObjectOfType<ConstructManager>();
        } 
        if (!m_ScenarioManager)
        {
            m_ScenarioManager = FindObjectOfType<ScenarioManager>();
        } 
    }

    private void Awake()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        m_Stopwatch = new System.Diagnostics.Stopwatch();
        m_StopwatchInitialized = false;

        m_Data = new float[m_FramesRecorded];
        for (int i = 0; i < m_FramesRecorded; i++)
        {
            m_Data[i] = -1.0f;
        }
        
        m_DataIndex = 0;

        m_CurrentFPSRange.x = float.MaxValue;
        m_CurrentFPSRange.y = float.MinValue;
        m_AllTimeFPSRange.x = float.MaxValue;
        m_AllTimeFPSRange.y = float.MinValue;
    }

    public void SetupScenario(int _scenario)
    {
        if (m_ScenarioLoaded != _scenario)
        {
            m_ScenariosCompleted = 0;
            m_AverageScenarioRunningAverage = -1.0f;
            m_AverageScenarioFPSRange.x = -1.0f;
            m_AverageScenarioFPSRange.y = -1.0f;
        }
        m_ScenarioRunningAverage = 0.0f;
        m_ScenarioFPSRange.x = float.MaxValue;
        m_ScenarioFPSRange.x = float.MinValue;
        m_ScenarioFramesRecorded = 0;
        m_ScenarioLoaded = _scenario;

        if (_scenario > 0)
        {
            m_ScenarioInSession = true;
        }
    }

    private float GetFPSAt(int _relativeIndex)
    {
        _relativeIndex += m_FramesRecorded;
        return m_Data[_relativeIndex % m_FramesRecorded];
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(m_ClearKey))
        {
            ResetStats();
        }
        if (Input.GetKeyDown(m_ToggleRecordingKey))
        {
            m_Recording = !m_Recording;
        }
        if (Input.GetKeyDown(m_ToggleVisibilityKey))
        {
            m_Visible = !m_Visible;
        }
    }

    private void CalculateFramesDisplayed()
    {
        int validFrames = 0;
        float sum = 0.0f;
        for (int i = 0; i < m_FramesDisplayedSampleRange; i++)
        {
            float value = GetFPSAt(m_DataIndex - i);
            if (value < 0.0f)
            {
                continue;
            }
            sum += value;
            validFrames++;
        }
        float movingAverage = sum / (float)validFrames;
        m_FramesDisplayed = Mathf.Clamp((int)(m_SecondsDisplayed * movingAverage), m_MinFramesDisplayed, m_FramesRecorded);
    }

    private void CalculateStatistics()
    {
        int bestFrameIndex = -1;
        float bestFrameValue = float.MinValue;
        int worstFrameIndex = -1;
        float worstFrameValue = float.MaxValue;
        int validFrames = 0;
        m_CurrentAverageFPS = 0.0f;
        for (int i = 0; i < m_FramesDisplayed; i++)
        {
            float value = GetFPSAt(m_DataIndex - i);
            if (value < 0.0f)
            {
                continue;
            }
            validFrames++;
            m_CurrentAverageFPS += value;
            if (value > bestFrameValue)
            {
                bestFrameIndex = i;
                bestFrameValue = value;
            }
            if (value < worstFrameValue)
            {
                worstFrameIndex = i;
                worstFrameValue = value;
            }
        }
        m_CurrentAverageFPS /= (float)validFrames;

        m_CurrentFPSRange.x = worstFrameValue;
        m_CurrentFPSRangeIndices.x = worstFrameIndex;
        m_CurrentFPSRange.y = bestFrameValue;
        m_CurrentFPSRangeIndices.y = bestFrameIndex;

        if (m_CurrentFPSRange.x < m_AllTimeFPSRange.x)
        {
            m_AllTimeFPSRange.x = m_CurrentFPSRange.x;
        }
        if (m_CurrentFPSRange.y > m_AllTimeFPSRange.y)
        {
            m_AllTimeFPSRange.y = m_CurrentFPSRange.y;
        }
    }

    private void ProcessScenario()
    {
        if (m_CurrentAverageFPS > 0)
        {
            m_ScenarioRunningAverage *= (float)m_ScenarioFramesRecorded;
            m_ScenarioRunningAverage += m_CurrentAverageFPS;
            m_ScenarioRunningAverage /= (float)(m_ScenarioFramesRecorded + 1);

            if (m_CurrentFPSRange.x > 0.0f)
            {
                m_ScenarioFPSRange.x = m_CurrentFPSRange.x;
            }
            if (m_CurrentFPSRange.y > 0.0f)
            {
                m_ScenarioFPSRange.y = m_CurrentFPSRange.y;
            }
            
            m_ScenarioFramesRecorded++;     
        }

        if (m_ConstructManager.m_Constructs.Count <= 0)
        {
            if (m_ScenariosCompleted == 0)
            {
                m_AverageScenarioRunningAverage = m_ScenarioRunningAverage;
                m_AverageScenarioFPSRange = m_ScenarioFPSRange;
            }
            else
            {
                m_AverageScenarioRunningAverage *= (float)m_ScenariosCompleted;
                m_AverageScenarioRunningAverage += m_ScenarioRunningAverage;
                m_AverageScenarioRunningAverage /= (float)(m_ScenariosCompleted + 1);

                m_AverageScenarioFPSRange.x *= (float)m_ScenariosCompleted;
                m_AverageScenarioFPSRange.x += m_ScenarioFPSRange.x;
                m_AverageScenarioFPSRange.x /= (float)(m_ScenariosCompleted + 1);

                m_AverageScenarioFPSRange.y *= (float)m_ScenariosCompleted;
                m_AverageScenarioFPSRange.y += m_ScenarioFPSRange.y;
                m_AverageScenarioFPSRange.y /= (float)(m_ScenariosCompleted + 1);
            }

            m_ScenariosCompleted++;
            m_ScenarioInSession = false;
        }
    }

    private void Update()
    {  
        ProcessInput();

        if (!m_Recording)
        {
            m_StopwatchInitialized = false;
            return;
        }

        if (m_StopwatchInitialized)
        {
            m_Stopwatch.Stop();
            m_CurrentFPS = (float)(System.Diagnostics.Stopwatch.Frequency) / (float)m_Stopwatch.Elapsed.Ticks;
        }
        else
        {
            m_CurrentFPS = -1.0f;
            m_StopwatchInitialized = true;
        }

        m_Data[m_DataIndex] = m_CurrentFPS;

        CalculateFramesDisplayed();

        CalculateStatistics();

        if (m_ScenarioInSession)
        {
            ProcessScenario();
        }

        m_DataIndex = (m_DataIndex + 1) % m_FramesRecorded;

        m_Stopwatch.Restart();
    }

    private float RemapValue(float _v, float _iMin, float _iMax, float _oMin, float _oMax)
    {
        return Mathf.Lerp(_oMin, _oMax, Mathf.InverseLerp(_iMin, _iMax, _v));
    }

    private float DrawHorizontalLine(float _v, List<float> _drawn, Vector2 _fpsExtents, Vector2[] _box, Color _c)
    {
        if (_v < _fpsExtents.x || _v > _fpsExtents.y)
        {
            return float.MaxValue;
        }

        Vector2[] line = new Vector2[]
        {
            new Vector2(0.0f, _v),
            new Vector2(1.0f + 0.01f, _v),
        };

        for (int i = 0; i < line.Length; i++)
        {
            line[i].x = Mathf.LerpUnclamped(_box[0].x, _box[2].x, line[i].x);
            line[i].y = RemapValue(line[i].y, _fpsExtents.x, _fpsExtents.y, _box[0].y, _box[2].y);
        }

        PointSet set = new PointSet(line, false);
        GLUtilities.DrawPointSet(set, _c);

        float drawnCutoffRange = 0.015f;
        for (int i = 0; i < _drawn.Count; i++)
        {
            if (line[0].y > _drawn[i] - drawnCutoffRange && line[0].y < _drawn[i] + drawnCutoffRange)
            {
                return float.MaxValue;
            }
        }
        
        GLUtilities.DrawText(line[1] + new Vector2(0.005f, -m_TextScale * 0.005f), _v.ToString("F1"), m_TextScale, _c);

        return line[0].y;
    }

    private void DrawFPSGraph(Vector2[] _box)
    {
        float range = m_CurrentFPSRange.y - m_CurrentFPSRange.x;
        Vector2 fpsExtents = new Vector2(m_CurrentFPSRange.x - 0.1f * range, m_CurrentFPSRange.y + 0.1f * range);

        Vector2[] graphPoints = new Vector2[m_FramesDisplayed];
        float lastValidData = -1.0f;
        for (int i = 0; i < m_FramesDisplayed; i++)
        {
            float value = GetFPSAt(m_DataIndex - i - 1);
            if (value < 0.0f)
            {
                value = lastValidData;
            }
            graphPoints[i] = new Vector2(m_FramesDisplayed - (float)i - 1.0f, Mathf.Max(0.0f, value));
            graphPoints[i].x = RemapValue(graphPoints[i].x, 0.0f, (float)m_FramesDisplayed, _box[0].x, _box[2].x);
            graphPoints[i].y = RemapValue(graphPoints[i].y, fpsExtents.x, fpsExtents.y, _box[0].y, _box[2].y);
            lastValidData = value;
        }

        List<float> drawnHeights = new List<float>(16);
        drawnHeights.Add(DrawHorizontalLine(m_CurrentAverageFPS, drawnHeights, fpsExtents, _box, m_AverageLineColour));
        drawnHeights.Add(DrawHorizontalLine(m_CurrentFPSRange.x, drawnHeights, fpsExtents, _box, m_LowLineColour));
        drawnHeights.Add(DrawHorizontalLine(m_CurrentFPSRange.y, drawnHeights, fpsExtents, _box, m_HighLineColour));
        drawnHeights.Add(DrawHorizontalLine(m_AllTimeFPSRange.x, drawnHeights, fpsExtents, _box, m_LowLineColour));
        drawnHeights.Add(DrawHorizontalLine(m_AllTimeFPSRange.y, drawnHeights, fpsExtents, _box, m_HighLineColour));
        drawnHeights.Add(DrawHorizontalLine(10.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(30.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(60.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(90.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(120.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(200.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));
        drawnHeights.Add(DrawHorizontalLine(300.0f, drawnHeights, fpsExtents, _box, m_GraphLineColour));

        PointSet set = new PointSet(graphPoints, false);
        GLUtilities.DrawPointSet(set, m_GraphPointsColour);

        PointSet box = new PointSet(_box, true);
        GLUtilities.DrawPointSet(box, m_GraphBoxColour);
    }

    private void UpdateTextStrings(string _format)
    {
        string currentFPS = m_CurrentFPS > 0.0f ? m_CurrentFPS.ToString(_format) : "N/A";
        m_CurrentFPSText = string.Format("FPS: {0}", currentFPS);
        string currentAverageFPS = m_CurrentAverageFPS > 0.0f ? m_CurrentAverageFPS.ToString(_format) : "N/A";
        m_CurrentAverageFPSText = string.Format("AVG: {0}", currentAverageFPS);
        string currentLowFPS = m_CurrentFPSRange.x > 0.0f && m_CurrentFPSRange.x < 10000.0f ? m_CurrentFPSRange.x.ToString(_format) : "N/A";
        string currentHighFPS = m_CurrentFPSRange.y > 0.0f && m_CurrentFPSRange.y < 10000.0f ? m_CurrentFPSRange.y.ToString(_format) : "N/A";
        m_CurrentFPSRangeText = string.Format("RNG: {0}-{1}", currentLowFPS, currentHighFPS);
        string allTimeLowFPS = m_AllTimeFPSRange.x > 0.0f && m_AllTimeFPSRange.x < 10000.0f ? m_AllTimeFPSRange.x.ToString(_format) : "N/A";
        string allTimeHighFPS = m_AllTimeFPSRange.y > 0.0f && m_AllTimeFPSRange.y < 10000.0f ? m_AllTimeFPSRange.y.ToString(_format) : "N/A";
        m_AllTimeFPSRangeText = string.Format("ALL: {0}-{1}", allTimeLowFPS, allTimeHighFPS);

        int enemyCount = m_EnemyManager.m_Enemies.Count;
        m_CurrentEnemiesText = string.Format("ENEMIES: {0}", enemyCount.ToString());
        m_CurrentConstructsText = string.Format("CONSTRUCTS: {0}", m_ConstructManager.m_Constructs.Count.ToString());

        string scenario = m_ScenarioLoaded >= 0 ? m_ScenarioLoaded.ToString() : "N/A";
        m_ScenarioText = string.Format("SCENARIO: {0}", scenario);

        string currentScenario = m_ScenarioLoaded >= 0 ? ((m_ScenarioInSession ? 1 : 0) + m_ScenariosCompleted).ToString() : "N/A";
        string scenariosCompleted = m_ScenarioLoaded >= 0 ? m_ScenariosCompleted.ToString() : "N/A";
        m_ScenariosCompletedText = string.Format("RUN: {0} ({1})", currentScenario, scenariosCompleted);

        string averageScenarioFPS = (m_ScenarioLoaded >= 0 && m_AverageScenarioRunningAverage > 0.0f) ? m_AverageScenarioRunningAverage.ToString(_format) : "N/A";
        m_ScenarioAverageFPSText = string.Format("AVG: {0}", averageScenarioFPS);
        string averageScenarioLow = (m_ScenarioLoaded >= 0 && m_AverageScenarioFPSRange.x > 0.0f) ? m_AverageScenarioFPSRange.x.ToString(_format) : "N/A";
        string averageScenarioHigh = (m_ScenarioLoaded >= 0 && m_AverageScenarioFPSRange.y > 0.0f) ? m_AverageScenarioFPSRange.y.ToString(_format) : "N/A";
        m_ScenarioFPSRangeText = string.Format("RNG: {0}-{1}", averageScenarioLow, averageScenarioHigh);
    }

    private void DrawTextStatistics(Vector2[] _box)
    {
        float xStep = (_box[2].x - _box[0].x) / 4.0f;
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 0, -0.02f), m_CurrentFPSText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 1, -0.02f), m_CurrentAverageFPSText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 2, -0.02f), m_CurrentFPSRangeText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 3, -0.02f), m_AllTimeFPSRangeText, m_TextScale, m_TextColour);

        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 0, -0.05f), m_ScenarioText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 1, -0.05f), m_ScenariosCompletedText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 2, -0.05f), m_ScenarioAverageFPSText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 3, -0.05f), m_ScenarioFPSRangeText, m_TextScale, m_TextColour);

        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 0, -0.08f), m_CurrentEnemiesText, m_TextScale, m_TextColour);
        GLUtilities.DrawText(_box[0] + new Vector2(xStep * 1, -0.08f), m_CurrentConstructsText, m_TextScale, m_TextColour);
    }

    private void OnPostRender()
    {
        if (!m_Visible)
        {
            return;
        }

        Vector2[] box = new Vector2[4]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, -0.5f)
        }; 

        for (int i = 0; i < box.Length; i++)
        {
            box[i].x *= m_BoxScale.x;
            box[i].y *= m_BoxScale.y;
            box[i] += m_BoxOrigin;
        }

        GL.PushMatrix();
        m_Material.SetPass(0);
        GL.LoadOrtho();

        DrawFPSGraph(box);

        if (Time.time > m_TextUpdateTime)
        {
            m_TextUpdateTime = Time.time + m_TextUpdateLength;
            UpdateTextStrings("F1");
        }
        DrawTextStatistics(box);

        GL.PopMatrix();
    }
}
