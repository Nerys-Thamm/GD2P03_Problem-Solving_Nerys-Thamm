using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointSet
{
    public Vector2[] m_Points;
    public bool m_ReuseStart;

    public PointSet(Vector2[] _points, bool _reuseStart)
    {
        m_Points = _points;
        m_ReuseStart = _reuseStart;
    }

    public PointSet(Vector2[] _points)
    {
        m_Points = _points;
        m_ReuseStart = false;
    }

    public PointSet(float[] _points, bool _reuseStart)
    {
        m_Points = new Vector2[_points.Length / 2];
        for (int i = 0; i < m_Points.Length; i++)
        {
            m_Points[i] = new Vector2(_points[i * 2], _points[i * 2 + 1]);
        }
        m_ReuseStart = _reuseStart;
    }

    public PointSet(float[] _points)
    {
        m_Points = new Vector2[_points.Length / 2];
        for (int i = 0; i < m_Points.Length; i++)
        {
            m_Points[i] = new Vector2(_points[i * 2], _points[i * 2 + 1]);
        }
        m_ReuseStart = false;
    }
}

public static class GLUtilities
{
    public static void DrawPointSet(PointSet _pointSet)
    {
        DrawPointSet(_pointSet, Vector2.zero, Vector2.one, Color.white, GL.LINE_STRIP);
    }

    public static void DrawPointSet(PointSet _pointSet, Color _c)
    {
        DrawPointSet(_pointSet, Vector2.zero, Vector2.one, _c, GL.LINE_STRIP);
    }

    public static void DrawPointSet(PointSet _pointSet, int _mode)
    {
        DrawPointSet(_pointSet, Vector2.zero, Vector2.one, Color.white, _mode);
    }

    public static void DrawPointSet(PointSet _pointSet, Vector2 _offset, Vector2 _scale, Color _c, int _mode)
    {
        GL.Begin(_mode);
        GL.Color(_c);
        for (int i = 0; i < _pointSet.m_Points.Length; i++)
        {
            Vector2 point = _pointSet.m_Points[i];
            point.x *= _scale.x;
            point.y *= _scale.y;
            point += _offset;
            GL.Vertex(point);
        }
        if (_pointSet.m_ReuseStart)
        {
            Vector2 point = _pointSet.m_Points[0];
            point.x *= _scale.x;
            point.y *= _scale.y;
            point += _offset;
            GL.Vertex(point);
        }
        GL.End();
    }

    public static void DrawPointSet(PointSet[] _pointSets)
    {
        DrawPointSet(_pointSets, Vector2.zero, Vector2.one, Color.white, GL.LINE_STRIP);
    }

    public static void DrawPointSet(PointSet[] _pointSets, Color _c)
    {
        DrawPointSet(_pointSets, Vector2.zero, Vector2.one, _c, GL.LINE_STRIP);
    }

    public static void DrawPointSet(PointSet[] _pointSets, int _mode)
    {
        DrawPointSet(_pointSets, Vector2.zero, Vector2.one, Color.white, _mode);
    }
    
    public static void DrawPointSet(PointSet[] _pointSets, Vector2 _offset, Vector2 _scale, Color _c, int _mode)
    {
        for (int i = 0; i < _pointSets.Length; i++)
        {
            DrawPointSet(_pointSets[i], _offset, _scale, _c, _mode);
        }
    }

    public static void DrawText(Vector2 _origin, string _text, float _scale, Color _c)
    {
        _text = _text.ToUpper();

        Vector2 pos = _origin;

        Vector2 strictScale = new Vector2(_scale * 0.001f, _scale * 0.001f);
        strictScale.x /= (float)Screen.width / (float)Screen.height;

        for (int i = 0; i < _text.Length; i++)
        {
            char c = _text[i];
            if (c == '\r' || c == '\n')
            {
                pos.x = _origin.x;
                pos.y -= (VectorFont.m_MonoCharacterSize.y + VectorFont.m_MonoCharacterPadding.y) * strictScale.y; 
            }
            else
            {
                PointSet[] set = VectorFont.m_Characters[(int)c - VectorFont.m_CharacterUnicodeOffset].m_PointSets;
                DrawPointSet(set, pos, strictScale, _c, GL.LINE_STRIP);
                pos.x += (VectorFont.m_MonoCharacterSize.x + VectorFont.m_MonoCharacterPadding.x) * strictScale.x;
            }
        }
    }
}
