using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType8_Shot : Shot
    {
        private static Texture2D e8_shot_0;
        private static Texture2D e8_shot_1;

        public static void loadImages(ContentManager content)
        {
            e8_shot_0 = content.Load<Texture2D>("./images/shots/e8_shot_0");
            e8_shot_1 = content.Load<Texture2D>("./images/shots/e8_shot_1");
        }

        public static void disposeImages()
        {
            e8_shot_0.Dispose();
            e8_shot_1.Dispose();
        }

        public EType8_Shot()
            : base()
        {
            this.initShot();
        }

        public EType8_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initShot();
        }

        public void initShot()
        {
            this.setDimension(e8_shot_0.Width, e8_shot_0.Height);
        }

        public override void start(Vector2 st)
        {
        }

        public override void start(Vector2 o, Vector2 st)
        {
            this.setPosition(st.X,st.Y);
            Vector2 tmp1 = new Vector2(o.X, o.Y);
            tmp1 -= st;
            tmp1 /= tmp1.Length() / 150;
            this.setVelocity(tmp1);

            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_8_SHOT);
        }

        public override void draw(SpriteBatch g)
        {
            //vykresleni bitmap
            if (this.getOrient() == false)
            {//doleva
                g.Draw(e8_shot_0, this.position, this.col);
            }
            if (this.getOrient() == true)
            {//doprava
                g.Draw(e8_shot_1, this.position, this.col);
            }
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X < 0)
            {
                this.hide();
                return;
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {
                this.hide();
                return;
            }
            if (this.getPosition().Y < Game.recalculate(Game.STATUS_BAR_H))
            {
                this.hide();
                return;
            }
            if (this.getPosition().Y + this.getH() > Game.RES_Y)
            {
                this.hide();
                return;
            }
            this.moveMassToNewPosition(dt);
        }

        public override void hide()
        {
            GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_ETYPE_8_SHOT);
            base.hide();
        }
    }
}
