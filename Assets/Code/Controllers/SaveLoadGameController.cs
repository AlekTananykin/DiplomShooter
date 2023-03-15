using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.PlayerInput;
using Assets.Code.SaveLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Controllers
{
    internal sealed class SaveLoadGameController<GModel> : IExecute
    {
        private IPlayerInput _playerInput;
        private GModel _model;
        private ControllersAndModelControllers _controllers;
        private IGameSaver<GModel> _gameSaver;

        internal SaveLoadGameController(
            IPlayerInput playerInput, GModel gameModel, 
            ControllersStorage controllersStorage, string savedGamesPath)
        {
            _playerInput = playerInput;
            _model = gameModel;

            if (!(controllersStorage is ControllersAndModelControllers))
                throw new GameException("SaveLoadGameController: " +
                    "controllersStorage is not ControllersAndModelControllers");

            _controllers = controllersStorage as ControllersAndModelControllers;

            _gameSaver = new GameSaver<GModel>(savedGamesPath);
        }

        public void Execute(float deltaTime)
        {
            if (_playerInput.IsSave())
            {
                _controllers.PreSafe();
                _gameSaver.Save(_model);
            }
            if (_playerInput.IsLoad())
            {
                IList<string> filenames = _gameSaver.GetSaveList();

                _gameSaver.Load(filenames.Count - 1, ref _model);
                ReloadModelControllers(_model, _controllers);
            }
        }

        void ReloadModelControllers(GModel gameModel,
            ControllersStorage controllers)
        {
            Type type = gameModel.GetType();
            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; ++i)
            {
                object modelFieldValue = fields[i].GetValue(gameModel);

                if (modelFieldValue is IModel[])
                    LoadArrayModels(modelFieldValue as IModel[], controllers);
                else if (modelFieldValue is IModel)
                    LoadModel(modelFieldValue as IModel, controllers);
                else throw new GameException();
            }
        }

        private void LoadArrayModels(IModel[] modelArray,
            ControllersStorage controllers)
        {
            for (int i = 0; i < modelArray.Length; ++i)
                LoadModel(modelArray[i], controllers);
        }

        private void LoadModel(IModel model, ControllersStorage controllers)
        {
            IModelController controller =
                _controllers.GetModelController(model.ModelId);
            controller.SetModel(model);
        }
    }
}
