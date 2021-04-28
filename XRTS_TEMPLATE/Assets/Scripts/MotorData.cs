using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MotorData", fileName = "New Motor Data")]
public class MotorData : ScriptableObject
{
    [Range(0.0f, 10.0f)]
    public float m_speed_multiplier;

    [Range(0.0001f, 20.0f)]
    public float m_max_speed;

    public AnimationCurve m_accel;
    public AnimationCurve m_decel;
    public AnimationCurve m_inertia;

}
