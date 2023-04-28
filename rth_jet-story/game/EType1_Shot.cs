using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType1_Shot : Shot
    {
        public const int IMAGES = 4;
        private static Texture2D[] e1_shot = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            e1_shot[0] = content.Load<Texture2D>("images/shots/e1_shot_0");
            e1_shot[1] = content.Load<Texture2D>("images/shots/e1_shot_1");
            e1_shot[2] = content.Load<Texture2D>("images/shots/e1_shot_2");
            e1_shot[3] = content.Load<Texture2D>("images/shots/e1_shot_3");
        }

        public static void disposeImages()
        {
            e1_shot[0].Dispose();
            e1_shot[1].Dispose();
            e1_shot[2].Dispose();
            e1_shot[3].Dispose();
        }

        public EType1_Shot()
            : base()
        {
            this.initShot();
        }

        public EType1_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initShot();
        }


        private void initShot()
        {
            this.act_b = 0;
            this.setDimension(e1_shot[0].Width, e1_shot[0].Height);
        }

        public override void start(Vector2 st)
        {
            //qDebug() << "> start";
            if (this.getOrient() == false)
            {//doleva
                this.setPosition(st.X - this.getW(), st.Y);
                this.setVelocity(-250, 0);
            }
            if (this.getOrient() == true)
            {//doprava
                this.setPosition(st.X + this.getW(), st.Y);
                this.setVelocity(250, 0);
            }
            this.act_b = 0;
            
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_1_SHOT);
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            //vykresleni bitmap
            g.Draw(e1_shot[this.act_b], this.position, this.col);
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X + this.getW() < 0)
            {
                this.hide();
            }
            if (this.getPosition().X + this.getW() >Game.RES_X)
            {
                this.hide();
            }
            this.moveMassToNewPosition(dt);
            switch(act_b)
            {
                case 0: act_b = 1; break;
                case 1: act_b = 2; break;
                case 2: act_b = 3; break;
                case 3: act_b = 0; break;
            }
        }

        public override void hide()
        {
            GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_ETYPE_1_SHOT);
            base.hide();
        }

    }
}
