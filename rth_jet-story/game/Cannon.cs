using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Cannon : Shot
    {
        private static Texture2D left;
        private static Texture2D right;

        public Cannon() : base()
        {
        }

        public Cannon(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
        }

        public static void loadImages(ContentManager content) 
        {
            left = content.Load<Texture2D>("images/ship/cannon_left");
            right = content.Load<Texture2D>("images/ship/cannon_right");
        }

        public static void disposeImages()
        {
            left.Dispose();
            right.Dispose();
        }

        public override void start(Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CANON);

            if (this.getOrient() == false)
            {//doleva
                this.setPosition(
                        st.X + Game.recalculate(21),
                        st.Y + Game.recalculate(25)
                        );
                this.setVelocity(-600, 0);
            }
            if (this.getOrient() == true)
            {//doprava
                this.setPosition(
                        st.X + Game.recalculate(78),
                        st.Y + Game.recalculate(25)
                        );
                this.setVelocity(600, 0);
            }
            this.setDimension(left.Width, left.Height);
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            if (this.getOrient() == false)
            {
                //<
                g.Draw(left, 
                    new Vector2(this.position.X, this.position.Y),
                    Color.White);
            }
            else
            {
                //>
                g.Draw(right, 
                    new Vector2(this.position.X, this.position.Y),
                    Color.White);
            }
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X + this.getW() < 0)
            {
                this.hide();
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {
                this.hide();
            }
            this.moveMassToNewPosition(dt);
        }
    }
}
