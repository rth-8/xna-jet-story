using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType3 : Enemy
    {
        private EType3_Shot shot; // strela
        private int shot_counter;

        private static Texture2D enemy_03_left;
        private static Texture2D enemy_03_right;

        public static void loadImages(ContentManager content)
        {
            enemy_03_left = content.Load<Texture2D>("images/enemies/enemy_03_0");
            enemy_03_right = content.Load<Texture2D>("images/enemies/enemy_03_1");
        }

        public static void disposeImages()
        {
            enemy_03_left.Dispose();
            enemy_03_right.Dispose();
        }

        public EType3()
            : base()
        {
            this.initEnemy();
        }
        public 
        EType3(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.shot = new EType3_Shot();
            this.shot_counter = 0;

            this.shot.setOrient(false);//doleva

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            if (this.getOrient() == false) //doleva
            {
                g.Draw(enemy_03_left, this.position, this.col);
            }
            if (this.getOrient() == true)
            {
                g.Draw(enemy_03_right, this.position, this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
            if (o.getPosition().X > this.getPosition().X)
            {
                this.setOrient(true);
                this.shot.setOrient(true);
            }
            if (o.getPosition().X < this.getPosition().X)
            {
                this.setOrient(false);
                this.shot.setOrient(false);
            }
            //nepritel samotny je staticky, pouze zajisteni strileni
            if (
                (
                 (o.getPosition().Y + Game.recalculate(10) >=
                     this.getPosition().Y) &&
                 (o.getPosition().Y + Game.recalculate(10) <=
                     this.getPosition().Y + this.getH())
                ) ||
                (
                 (o.getPosition().Y + o.getH() - Game.recalculate(10) >=
                     this.getPosition().Y) &&
                 (o.getPosition().Y + o.getH() - Game.recalculate(10) <=
                     this.getPosition().Y + this.getH())
                )
               )
            {
                if (this.shot.isShown() == false
                    && this.shot_counter == 0)
                {
                    this.shot.start(this.getPosition());
                    this.shot.show();
                    this.shot_counter = 100;
                }
            }

            if (this.shot_counter > 0)
            {
                this.shot_counter--;
            }
        }

        public override Shot getShot()
        {
            return this.shot;
        }
    }
}
