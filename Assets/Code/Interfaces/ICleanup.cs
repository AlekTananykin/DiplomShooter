﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Interfaces
{
    interface ICleanup: IInteractionObject
    {
        void Cleanup();
    }
}
