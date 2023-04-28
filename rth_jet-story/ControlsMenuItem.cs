using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace rth_jet_story
{
    class ControlsMenuItem : MenuItem
    {
        private bool changing;
        public bool Changing
        {
            get { return this.changing; }
            set { this.changing = value; }
        }

        private Keys key;
        public Keys Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public ControlsMenuItem(Menu parent, int id, string[] items, Keys key)
            : base(parent, id, items)
        {
            this.changing = false;
            this.key = key;
        }

        public ControlsMenuItem(Menu parent, int id, string[] items, Layouts layout, 
            Keys key)
            : base(parent, id, items, layout)
        {
            this.changing = false;
            this.key = key;
        }

        public ControlsMenuItem(Menu parent, int id, string[] items, Layouts[] layouts, 
            Keys key)
            : base(parent, id, items, layouts)
        {
            this.changing = false;
            this.key = key;
        }
    }
}
