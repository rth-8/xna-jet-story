using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class IType4 : Item
    {
        private static Texture2D item_m_lr;

        public static void loadImages(ContentManager content)
        {
            item_m_lr = content.Load<Texture2D>("images/items/item_m_lr");
        }

        public static void disposeImages()
        {
            item_m_lr.Dispose();
        }

        public IType4()
            : base()
        {
            this.initItem();
        }

        public IType4(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initItem();
        }

        void initItem()
        {
            this.it = item_m_lr;
        }
    }
}
