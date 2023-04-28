using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType7 : Enemy
    {
        public const int IMAGES = 4;
        private static Texture2D[] enemy_07 = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            enemy_07[0] = content.Load<Texture2D>("images/enemies/enemy_07_0");
            enemy_07[1] = content.Load<Texture2D>("images/enemies/enemy_07_1");
            enemy_07[2] = content.Load<Texture2D>("images/enemies/enemy_07_2");
            enemy_07[3] = content.Load<Texture2D>("images/enemies/enemy_07_3");
        }

        public static void disposeImages()
        {
            enemy_07[0].Dispose();
            enemy_07[1].Dispose();
            enemy_07[2].Dispose();
            enemy_07[3].Dispose();
        }

        private EType7_Shot shot;
        private int shot_counter;

        public EType7()
            : base()
        {
            this.initEnemy();
        }

        public EType7(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.shot = new EType7_Shot();
            this.shot_counter = 0;

            this.act_b = 0;
            this.citac = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_07[this.act_b], this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            //nepritel samotny je staticky, pouze zajisteni strileni
            if (this.shot.isShown() == false
                && this.shot_counter == 0)
            {
                //qDebug() << "+ move: start shooting";
                this.shot.start(o.getPosition(), this.getPosition());
                this.shot.show();
                this.shot_counter = 50;
            }

            this.citac++;
            if (this.citac == 10)
            {
                switch(act_b)
                {
                    case 0: act_b = 1; break;
                    case 1: act_b = 2; break;
                    case 2: act_b = 3; break;
                    case 3: act_b = 0; break;
                }
                this.citac = 0;
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
