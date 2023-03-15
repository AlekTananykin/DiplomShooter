using Assets.Code.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Interfaces
{
    interface IActionSystem: IAction
    {
        void Add(LootName name, IInteractionObject system);
    }
}
