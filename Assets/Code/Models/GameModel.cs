using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    internal sealed class GameModel
    {
        public CameraModel Camera;
        public PersonModel Hero;
        public PersonModel[] Pirates;
        public ChestModel[] Chests;
    }
}
