using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType17 : Enemy
    {
        public const int LIMAGES = 2;
        private static Texture2D[] eb_left = new Texture2D[LIMAGES]; //2
        public const int RIMAGES = 2;
        private static Texture2D[] eb_right = new Texture2D[RIMAGES]; //2

        public static void loadImages(ContentManager content)
        {
            eb_left[0] = content.Load<Texture2D>("images/enemies/enemy_17_0_0");
            eb_left[1] = content.Load<Texture2D>("images/enemies/enemy_17_0_1");

            eb_right[0] = content.Load<Texture2D>("images/enemies/enemy_17_1_0");
            eb_right[1] = content.Load<Texture2D>("images/enemies/enemy_17_1_1");
        }

        public static void disposeImages()
        {
            eb_left[0].Dispose();
            eb_left[1].Dispose();

            eb_right[0].Dispose();
            eb_right[1].Dispose();
        }

        public EType17()
            : base()
        {
            this.initEnemy();
        }

        public EType17(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.setOrient(true);
            this.citacPohybu = 10;
            this.act_b = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            //doleva
            if (this.getOrient() == false)
            {
                g.Draw(eb_left[this.act_b], this.position, this.col);
            }
            //doprava
            if (this.getOrient() == true) {
                g.Draw(eb_right[this.act_b], this.position, this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
            //pohyb
            if (o.getPosition().X > this.getPosition().X) {
                this.setOrient(true);
            }
            if (o.getPosition().X < this.getPosition().X) {
                this.setOrient(false);
            }
            this.initMass();
            if (this.citacPohybu == 10) {
                switch (this.act_b) {
                    case 0: this.act_b = 1; break;
                    case 1: this.act_b = 0; break;
                }
                this.applyForce(
                        Game.rand.Next(500)-250,
                        Game.rand.Next(500) - 250);
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            //y podle y lode
            if (o.getPosition().X != this.getPosition().X)
                this.applyForce(
                        0,
                        o.getPosition().Y-this.getPosition().Y/10);
            //
            //x podle x lode
            if (o.getPosition().Y != this.getPosition().Y)
                this.applyForce(
                        o.getPosition().X-this.getPosition().X/10,
                        0);
            //
            this.applyForce(0, 1000);
            this.applyForce(0, -1000);
            this.applyForce(1000, 0);
            this.applyForce(-1000, 0);
            //
            //odrazy od okraju screenu
            if (this.getPosition().X < 0) {
                this.setPosition(0, this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {
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
