using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.PlayerInput;
using Assets.Code.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    delegate void SelectPoint(Vector3 point);

    internal sealed class PlayerController : IExecute, IInitialization
    {
        private IPlayerInput _input;
        Camera _camera;
        GameObjectsPool<PathMarksFabric> _pathMarkPool;
        const uint _marksPoolSize = 5;
        IManagedPerson _managedPerson;
        
        internal PlayerController(
            IPlayerInput input, GameObject camera, IManagedPerson managedPerson)
        {
            _managedPerson = managedPerson;
            _input = input;
            _camera = camera.GetComponent<Camera>();
            _pathMarkPool = new GameObjectsPool<PathMarksFabric>(
                new PathMarksFabric(), _marksPoolSize);
        }

        public void Initialize()
        {
            if (null == _input || null == _camera)
                throw new GameException(
                    "PlayerController: input is absent. ");
        }

        public void Execute(float deltaTime)
        {
            Vector3 position = new Vector3();
            _input.GetMousePosition(ref position);

            Ray ray = _camera.ScreenPointToRay(position);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            if (_input.IsLeftMouseClicked())
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                    _managedPerson.HitToPoint(hit.point);
                else if (hit.collider.gameObject.CompareTag("Loot"))
                    _managedPerson.TakeLoot(hit.point);
                else
                    GoToPoint(hit.point);
            }

            Current_Point?.Invoke(hit.point);
        }


        void GoToPoint(Vector3 point)
        {
            GameObject pathMark = _pathMarkPool.Create();
            pathMark.transform.position = point;
            PathMarkScript script = pathMark.GetComponent<PathMarkScript>();
            if (null == script)
                throw new GameException("PathMarkScript is not attached. ");

            script.OnPathMatkCall = OnPathMarkTrigger;

            _managedPerson.GoToPoint(point);
            Go_To_Point?.Invoke(point);
        }

        public SelectPoint Go_To_Point;
        public SelectPoint Current_Point;

        public void OnPathMarkTrigger(GameObject pathObj)
        {
            PathMarkScript script = pathObj.GetComponent<PathMarkScript>();
            script.OnPathMatkCall -= OnPathMarkTrigger;
            _pathMarkPool.Intake(ref pathObj);
        }
    }
}
