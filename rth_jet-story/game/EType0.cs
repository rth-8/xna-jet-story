using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType0 : Enemy
    {
        private static Texture2D enemy_00;

        public EType0() : base()
        {
            this.initEnemy();
        }

        public EType0(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.col = Game.getRandomColor();
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(
                enemy_00,
                new Vector2((int)this.getPosition().X, (int)this.getPosition().Y),
                this.col);
        }

        public static void loadImages(ContentManager content)
        {
            enemy_00 = content.Load<Texture2D>("images/enemies/enemy_00");
        }

        public static void disposeImages()
        {
            enemy_00.Dispose();
        }
    }
}
