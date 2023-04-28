using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType7_Shot : Shot
    {
        public const int IMAGES = 4;
        private static Texture2D[] e7_shot = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            e7_shot[0] = content.Load<Texture2D>("images/shots/e7_shot_0");
            e7_shot[1] = content.Load<Texture2D>("images/shots/e7_shot_1");
            e7_shot[2] = content.Load<Texture2D>("images/shots/e7_shot_2");
            e7_shot[3] = content.Load<Texture2D>("images/shots/e7_shot_3");
        }

        public static void disposeImages()
        {
            e7_shot[0].Dispose();
            e7_shot[1].Dispose();
            e7_shot[2].Dispose();
            e7_shot[3].Dispose();
        }

        public EType7_Shot()
            : base()
        {
        }

        public EType7_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
        }

        public override void start(Vector2 st)
        {
        }

        public override void start(Vector2 o, Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_7_SHOT);

            this.setPosition(
                    st.X + Game.recalculate(25),
                    st.Y + Game.recalculate(10)
                    );
            Vector2 tmp1 = new Vector2(o.X, o.Y);
            tmp1 -= st;
            tmp1 /= tmp1.Length() / 200;
            this.setVelocity(tmp1);
            this.setDimension(e7_shot[0].Width, e7_shot[0].Height);
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(e7_shot[this.act_b], this.position, this.col);
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

    }
}
