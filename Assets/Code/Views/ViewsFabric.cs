using Assets.Code.Exceptions;
using UnityEngine;

namespace Assets.Code.Views
{
    internal class ViewsFabric: GameObjectsLoader
    {
        private GameObject _hero;
        internal GameObject CreateHero()
        {
            if (null == _hero)
            {
                GameObject heroPrefab = 
                    (GameObject)Resources.Load("Prefabs/Heroes/Hero");
                if (null == heroPrefab)
                {
                    throw new GameException(
                        "CreateHero: Hero prefab is not found. ");
                }
                _hero = (GameObject)GameObject.Instantiate(heroPrefab);
            }
            return _hero;
        }

        internal GameObject CreateAnastasia()
        {
            if (null == _hero)
            {
                GameObject heroPrefab =
                    (GameObject)Resources.Load("Prefabs/Heroes/Anastasiya");
                if (null == heroPrefab)
                {
                    throw new GameException(
                        "CreateHero: Hero prefab is not found. ");
                }
                _hero = (GameObject)GameObject.Instantiate(heroPrefab);
            }
            return _hero;
        }

        private GameObject _camera;
        internal GameObject CreateCamera()
        {
            if (null == _camera)
            {
                GameObject cameraPrefab = 
                    (GameObject)Resources.Load("Prefabs/Player/Main Camera");
                if (null == cameraPrefab)
                {
                    throw new GameException(
                        "CreateCamera: Camera prefab is not found. ");
                }
                _camera = GameObject.Instantiate(cameraPrefab);
            }
            return _camera;
        }

        private GameObject _frontCamera;
        internal GameObject CreateFrontCamera()
        {
            if (null == _frontCamera)
            {
                GameObject cameraPrefab = 
                    (GameObject)Resources.Load("Prefabs/PlayerZond/FrontCamera");
                if (null == cameraPrefab)
                    throw new GameException("Create Front Camera");
                _frontCamera = GameObject.Instantiate(cameraPrefab);
            }
            return _frontCamera;
        }

        private GameObject _zond;
        internal GameObject CreateZond()
        {
            if (null == _zond)
            {
                GameObject zondPrefab =
                    (GameObject)Resources.Load("Prefabs/PlayerZond/PlayerZond");
                if (null == zondPrefab)
                    throw new GameException("Create Player zond");
                _zond = GameObject.Instantiate(zondPrefab);
            }
            return _zond;
        }

        private GameObject _playerPanel;
        internal GameObject CreatePlayerPanel()
        {
            if (null == _playerPanel)
            {
                GameObject pannelPrefab =
                    (GameObject)Resources.Load("Prefabs/Player/Canvas");
                if (null == pannelPrefab)
                    throw new GameException("Create Player Panel(Canvas)");
                _playerPanel = GameObject.Instantiate(pannelPrefab);
            }
            return _playerPanel;
        }
    }
}
