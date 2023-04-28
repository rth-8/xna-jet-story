using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType18 : Enemy
    {
        private static Texture2D enemy_18;

        public static void loadImages(ContentManager content)
        {
            enemy_18 = content.Load<Texture2D>("images/enemies/enemy_18");
        }

        public static void disposeImages()
        {
            enemy_18.Dispose();
        }

        public EType18()
            : base()
        {
            this.initEnemy();
        }

        public EType18(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.citacPohybu = 5;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_18, this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            this.initMass();
            if (this.citacPohybu == 5) {
                this.applyForce(
                        Game.rand.Next(200)-100,
                        Game.rand.Next(200) - 100
                        );
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            this.applyForce(0, 100);
            this.applyForce(0, -100);
            this.applyForce(100, 0);
            this.applyForce(-100, 0);
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
