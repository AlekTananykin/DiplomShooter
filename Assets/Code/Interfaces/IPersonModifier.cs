using Assets.Code.Models;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Interfaces
{
    interface IPersonModifier
    {
        bool Modify(Thing thing, PersonModel personModel);
    }
}
