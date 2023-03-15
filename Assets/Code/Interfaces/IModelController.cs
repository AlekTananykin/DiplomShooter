using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Interfaces
{
    interface IModelController
    {
        void SetModel(IModel model);
        void PreSafe();
        int Id { get; }
    }
}
