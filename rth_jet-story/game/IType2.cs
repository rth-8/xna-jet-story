using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class IType2 : Item
    {
        private static Texture2D item_fuel;

        public static void loadImages(ContentManager content)
        {
            item_fuel = content.Load<Texture2D>("images/items/item_fuel");
        }

        public static void disposeImages()
        {
            item_fuel.Dispose();
        }

        public IType2()
            : base()
        {
            this.initItem();
        }

        public IType2(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initItem();
        }

        void initItem()
        {
            this.it = item_fuel;
        }
    }
}
