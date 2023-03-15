using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.PlayerInput
{
    sealed class PlayerPcInput : IPlayerInput
    {
        public float MoveX => Input.GetAxis("Mouse X");
        public float MoveY => Input.GetAxis("Mouse Y");
        

        public bool GetClickPosition(ref Vector3 position)
        {
            if (!Input.GetMouseButtonUp(0))
                return false;

            position = Input.mousePosition;
            return true;
        }

        public void GetMousePosition(ref Vector3 position)
        {
            position = Input.mousePosition;
        }

        public bool IsLeftMouseClicked()
        {
            return Input.GetMouseButtonUp(0);
        }

        public bool IsLoad()
        {
            return Input.GetKeyDown(KeyCode.L);
        }

        public bool IsSave()
        {
            return Input.GetKeyDown(KeyCode.S);
        }
    }
}
