using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFont
{
    public struct Character
    {
        public char m_Character;
        public PointSet[] m_PointSets;

        public Character(char _character, PointSet[] _points)
        {
            m_Character = _character;
            m_PointSets = _points;
        }
    }

    public static string m_TestString = "!\"#$%&\'()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[\\]^_";
    public static int m_CharacterUnicodeOffset = 32;
    public static Vector2 m_MonoCharacterSize = new Vector2(6.0f, 8.0f);
    public static Vector2 m_MonoCharacterPadding = new Vector2(2.0f, 4.0f);

    public static Character[] m_Characters = new Character[]
    {
        new Character(' ', new PointSet[] {}),
        new Character('!', new PointSet[] {
            new PointSet(new float[] {  3, 0,   2, 1,   3, 2,   4, 1 }, true),
            new PointSet(new float[] {  3, 3,   3, 8 })
        }),
        new Character('"', new PointSet[] {
            new PointSet(new float[] {  0, 8,   2, 8,   2, 7,   1, 6,   1, 7,   0, 7 }, true),
            new PointSet(new float[] {  4, 8,   6, 8,   6, 7,   5, 6,   5, 7,   4, 7 }, true)
        }),
        new Character('#', new PointSet[] {
            new PointSet(new float[] {  2, 0,   2, 8 }),
            new PointSet(new float[] {  4, 0,   4, 8 }),
            new PointSet(new float[] {  0, 3,   6, 3 }),
            new PointSet(new float[] {  0, 5,   6, 5 })
        }),
        new Character('$', new PointSet[] {
            new PointSet(new float[] {  6, 6,   5, 7,   1, 7,   0, 6,   0, 5,   1, 4,   5, 4,   6, 3,   6, 2,   5, 1,   1, 1,   0, 2 }),
            new PointSet(new float[] {  3, 0,   3, 8 })
        }),
        new Character('%', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 1,   6, 7,   6, 8 }),
            new PointSet(new float[] {  1, 4,   0, 5,   1, 6,   2, 5 }, true),
            new PointSet(new float[] {  5, 2,   4, 3,   5, 4,   6, 3 }, true)
        }),
        new Character('&', new PointSet[] {
            new PointSet(new float[] {  6, 0,   0, 6,   0, 7,   1, 8,   2, 8,   3, 7,   3, 6,   0, 3,   0, 1,   1, 0,   4, 0,   6, 2 }),
        }),
        new Character('\'', new PointSet[] {
            new PointSet(new float[] {  2, 8,   4, 8,   4, 7,   3, 6,   3, 7,   2, 7 }, true)
        }),
        new Character('(', new PointSet[] {
            new PointSet(new float[] {  4, 0,   2, 2,   2, 6,   4, 8 })
        }),
        new Character(')', new PointSet[] {
            new PointSet(new float[] {  2, 0,   4, 2,   4, 6,   2, 8 })
        }),
        new Character('*', new PointSet[] {
            new PointSet(new float[] {  1, 4,   5, 4 }),
            new PointSet(new float[] {  3, 2,   3, 6 }),
            new PointSet(new float[] {  1, 2,   5, 6 }),
            new PointSet(new float[] {  5, 2,   1, 6 })
        }),
        new Character('+', new PointSet[] {
            new PointSet(new float[] {  1, 4,   5, 4 }),
            new PointSet(new float[] {  3, 2,   3, 6 })
        }),
        new Character(',', new PointSet[] {
            new PointSet(new float[] {  2, 2,   4, 2,   4, 1,   3, 0,   3, 1,   2, 1 }, true)
        }),
        new Character('-', new PointSet[] {
            new PointSet(new float[] {  1, 4,   5, 4 }),
        }),
        new Character('.', new PointSet[] {
            new PointSet(new float[] {  3, 0,   2, 1,   3, 2,   4, 1 }, true)
        }),
        new Character('/', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 1,   6, 7,   6, 8 })
        }),
        new Character('0', new PointSet[] {
            new PointSet(new float[] {  0, 1,   0, 7,   1, 8,   5, 8,   6, 7,   6, 1,   5, 0,   1, 0 }, true),
            new PointSet(new float[] {  0, 1,   6, 7 })
        }),
        new Character('1', new PointSet[] {
            new PointSet(new float[] {  0, 0,   6, 0 }),
            new PointSet(new float[] {  3, 0,   3, 8,   1, 6 })
        }),
        new Character('2', new PointSet[] {
            new PointSet(new float[] {  0, 7,   1, 8,   5, 8,   6, 7,   6, 6,   0, 0,   6, 0 })
        }),
        new Character('3', new PointSet[] {
            new PointSet(new float[] {  0, 7,   1, 8,   5, 8,   6, 7,   6, 6,   5, 5 }),
            new PointSet(new float[] {  1, 5,   5, 5,   6, 4,   6, 1,   5, 0,   1, 0,   0, 1 })
        }),
        new Character('4', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 6,   1, 5,   6, 5 }),
            new PointSet(new float[] {  6, 8,   6, 0 })
        }),
        new Character('5', new PointSet[] {
            new PointSet(new float[] {  0, 2,   2, 0,   4, 0,   6, 2,   6, 4,   5, 5,   0, 5,   0, 8,   6, 8 })
        }),
        new Character('6', new PointSet[] {
            new PointSet(new float[] {  6, 7,   5, 8,   1, 8,   0, 7,   0, 1,   1, 0,   5, 0,   6, 1,   6, 4,   5, 5,   0, 5 })
        }),
        new Character('7', new PointSet[] {
            new PointSet(new float[] {  0, 8,   6, 8,   6, 7,   3, 4,   3, 0 })
        }),
        new Character('8', new PointSet[] {
            new PointSet(new float[] {  1, 5,   0, 6,   0, 7,   1, 8,   5, 8,   6, 7,   6, 6,   5, 5 }, true),
            new PointSet(new float[] {  1, 5,   0, 4,   0, 1,   1, 0,   5, 0,   6, 1,   6, 4,   5, 5 })
        }),
        new Character('9', new PointSet[] {
            new PointSet(new float[] {  6, 4,   1, 5,   0, 6,   0, 7,   1, 8,   5, 8,   6, 7,   6, 1,   5, 0,   1, 0,   0, 1 })
        }),
        new Character(':', new PointSet[] {
            new PointSet(new float[] {  3, 1,   2, 2,   3, 3,   4, 2 }, true),
            new PointSet(new float[] {  3, 5,   2, 6,   3, 7,   4, 6 }, true)
        }),
        new Character(';', new PointSet[] {
            new PointSet(new float[] {  3, 5,   2, 6,   3, 7,   4, 6 }, true),
            new PointSet(new float[] {  2, 3,   4, 3,   4, 2,   3, 1,   3, 2,   2, 2 }, true)
        }),
        new Character('<', new PointSet[] {
            new PointSet(new float[] {  4, 2,   2, 4,   4, 6 })
        }),
        new Character('=', new PointSet[] {
            new PointSet(new float[] {  1, 3,   5, 3 }),
            new PointSet(new float[] {  1, 5,   5, 5 })
        }),
        new Character('>', new PointSet[] {
            new PointSet(new float[] {  2, 2,   4, 4,   2, 6 })
        }),
        new Character('?', new PointSet[] {
            new PointSet(new float[] {  3, 0,   2, 1,   3, 2,   4, 1 }, true),
            new PointSet(new float[] {  3, 3,   3, 4,   5, 6,   5, 7,   4, 8,   2, 8,   1, 7 })
        }),
        new Character('@', new PointSet[] {
            new PointSet(new float[] {  4, 3,   4, 5,   2, 5,   1, 4,   1, 2,   3, 2,   4, 3,   5, 2,   6, 2,   6, 6,   4, 8,   2, 8,   0, 6,   0, 2,   2, 0,   4, 0,   5, 1 })
        }),
        new Character('A', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 6,   2, 8,   4, 8,   6, 6,   6, 5,   6, 0 }),
            new PointSet(new float[] {  0, 5,   6, 5 })
        }),
        new Character('B', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8,   4, 8,   5, 7,   5, 6,   4, 5,   6, 3,   6, 2,   4, 0 }, true),
            new PointSet(new float[] {  0, 5,   4, 5 })
        }),
        new Character('C', new PointSet[] {
            new PointSet(new float[] {  6, 6,   4, 8,   2, 8,   0, 6,   0, 2,   2, 0,   4, 0,   6, 2 })
        }),
        new Character('D', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8,   4, 8,   6, 6,   6, 2,   4, 0 }, true)
        }),
        new Character('E', new PointSet[] {
            new PointSet(new float[] {  6, 0,   0, 0,   0, 8,   6, 8 }),
            new PointSet(new float[] {  0, 5,   5, 5 })
        }),
        new Character('F', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8,   6, 8 }),
            new PointSet(new float[] {  0, 5,   5, 5 })
        }),
        new Character('G', new PointSet[] {
            new PointSet(new float[] {  6, 6,   4, 8,   2, 8,   0, 6,   0, 2,   2, 0,   4, 0,   6, 2,   6, 5,   2, 5 })
        }),
        new Character('H', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8 }),
            new PointSet(new float[] {  6, 0,   6, 8 }),
            new PointSet(new float[] {  0, 5,   6, 5 })
        }),
        new Character('I', new PointSet[] {
            new PointSet(new float[] {  0, 8,   6, 8 }),
            new PointSet(new float[] {  0, 0,   6, 0 }),
            new PointSet(new float[] {  3, 8,   3, 0 })
        }),
        new Character('J', new PointSet[] {
            new PointSet(new float[] {  2, 8,   6, 8,   6, 2,   4, 0,   2, 0,   0, 2 })
        }),
        new Character('K', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8 }),
            new PointSet(new float[] {  0, 2,   6, 8 }),
            new PointSet(new float[] {  2, 4,   6, 0 })
        }),
        new Character('L', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 0,   6, 0 })
        }),
        new Character('M', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8,   3, 5,   6, 8,   6, 0 })
        }),
        new Character('N', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 8,   6, 2 }),
            new PointSet(new float[] {  6, 0,   6, 8 })
        }),
        new Character('O', new PointSet[] {
            new PointSet(new float[] {  6, 6,   4, 8,   2, 8,   0, 6,   0, 2,   2, 0,   4, 0,   6, 2 }, true)
        }),
        new Character('P', new PointSet[] {
            new PointSet(new float[] {  0, 5,   5, 5,   6, 6,   6, 7,   5, 8,   0, 8,   0, 0 })
        }),
        new Character('Q', new PointSet[] {
            new PointSet(new float[] {  6, 6,   4, 8,   2, 8,   0, 6,   0, 2,   2, 0,   4, 0,   6, 2 }, true),
            new PointSet(new float[] {  4, 2,   6, 0 })
        }),
        new Character('R', new PointSet[] {
            new PointSet(new float[] {  0, 5,   5, 5,   6, 6,   6, 7,   5, 8,   0, 8,   0, 0 }),
            new PointSet(new float[] {  1, 5,   6, 0 })
        }),
        new Character('S', new PointSet[] {
            new PointSet(new float[] {  6, 7,   5, 8,   1, 8,   0, 7,   0, 6,   1, 5,   5, 5,   6, 4,   6, 2,   4, 0,   2, 0,   0, 2 })
        }),
        new Character('T', new PointSet[] {
            new PointSet(new float[] {  0, 8,   6, 8 }),
            new PointSet(new float[] {  3, 8,   3, 0 })
        }),
        new Character('U', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 2,   2, 0,   4, 0,   6, 2,   6, 8 })
        }),
        new Character('V', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 3,   3, 0,   6, 3,   6, 8 })
        }),
        new Character('W', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 0,   3, 3,   6, 0,   6, 8 })
        }),
        new Character('X', new PointSet[] {
            new PointSet(new float[] {  0, 0,   0, 1,   6, 7,   6, 8 }),
            new PointSet(new float[] {  0, 8,   0, 7,   6, 1,   6, 0 })
        }),
        new Character('Y', new PointSet[] {
            new PointSet(new float[] {  0, 8,   3, 5,   6, 8 }),
            new PointSet(new float[] {  3, 5,   3, 0 })
        }),
        new Character('Z', new PointSet[] {
            new PointSet(new float[] {  0, 8,   6, 8,   6, 7,   0, 1,   0, 0,   6, 0 })
        }),
        new Character('[', new PointSet[] {
            new PointSet(new float[] {  4, 0,   2, 0,   2, 8,   4, 8 })
        }),
        new Character('\\', new PointSet[] {
            new PointSet(new float[] {  0, 8,   0, 7,   6, 1,   6, 0 })
        }),
        new Character(']', new PointSet[] {
            new PointSet(new float[] {  2, 0,   4, 0,   4, 8,   2, 8 })
        }),
        new Character('^', new PointSet[] {
            new PointSet(new float[] {  1, 6,   3, 8,   5, 6 })
        }),
        new Character('_', new PointSet[] {
            new PointSet(new float[] {  0, 0,   6, 0 })
        })
    };
}
