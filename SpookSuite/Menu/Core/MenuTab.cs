using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Menu.Core
{
    internal class MenuTab : MenuFragment
    {

        public string name;
        
        public MenuTab(string name)
        {
            this.name = name;
        }

        public virtual void Draw() { }
    }
}
