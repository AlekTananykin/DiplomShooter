using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Person.States;
using UnityEngine;

namespace Assets.Code.Controllers
{
    class StatePersonController : PersonController
    {
        private IPersonState[] _states;
        protected enum PesonStateCode { crawl = 1, run = 2, sit = 3, walk = 4 };
        protected PesonStateCode CurrentState { get; set; }

        public StatePersonController(PersonModel model, GameObject view,
            IActionSystem actionSystem)
            :base(model, view, actionSystem)
        {
            CurrentState = PesonStateCode.walk;
            _states = new IPersonState[] { new ToCrawl(),
                new ToRun(), new ToSit(), new ToWalk()};
        }

        public override void Execute(float deltaTime)
        {
            base.Execute(
                deltaTime, _states[(int)CurrentState].Speed);
        }

        protected override void HitToPoint(Vector3 targetPoint)
        {
            if (_states[(int)CurrentState].IsAttackEnable)
                base.HitToPoint(targetPoint);
        }

    }
}
