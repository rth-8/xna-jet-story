using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class IType0 : Item
    {
        private static Texture2D item_ammo;

        public static void loadImages(ContentManager content)
        {
            item_ammo = content.Load<Texture2D>("images/items/item_ammo");
        }

        public static void disposeImages()
        {
            item_ammo.Dispose();
        }

        public IType0()
            : base()
        {
            this.initItem();
        }

        public IType0(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initItem();
        }

        void initItem()
        {
            this.it = item_ammo;
        }
    }
}
