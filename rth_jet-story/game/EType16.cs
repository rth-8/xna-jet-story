using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType16 : Enemy
    {
        public const int IMAGES = 4;
        private static Texture2D[] enemy_16 = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            enemy_16[0] = content.Load<Texture2D>("images/enemies/enemy_16_0");
            enemy_16[1] = content.Load<Texture2D>("images/enemies/enemy_16_1");
            enemy_16[2] = content.Load<Texture2D>("images/enemies/enemy_16_2");
            enemy_16[3] = content.Load<Texture2D>("images/enemies/enemy_16_3");
        }

        public static void disposeImages()
        {
            enemy_16[0].Dispose();
            enemy_16[1].Dispose();
            enemy_16[2].Dispose();
            enemy_16[3].Dispose();
        }

        public EType16()
            : base()
        {
            this.initEnemy();
        }

        public EType16(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.citacPohybu = 5;
            this.act_b = 0;
            int vx = Game.rand.Next(500)-250;
            if (vx > -100 && vx < 100) {
                if (vx < 0) vx = -100;
                else vx = 100;
            }
            int vy = Game.rand.Next(500)-250;
            if (vy > -100 && vy < 100) {
                if (vy < 0) vy = -100;
                else vy = 100;
            }
            this.setVelocity(vx, vy);

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_16[this.act_b], this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            this.initMass();
            if (this.citacPohybu == 5) {
                switch(act_b) {
                    case 0: act_b = 1; break;
                    case 1: act_b = 2; break;
                    case 2: act_b = 3; break;
                    case 3: act_b = 0; break;
                }
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            //odrazy od okraju screenu
            if (this.getPosition().X < 0) {
                this.setPosition(0, this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().X+this.getW() > Game.RES_X) {
                this.setPosition(Game.RES_X - this.getW(), this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().Y <
                    Game.recalculate(Game.STATUS_BAR_H)) {
                this.setPosition(
                        this.getPosition().X,
                        Game.recalculate(Game.STATUS_BAR_H));
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            if (this.getPosition().Y + this.getH() > Game.RES_Y)
            {
                this.setPosition(this.getPosition().X, Game.RES_Y - this.getH());
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            this.moveMassToNewPosition(dt);
        }
    }
}
