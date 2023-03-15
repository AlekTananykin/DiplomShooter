using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Person.Modifiers
{
    internal class ModifySpeed : IPersonModifier
    {
        public bool Modify(Thing thing, PersonModel personModel)
        {
            if (LootName.jackboots != thing.Name)
                return false;

            personModel.Speed += 3f;

            return true;
        }
    }
}