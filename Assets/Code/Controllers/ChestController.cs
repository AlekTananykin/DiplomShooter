using Assets.Code.Auxiliary;
using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    internal sealed class ChestController : IInitialization, ICleanup, 
        IModelController
    {
        private ChestModel _model;

        public void SetModel(IModel model)
        {
            if (model is ChestModel)
            {
                _model = model as ChestModel;
                _view.transform.position = _model.Position;
            }
            else throw new GameException(
                "ChestController.SetModel: model as not ChestModel. ");

        }

        public void PreSafe()
        {
            _model.Position = _view.transform.position;
        }

        public int Id => _model.ModelId;

        private GameObject _view;

        public ChestController(ChestModel model, GameObject view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            BagScript storageScript = _view.AddComponent<BagScript>();
            storageScript.Get_Storage += GetThings;

            _view.transform.position = _model.Position;
        }

        public IList<Thing> GetThings()
        {
            return _model.Items;
        }

        public void Cleanup()
        {
            //BagScript storageScript = _view.GetComponent<BagScript>();
            //if (null != storageScript?.Get_Storage)
            //    storageScript.Get_Storage -= GetThings;
        }
    }
}
