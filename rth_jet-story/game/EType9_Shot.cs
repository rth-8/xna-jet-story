using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType9_Shot : Shot
    {
        private static Texture2D e9_shot_left;
        private static Texture2D e9_shot_right;

        public static void loadImages(ContentManager content)
        {
            e9_shot_left = content.Load<Texture2D>("images/shots/e9_shot_0");
            e9_shot_right = content.Load<Texture2D>("images/shots/e9_shot_1");
        }

        public static void disposeImages()
        {
            e9_shot_left.Dispose();
            e9_shot_right.Dispose();
        }

        public EType9_Shot()
            : base()
        {
            this.initShot();
        }

        public EType9_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initShot();
        }


        public void initShot()
        {
            this.setDimension(e9_shot_left.Width, e9_shot_left.Height);
        }

        public override void start(Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_9_LAUNCH);

            if (this.getOrient() == false)
            {//doleva
                this.setPosition(st.X-this.getW(), st.Y);
                this.setVelocity(-150, 0);
            }
            if (this.getOrient() == true)
            {//doprava
                this.setPosition(st.X+this.getW(), st.Y);
                this.setVelocity(150, 0);
            }

            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_9_SHOT);
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            //vykresleni bitmap
            if (this.getOrient() == false)
            {//doleva
                g.Draw(e9_shot_left, this.position, this.col);
            }
            if (this.getOrient() == true)
            {//doprava
                g.Draw(e9_shot_right, this.position, this.col);
            }
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X + this.getW() < 0)
            {
                this.hide();
                return;
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {
                this.hide();
                return;
            }
            this.moveMassToNewPosition(dt);
        }

        public override void hide()
        {
            GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_ETYPE_9_SHOT);
            base.hide();
        }

    }
}
