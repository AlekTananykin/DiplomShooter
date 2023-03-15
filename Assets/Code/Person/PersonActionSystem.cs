using Assets.Code.Auxiliary;
using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Person
{
    internal sealed class PersonActionSystem
    {
        internal GameObject View;
        internal PersonModel Model;
        private int _selectedAction = 0;
        private IActionSystem _actionSystem;

        public PersonActionSystem(IActionSystem actionStorage)
        {
            _actionSystem = actionStorage;
        }

        public void ActionToPoint(Vector3 targetPoint)
        {
            ChechActionNumber(_selectedAction);
            _actionSystem.Attack(View.transform.position + Vector3.up * 2, 
                targetPoint, Model.AppliedItems[_selectedAction]);
        }

        public void SelectAction(int actionNumber)
        {
            ChechActionNumber(actionNumber);
            _selectedAction = actionNumber;
        }

        private void ChechActionNumber(int number)
        {
            if (number >= Model.AppliedItems.Count ||
               0 == Model.AppliedItems.Count)
            {
                throw new GameException("PersonActionSystem.SelectedAction: " +
                    "Model.AppliedItems is empty or selectedAction is greater " +
                    "then appliedItemsCount. ");
            }
        }
    }
}
