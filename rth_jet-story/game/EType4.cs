using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType4 : Enemy
    {
        private static Texture2D enemy_04;

        public static void loadImages(ContentManager content)
        {
            enemy_04 = content.Load<Texture2D>("images/enemies/enemy_04");
        }

        public static void disposeImages()
        {
            enemy_04.Dispose();
        }

        public EType4()
            : base()
        {
            this.initEnemy();
        }

        public EType4(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_04, this.position, this.col);
        }
    }
}
