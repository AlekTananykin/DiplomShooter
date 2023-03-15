using Assets.Code.Things;
using UnityEngine;

namespace Assets.Code.Interfaces
{
    interface IAction: IInteractionObject
    {
        bool Attack(Vector3 place, Vector3 targetPoint, Thing attackThing);
    }
}
