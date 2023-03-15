using Assets.Code.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.SaveLoad
{
    internal sealed class GameSaver<T>: IGameSaver<T>
    {
        private string _path;
        IList<string> _filenames;

        private const string _nameFormat = "yyyy-MM-dd-HH-MM-ss";

        internal GameSaver(string pathToSave)
        {
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.AppendFormat("{0}\\{1}", Directory.GetCurrentDirectory(), pathToSave);

            if (0 != pathToSave.Length && '\\' != pathToSave[pathToSave.Length - 1])
                pathBuilder.Append("\\");
            
            _path = pathBuilder.ToString();

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
                if (!Directory.Exists(_path))
                    throw new GameException("GameSave.GameSaver: wron filepath. ");
            }

            _filenames = new List<string>();
        }

        public void Save(T dataToSave)
        {
            string serializedData = JsonConvert.SerializeObject(dataToSave);
            string filename = GetFileName();
            File.WriteAllText(filename , serializedData);
        }

        private string GetFileName()
        {
            StringBuilder filename = new StringBuilder(_path);

            DateTime time = DateTime.Now;
            filename.Append(time.ToString(_nameFormat));
            return filename.ToString();
        }

        public void Load(int fileIndex, ref T loadedData)
        {
            if (fileIndex > _filenames.Count)
                throw new GameException("GameSaver.Load: index is out of range. ");

            string filename = string.Format("{0}{1}",_path, _filenames[fileIndex] );

            string json = File.ReadAllText(filename);

            loadedData = JsonConvert.DeserializeObject<T>(json);
        }

        public IList<string> GetSaveList()
        {
            _filenames = Directory.GetFiles(_path).ToList();

            for (int i = 0; i < _filenames.Count; ++ i)
                _filenames[i] = Path.GetFileName(_filenames[i]);

            return new List<string>(_filenames);
        }
    }
}
