using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType2 : Enemy
    {
        private EType2_Shot shot;
        private int shot_counter;

        private static Texture2D enemy_02;

        public static void loadImages(ContentManager content)
        {
            enemy_02 = content.Load<Texture2D>("images/enemies/enemy_02");
        }

        public static void disposeImages()
        {
            enemy_02.Dispose();
        }

        public EType2() : base()
        {
            this.initEnemy();
        }

        public EType2(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        public void initEnemy()
        {
            this.shot = new EType2_Shot();
            this.shot_counter = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_02,
                new Vector2((int)getPosition().X, (int)getPosition().Y),
                this.col);
        }

        public override void move(GameObject o, float dt)
        {
            //nepritel samotny je staticky, pouze zajisteni strileni
            if (this.shot.isShown() == false
                && this.shot_counter == 0)
            {
                this.shot.start(o.getPosition(), this.getPosition());
                this.shot.show();
                this.shot_counter = 100;
            }

            if (this.shot_counter > 0)
            {
                this.shot_counter--;
            }
        }

        public override Shot getShot()
        {
            return this.shot;//vraci ukazatel na shot
        }
    }
}
