using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Models
{
    internal sealed class GameModelFabric
    {
        private System.Random _rand = new System.Random();
        private const float _minDistance = 10f;
        private const int _piratesRange = 1000;
        private IThingsFabric _thingsFabric = new ThingsRandFabric();
        private const int _maxThingsCount = 3;

        internal GameModel InitGameModel(uint piratesNum)
        {
            GameModel model = new GameModel();
            model.Hero = CreateHeroModel();
            model.Camera = CreateCameraModel(model.Hero.InitPosition);
            
            model.Chests = new ChestModel[piratesNum];
            for (int i = 0; i < piratesNum; ++i)
            {
                model.Chests[i] = CreateChestModel(
                    GetRandChestPos(model.Hero.InitPosition,
                    model.Chests, i, _minDistance));
            }

            model.Pirates = new PersonModel[piratesNum];
            for (int i = 0; i < piratesNum; ++i)
            {
                model.Pirates[i] = CreatePirateModel(
                    model.Chests[i].Position, _minDistance);
            }

            return model;
        }

        private ChestModel CreateChestModel(Vector3 position)
        {
            return new ChestModel()
            {
                Position = position,
                Items = _thingsFabric.CreateThings(
                    _rand.Next(_maxThingsCount))
            };
        }

        private Vector3 GetRandChestPos(Vector3 heroInitPosition, 
            ChestModel[] chests, int count, float minDistance)
        {
            const int matTryCount = 15;
            Vector3 pos = new Vector3();
            for (int i = 0; i < matTryCount; ++i)
            {
                pos = new Vector3(_rand.Next(0, _piratesRange),
                    heroInitPosition.y, _rand.Next(0, _piratesRange));

                if ((heroInitPosition - pos).magnitude < minDistance)
                    continue;

                if (CheckChestPosition(pos, chests, count, minDistance))
                    break;
            }
            return pos;
        }

        private bool CheckChestPosition(Vector3 pos, ChestModel[] chests, 
            int count, float minDistance)
        {
            for (int j = 0; j < count; ++j)
            {
                if ((chests[j].Position - pos).magnitude < minDistance)
                    return false;
            }
            return true;
        }


        private PersonModel CreateHeroModel()
        {
            var model = new PersonModel();
            model.Health = 100;
            model.MaxHealth = 100;
            model.InitPosition = new Vector3(10, 0, 10);
            model.Skill = 20;
            model.Speed = 5;
            model.AppliedItems.Add(new Thing() 
            { 
                Name = LootName.gun, Cost = 10, Target = LootName.none 
            });
            return model;
        }

        private CameraModel CreateCameraModel(Vector3 xzPosition)
        {
            var model = new CameraModel();
            model.Forward = Vector3.down;
            model.Height = 10f;
            model.InitPosition = new Vector3(
                xzPosition.x,
                model.Height, xzPosition.z);
            model.Speed = 3f;
            return model;
        }

        private PersonModel CreatePirateModel(
            Vector3 chestPosition, float distance)
        {
            var model = new PersonModel();
            
            model.MaxHealth = 100;
            model.Health = model.MaxHealth;

            model.InitPosition = chestPosition + 
                Vector3.ClampMagnitude(new Vector3(
                    (float)_rand.NextDouble() * distance,
                    0,
                    (float)_rand.NextDouble() * distance),
                    distance);

            model.AppliedItems.Add(new Thing()
            {
                Name = LootName.gun,
                Cost = 10,
                Target = LootName.none
            });

            model.Skill = 19;
            model.Speed = 7f;
            model.RotationSpeed = 1f;

            return model;
        }
    }
}
