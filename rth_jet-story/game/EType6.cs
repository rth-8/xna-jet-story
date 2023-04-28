using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType6 : Enemy
    {
        private EType6_Shot shot;
        private int shot_counter;

        private static Texture2D enemy_06;
        
        public static void loadImages(ContentManager content)
        {
            enemy_06 = content.Load<Texture2D>("images/enemies/enemy_06");
        }

        public static void disposeImages()
        {
            enemy_06.Dispose();
        }

        public EType6()
            : base()
        {
            this.initEnemy();
        }

        public EType6(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.shot = new EType6_Shot();
            this.shot_counter = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_06, this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            //nepritel samotny je staticky, pouze zajisteni strileni
            if (
                (
                 (o.getPosition().X + Game.recalculate(10) >=
                     this.getPosition().X) &&
                 (o.getPosition().X + Game.recalculate(10) <=
                     this.getPosition().X + this.getW())
                ) ||
                (
                 (o.getPosition().X + o.getW() - Game.recalculate(10) >=
                     this.getPosition().X) &&
                 (o.getPosition().X + o.getW() - Game.recalculate(10) <=
                     this.getPosition().X + this.getW())
                )
               )
            {
                if (this.shot.isShown() == false
                    && this.shot_counter == 0)
                {
                    this.shot.start(this.getPosition());
                    this.shot.show();
                    this.shot_counter = 50;
                }
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
