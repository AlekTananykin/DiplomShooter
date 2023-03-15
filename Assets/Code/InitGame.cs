using Assets.Code.Auxiliary;
using Assets.Code.Controllers;
using Assets.Code.Exceptions;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.PlayerInput;
using Assets.Code.Views;
using System;
using UnityEngine;

namespace Assets.Code
{
    sealed class InitGame
    {
        public InitGame(ControllersAndModelControllers controllers, GameModel model,
            IPlayerInput playerInput, string saveGamePath)
        {
            InitSaveLoadGameController(playerInput, model, 
                controllers, saveGamePath);

            ViewsFabric viewsFabric = new ViewsFabric();

            IActionSystem actionsController = InitActions(controllers);

            HeroController hero = InitializeCameraAndPlayer(viewsFabric,
                controllers, model, playerInput, actionsController);

            IPlayerPanel playerPanel = InitPlayerPanel(viewsFabric, controllers, model);

            InitializeChests(controllers, model);
            InitializePirates(controllers, model, actionsController, hero, playerPanel);
        }

        private void InitSaveLoadGameController(IPlayerInput playerInput, 
            GameModel model, ControllersAndModelControllers controllers, string saveGamePath)
        {
            var saveLoadController = new SaveLoadGameController<GameModel>(
                playerInput, model, controllers, saveGamePath);
            controllers.Add(saveLoadController);
        }

        IActionSystem InitActions(ControllersAndModelControllers controllers)
        {
            IActionSystem actionSystem = new ActionsController();
            if (!(actionSystem is IInteractionObject))
                throw new GameException(
                    "InitGame.InitActions: ActionsController is not IAttackSystem");

            controllers.Add(actionSystem);

            actionSystem.Add(LootName.gun, new ShootoutController<BombViewFabric>());
            actionSystem.Add(LootName.cutlass, new PrickAttackController());

            return actionSystem;
        }

        private HeroController InitializeCameraAndPlayer(
            ViewsFabric viewsFabric,
            ControllersAndModelControllers controllers, GameModel model,
            IPlayerInput playerInput,
            IActionSystem actionSysterm)
        {
            GameObject heroView = viewsFabric.CreateAnastasia();
            heroView.transform.position = model.Hero.InitPosition;
            var hero = new HeroController(model.Hero, heroView, actionSysterm);
            
            GameObject cameraView = viewsFabric.CreateCamera();
            cameraView.transform.position = model.Camera.InitPosition;
            var camera = new CameraController(model.Camera, cameraView);

            GameObject zondView = viewsFabric.CreateZond();
            GameObject frontCameraView = viewsFabric.CreateFrontCamera();
            var zondController = new ZondController(frontCameraView, zondView, heroView);
            
            var player = new PlayerController(playerInput, cameraView, hero);

            player.Go_To_Point += camera.AddNewTargetPosition;
            player.Current_Point += zondController.SetPosition;
            
            controllers.Add(player);
            controllers.Add(hero);
            controllers.Add(camera);
            controllers.Add(zondController);

            return hero;
        }

        private void InitializePirates(
            ControllersAndModelControllers controllers, GameModel model,
            IActionSystem actionSystem,
            HeroController hero,
            IPlayerPanel playerPanel)
        {
            PiratesViewFabric viewFabric = new PiratesViewFabric();
            for (int i = 0; i < model.Pirates.Length; ++i)
            {
                var pirate = new PirateController(
                model.Pirates[i], viewFabric.CreateGameObject(), actionSystem, hero);
                pirate.SelectAction(0);

                pirate.IsKilled += playerPanel.MessageReceiver;

                controllers.Add(pirate);
            }
        }

        private void InitializeChests(
            ControllersAndModelControllers controllers, GameModel model)
        {
            ChestViewFabric viewFabric = new ChestViewFabric();
            for (int i = 0; i < model.Chests.Length; ++i)
            {
                InitializeSingleChest(model.Chests[i],
                    viewFabric.CreateGameObject(), controllers);
            }
        }

        private void InitializeSingleChest(ChestModel chestModel, 
            GameObject view, ControllersAndModelControllers controllers)
        {
            var chest = new ChestController(chestModel, view);
            controllers.Add(chest);
        }

        private IPlayerPanel InitPlayerPanel(
            ViewsFabric viewsFabric,
            ControllersAndModelControllers controllers, 
            GameModel model)
        {
            GameObject pannelView = viewsFabric.CreatePlayerPanel();
            
            var pannelController = new PlayerPanelController(pannelView);
            controllers.Add(pannelController);

            return pannelController;
        }
    }
}
