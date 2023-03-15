using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.PlayerInput
{
    interface IPlayerInput
    {
        float MoveX { get; }
        float MoveY { get; }

        bool GetClickPosition(ref Vector3 position);

        void GetMousePosition(ref Vector3 position);
        bool IsLeftMouseClicked();

        bool IsSave();
        bool IsLoad();
    }
}
