using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Person.Modifiers;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Person
{
    internal sealed class PersonSubjectSystem
    {
        IList<IPersonModifier> _modifiers;

        PersonModel _personModel;
        internal PersonSubjectSystem(PersonModel personModel)
        {
            _personModel = personModel;
            _modifiers = new List<IPersonModifier>();
            _modifiers.Add(new ModifyLoadCapacity());
            _modifiers.Add(new ModifyMaxHealth());
            _modifiers.Add(new ModifyMaxSkill());
            _modifiers.Add(new ModifySpeed());
        }

        internal void ModifyPerson(Thing thing)
        {
            for (int i = 0; i < _modifiers.Count; ++i)
            {
                if (_modifiers[i].Modify(thing, _personModel))
                    break;
            }
        }

    }
}
