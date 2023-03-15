using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Controllers
{
    internal class ControllersAndModelControllers: ControllersStorage
    {
        IDictionary<int, IModelController> _modelControllers;

        internal ControllersAndModelControllers()
        {
            _modelControllers = new Dictionary<int, IModelController>();
        }

        public new void Add(IInteractionObject interactionObject)
        {
            base.Add(interactionObject);
            if (interactionObject is IModelController modelController)
            {
                _modelControllers.Add(modelController.Id, modelController);
            }
        }

        public IModelController GetModelController(int modelId)
        {
            if (_modelControllers.TryGetValue(
                modelId, out IModelController controller))
                return controller;

            throw new GameException(
                "ControllersStorage.GetModelController: can't find ModelId");
        }

        public void PreSafe()
        {
            foreach (KeyValuePair<int, IModelController> pair in _modelControllers)
            {
                pair.Value?.PreSafe();
            }
         }
    }
}
