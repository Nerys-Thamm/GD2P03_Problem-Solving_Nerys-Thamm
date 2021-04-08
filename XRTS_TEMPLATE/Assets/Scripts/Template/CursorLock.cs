using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool m_CursorLocked = true;

    private void LockCursor()
    {
        //Cursor.lockState = m_CursorLocked ? CursorLockMode.Confined : CursorLockMode.None;
        Cursor.visible = !m_CursorLocked;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_CursorLocked = !m_CursorLocked;
            LockCursor();
        }
    }
}
