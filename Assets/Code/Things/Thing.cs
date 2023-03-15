using Assets.Code.Auxiliary;
using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Things
{
    [Serializable]
    public sealed class Thing
    { 
        public LootName Name { get; set; }
        public int Cost { get; set; }
        public LootName Target { get; set; }
        public IDictionary<LootProperties, int> Properties{get;}
        public IDictionary<LootName, Thing> Components { get; }

        internal Thing()
        {
            Properties = new Dictionary<LootProperties, int>();
            Components = new Dictionary<LootName, Thing>();
        }

        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }
    }
}
