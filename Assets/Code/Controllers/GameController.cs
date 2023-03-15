using Assets.Code.Exceptions;
using Assets.Code.Models;
using Assets.Code.PlayerInput;
using Assets.Code.SaveLoad;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Controllers
{
    public sealed class GameController : MonoBehaviour
    {
        private ControllersAndModelControllers _controllersStorage;

        private IPlayerInput _playerInput;
        private GameModelFabric _gameModelFabric;
        
        private const string _savedGamesPath = "SavedGames";
        private uint _piratesCount = 1200;

        void Start()
        {
            try
            {
                _gameModelFabric = new GameModelFabric();
                GameModel gameModel = 
                    _gameModelFabric.InitGameModel(_piratesCount);

                _controllersStorage = new ControllersAndModelControllers();
                _playerInput = new PlayerPcInput();
                new InitGame(_controllersStorage, gameModel,
                    _playerInput, _savedGamesPath);

                _controllersStorage.Initialize();
            }
            catch (GameException ex)
            {
                Debug.LogError(ex.Message);
                Application.Quit();
            }
        }

        void Update()
        {
            _controllersStorage.Execute(Time.deltaTime);
        }
        private void LateUpdate()
        {
            _controllersStorage.LateExecute(Time.deltaTime);
        }

        public void OnDestroy()
        {
            _controllersStorage.Cleanup();
        }

 
    }
}