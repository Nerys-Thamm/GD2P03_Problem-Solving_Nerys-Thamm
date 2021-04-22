using UnityEngine;

[CreateAssetMenu(menuName = "XRTS/Sprite Data")]
public class CharacterSpriteData : ScriptableObject
{
    public Sprite[] m_RunSpriteSheet;
    public Sprite[] m_AttackSpriteSheet;
}