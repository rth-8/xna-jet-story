using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType19 : Enemy
    {
        public const int IMAGES = 2;
        private static Texture2D[] enemy_19 = new Texture2D[IMAGES]; //2
        
        public static void loadImages(ContentManager content)
        {
            enemy_19[0] = content.Load<Texture2D>("images/enemies/enemy_19_0");
            enemy_19[1] = content.Load<Texture2D>("images/enemies/enemy_19_1");
        }

        public static void disposeImages()
        {
            enemy_19[0].Dispose();
            enemy_19[1].Dispose();
        }

        public EType19() : base()
        {
            this.initEnemy();
        }

        public EType19(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.citacPohybu = 10;
            this.act_b = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_19[this.act_b], this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            //pohyb
            this.initMass();
            if (this.citacPohybu == 10) {
                switch (this.act_b) {
                    case 0: this.act_b = 1; break;
                    case 1: this.act_b = 0; break;
                }
                this.applyForce(
                        Game.rand.Next(500)-200,
                        Game.rand.Next(500) - 250
                        );
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            //y podle y lode
            if (o.getPosition().X != this.getPosition().X)
                this.applyForce(
                        0,
                        o.getPosition().Y-this.getPosition().Y/10
                        );
            //
            //x podle x lode
            if (o.getPosition().Y != this.getPosition().Y)
                this.applyForce(
                        o.getPosition().X-this.getPosition().X/10,
                        0
                        );
            //
            this.applyForce(0, 1000);
            this.applyForce(0, -1000);
            this.applyForce(1000, 0);
            this.applyForce(-1000, 0);
            //
            //odrazy od okraju screenu
            if (this.getPosition().X < 0)
            {
                this.setPosition(0, this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {
                this.setPosition(Game.RES_X - this.getW(), this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().Y <
                    Game.recalculate(Game.STATUS_BAR_H))
            {
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
