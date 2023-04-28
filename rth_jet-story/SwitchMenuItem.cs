using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rth_jet_story
{
    class SwitchMenuItem : MenuItem
    {
        private bool status;
        public bool Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public SwitchMenuItem(Menu parent, int id, string[] items, bool status)
            : base(parent, id, items)
        {
            this.status = status;
        }

        public SwitchMenuItem(Menu parent, int id, string[] items, Layouts layout, bool status)
            : base(parent, id, items, layout)
        {
            this.status = status;
        }

        public SwitchMenuItem(Menu parent, int id, string[] items, Layouts[] layouts, bool status)
            : base(parent, id, items, layouts)
        {
            this.status = status;
        }

        public void toggle()
        {
            this.status = !this.status;
        }
    }
}
