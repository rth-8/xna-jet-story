using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType3_Shot : Shot
    {
        private static Texture2D e3_shot;

        public static void loadImages(ContentManager content)
        {
            e3_shot = content.Load<Texture2D>("images/shots/e3_e13_shot");
        }

        public static void disposeImages()
        {
            e3_shot.Dispose();
        }

        public EType3_Shot() : base()
        {
        }

        public EType3_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
        }

        public override void start(Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_3_13_SHOT);

            if (this.getOrient() == false)
            {//doleva
                this.setPosition(
                        st.X - this.getW(),
                        st.Y + Game.recalculate(25));
                this.setVelocity(-250, 0);
            }
            if (this.getOrient() == true)
            {//doprava
                this.setPosition(
                        st.X + this.getW(),
                        st.Y + Game.recalculate(25));
                this.setVelocity(250, 0);
            }
            this.setDimension(e3_shot.Width, e3_shot.Height);
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(e3_shot, this.position, this.col);
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X + this.getW() < 0) {
                this.hide();
                return;
            }
            if (this.getPosition().X + this.getW() > Game.RES_X) {
                this.hide();
                return;
            }
            this.moveMassToNewPosition(dt);
        }


    }
}
