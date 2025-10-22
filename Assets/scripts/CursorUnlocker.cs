using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echoes_At_The_Last_Station
{
    public class CursorUnlocker : MonoBehaviour
    {
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            // ќпционально Ч проверка на ESC, чтобы вернуть курсор
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}

