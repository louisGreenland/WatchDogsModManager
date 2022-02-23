using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDogsModManager
{
    class Mod
    {
        public string modName = "Mod";
        public string modPath = "";
        public bool enabled;

        public override string ToString() {
            return modName;
        }
    }    
}
