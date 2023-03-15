using Assets.Code.Auxiliary;
using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    internal abstract class PersonController : 
        IExecute, IPersonController, IModelController, ICleanup
    {
        protected PersonModel _model;

        protected GameObject _view;

        private float _ceenterY;

        PersonScript _script;
        private IPersonMover _personMover;
        private PersonLootSystem _lootSystem;
        private PersonActionSystem _actionSystem;

        private PersonSubjectSystem _subjectSystem;

        public PersonController(PersonModel model, GameObject view, 
            IActionSystem actionSystem)
        {
            _model = model;
            _view = view;

            _lootSystem = new PersonLootSystem() {View =  _view, Model = _model };
            _actionSystem = new PersonActionSystem(actionSystem) 
                { View = _view, Model = _model };

            _subjectSystem = new PersonSubjectSystem(_model);

            _lootSystem.Taken_Thing += _subjectSystem.ModifyPerson;
        }

        internal void Initialize(ILeash leash, float ceenterY)
        {
            if (null == _model || null == _view)
                throw new Exception("PersonController is not ready. ");

            _view.transform.position = _model.InitPosition;

            _personMover = new PersonMover(leash, _model.RotationSpeed);
            _ceenterY = ceenterY;

            _script = _view.AddComponent<PersonScript>();
            _script.Damage += ReactToHit;

        }

        private void ReactToHit(uint damage)
        {
            _model.Health -= (int)damage;
            if (_model.Health <= 0)
            {
                _model.Health = 0;
                Debug.Log(_model.ModelId + " is killed. ");

                _model.InitPosition = new Vector3(-100, -100, -100);
                _view.transform.position = _model.InitPosition;
                _view.SetActive(false);
                IsKilled?.Invoke(_model.ModelId.ToString());
            }
        }

        public virtual void Execute(float deltaTime)
        {
            Execute(deltaTime, _model.Speed);
        }

        protected virtual void Execute(float deltaTime, float speed)
        {
            if (!_personMover.IsNeedMove || 0 == _model.Health)
                return;

            Vector3 direction;
            (_view.transform.position, direction) =
                _personMover.Execute(deltaTime, speed);

            if (direction.sqrMagnitude > 0)
                _view.transform.forward = direction;
        }


        protected void AddNewTargetPoint(Vector3 position)
        {
            Vector3 heroNewPosition = new Vector3(position.x,
                position.y + _ceenterY, position.z);

            _personMover.AddPoint(heroNewPosition);
        }

        protected virtual void HitToPoint(Vector3 targetPoint)
        {
            _actionSystem.ActionToPoint(targetPoint);
        }

        protected void SelectAction(int actionNumber)
        {
            _actionSystem.SelectAction(actionNumber);
        }

        protected void TakeLoot(Vector3 targetPoint)
        {
            _lootSystem.TakeLoot(targetPoint);
        }

        public Vector3 Position => _view.transform.position;

        public int Id => _model.ModelId;

        public void SetModel(IModel model)
        {
            if (model is PersonModel)
            {
                _model = model as PersonModel;
                _view.transform.position = _model.InitPosition;
            }
            else throw new GameException(
                "PersonController.SetModel: model as not PersonModel. ");
        }

        public void PreSafe()
        {
            _model.InitPosition = _view.transform.position;
        }

        public void Cleanup()
        {
           _lootSystem.Taken_Thing -= _subjectSystem.ModifyPerson;
        }

        public Action<string> IsKilled;
        
    }
}
