using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    internal abstract class ModelBase: IModel
    {
        public int ModelId { get; set; }
        private static int _modelId = 0;

        internal ModelBase()
        {
            ModelId = ++ _modelId;
        }
    }
}
