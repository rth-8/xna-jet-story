using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType2_Shot : Shot
    {
        private static Texture2D e2_shot;

        public static void loadImages(ContentManager content)
        {
            e2_shot = content.Load<Texture2D>("images/shots/e2_shot");
        }

        public static void disposeImages()
        {
            e2_shot.Dispose();
        }

        public EType2_Shot() : base()
        {
        }

        public EType2_Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
        }

        public override void start(Vector2 st)
        {
        }

        public override void start(Vector2 o, Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_2_SHOT);

            this.setPosition(
                    st.X + Game.recalculate(25),
                    st.Y + Game.recalculate(10));
            Vector2 tmp1 = new Vector2(o.X, o.Y);
            tmp1 -= st;
            tmp1 /= ((tmp1.Length()/150));
            this.setVelocity(tmp1);
            this.setDimension(e2_shot.Width, e2_shot.Height);
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(e2_shot,
                new Vector2(this.position.X, this.position.Y),
                this.col);
        }

        public override void move(float dt)
        {
            this.initMass();
            if (this.getPosition().X < 0) {
                this.hide();
                return;
            }
            if (this.getPosition().X + this.getW() > Game.RES_X) {
                this.hide();
                return;
            }
            if (this.getPosition().Y < Game.recalculate(Game.STATUS_BAR_H)) {
                this.hide();
                return;
            }
            if (this.getPosition().Y + this.getH() > Game.RES_Y) {
                this.hide();
                return;
            }
            this.moveMassToNewPosition(dt);
        }
    }
}
