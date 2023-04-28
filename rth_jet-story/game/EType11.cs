using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType11 : Enemy
    {
        private static Texture2D enemy_11_0;
        private static Texture2D enemy_11_1;

        public static void loadImages(ContentManager content)
        {
            enemy_11_0 = content.Load<Texture2D>("images/enemies/enemy_11_0");
            enemy_11_1 = content.Load<Texture2D>("images/enemies/enemy_11_1");
        }

        public static void disposeImages()
        {
            enemy_11_0.Dispose();
            enemy_11_1.Dispose();
        }

        public EType11()
            : base()
        {
            this.initEnemy();
        }

        public EType11(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.setOrient(true);
            this.citacPohybu = 10;

            //this.setColor(Game.getRandomColor());
            this.col = Game.getRandomColor();
            //Console.WriteLine(this.col.ToString());
        }

        public override void draw(SpriteBatch g)
        {
            //doleva
            if (this.getOrient() == false) {
                g.Draw(enemy_11_0, this.position, this.col);
            }
            //doprava
            if (this.getOrient() == true) {
                g.Draw(enemy_11_1, this.position, this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
            if (o.getPosition().X > this.getPosition().X)
            {
                this.setOrient(true);
            }
            if (o.getPosition().X < this.getPosition().X)
            {
                this.setOrient(false);
            }
            this.initMass();
            if (this.citacPohybu == 10)
            {
                this.applyForce(Game.rand.Next(500)-250, 0);
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            //y podle y lode
            if (o.getPosition().X != this.getPosition().X)
                this.applyForce(
                        0,
                        o.getPosition().Y - this.getPosition().Y);
            //
            this.applyForce(0, 10000);
            this.applyForce(0, -10000);
            this.applyForce(100, 0);
            this.applyForce(-100, 0);
            //
            //odrazy od okraju screenu
            if (this.getPosition().X < 0) {
                this.setPosition(0, this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().X+this.getW() > Game.RES_X) {
                this.setPosition(Game.RES_X-this.getW(), this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().Y <
                    Game.recalculate(Game.STATUS_BAR_H)) {
                this.setPosition(
                        this.getPosition().X,
                        Game.recalculate(Game.STATUS_BAR_H));
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            if (this.getPosition().Y+this.getH() > Game.RES_Y) {
                this.setPosition(this.getPosition().X, Game.RES_Y-this.getH());
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            this.moveMassToNewPosition(dt);
        }
    }
}
