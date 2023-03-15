using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.SaveLoad
{
    internal interface IGameSaver<T>
    {
        void Save(T dataToSave);
        void Load(int fileIndex, ref T loadedData);
        IList<string> GetSaveList();
    }
}
