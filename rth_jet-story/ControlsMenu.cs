using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace rth_jet_story
{
    class ControlsMenu : Menu
    {
        private bool first;

        public ControlsMenu(Game parent, int x, int y, int w, int h, SpriteFont font)
            : base(parent, x, y, w, h, font)
        {
            this.first = true;
        }

        public override int processKey(KeyboardState kbs)
        {
            int ret = base.processKey(kbs);

            if (!this.first)
            {
                ControlsMenuItem item = (ControlsMenuItem)this.items[this.act_item];
                if (ret > -1 && !item.Changing)
                {
                    Console.WriteLine("press a key to change");
                    item.Changing = true;
                }
                else if (kbs.GetPressedKeys().Length > 0 && item.Changing && !this.keyEnter)
                {
                    Console.WriteLine("new key = " + kbs.GetPressedKeys()[0].ToString());
                    item.Key = kbs.GetPressedKeys()[0];
                    item.setText(1, kbs.GetPressedKeys()[0].ToString());
                    item.Changing = false;
                }
            }
            else
            {
                this.first = false;
            }

            return ret;
        }
    }
}
