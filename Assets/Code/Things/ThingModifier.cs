using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Things
{
    internal static class ThingModifier
    {
        public static bool AddModification(
            this Thing target, Thing modification)
        {
            if (target.Name != modification.Target ||
                target.Components.ContainsKey(modification.Name))
                return false;
            target.Cost += modification.Cost;
            var keys = modification.Properties.Keys;
            foreach (LootProperties name in keys)
            {
                target.Properties[name] = target.Properties[name] + 
                    modification.Properties[name];
            }

            target.Components.Add(modification.Name, modification);
            return true;
        }

        public static Thing RemoveModification(
            this Thing target, LootName modificationName)
        {
            if (!target.Components.TryGetValue(
                modificationName, out Thing modification))
                return null;

            target.Components.Remove(modificationName);
            var modificationKeys = modification.Properties.Keys;
            target.Cost -= modification.Cost;

            foreach (LootProperties name in modificationKeys)
            {
                target.Properties[name] = target.Properties[name] -
                    modification.Properties[name];
            }
            return modification;
        }

        public static Thing RemoveModification(
           this Thing target, Thing modification)
        {
            return target.RemoveModification(modification.Name);
        }
    }
}
