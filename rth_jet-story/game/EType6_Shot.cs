using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType6_Shot : Shot
    {
        public const int IMAGES = 4;
        private static Texture2D[] e6_shot = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            e6_shot[0] = content.Load<Texture2D>("images/shots/e6_shot_0");
            e6_shot[1] = content.Load<Texture2D>("images/shots/e6_shot_1");
            e6_shot[2] = content.Load<Texture2D>("images/shots/e6_shot_2");
            e6_shot[3] = content.Load<Texture2D>("images/shots/e6_shot_3");
        }

        public static void disposeImages()
        {
            e6_shot[0].Dispose();
            e6_shot[1].Dispose();
            e6_shot[2].Dispose();
            e6_shot[3].Dispose();
        }

        public EType6_Shot()
            : base()
        {
            this.initShot();
        }

        public EType6_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initShot();
        }

        public void initShot()
        {
            this.act_b = 0;
            this.setDimension(e6_shot[0].Width, e6_shot[0].Height);
            this.citac = 0;
        }

        public override void start(Vector2 st)
        {
            //strili dolu
            this.setPosition(st.X, st.Y+this.getH());
            this.setVelocity(0, 100);
            act_b = 0;

            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_6_SHOT);
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            //vykresleni bitmap
            g.Draw(e6_shot[this.act_b], this.position, this.col);
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().Y + this.getH() > Game.RES_Y) this.hide();
            this.moveMassToNewPosition(dt);
            this.citac++;
            if (this.citac == 10)
            {
                switch (act_b)
                {
                    case 0: act_b = 1; break;
                    case 1: act_b = 2; break;
                    case 2: act_b = 3; break;
                    case 3: act_b = 0; break;
                }
                this.citac = 0;
            }
        }

        public override void hide()
        {
            GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_ETYPE_6_SHOT);
            base.hide();
        }
    }
}
