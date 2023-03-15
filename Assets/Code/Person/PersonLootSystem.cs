using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using Assets.Code.Models;
using Assets.Code.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Person
{
    internal class PersonLootSystem
    {
        public GameObject View;
        public PersonModel Model;

        private const float _takeDistance = 4.0f;

        public void TakeLoot(Vector3 targetPoint)
        {
            Ray ray = new Ray(View.transform.position,
                (targetPoint - View.transform.position).normalized);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo))
                return;

            if (hitInfo.distance > _takeDistance)
                return;

            if (hitInfo.collider.gameObject.TryGetComponent(out IStorage bag))
            {
                IList<Thing> bagItems = bag.GetItems();
                foreach (var item in bagItems)
                    AddLoot(item);

                bagItems.Clear();
            }
        }

        private void AddLoot(Thing thing)
        {
            if (Model.GetBagItems().ContainsKey(thing.Name))
                Model.GetBagItems()[thing.Name].Add(thing);
            else
                Model.GetBagItems().Add(thing.Name, new List<Thing>() {thing});

            Taken_Thing?.Invoke(thing);

            Debug.Log("Loot: " + thing.Name);
        }

        internal Action<Thing> Taken_Thing;
    }
}
