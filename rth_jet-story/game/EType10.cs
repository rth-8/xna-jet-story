using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType10 : Enemy
    {
        private static Texture2D enemy_10_0;
        private static Texture2D enemy_10_1;

        public static void loadImages(ContentManager content)
        {
            enemy_10_0 = content.Load<Texture2D>("images/enemies/enemy_10_0");
            enemy_10_1 = content.Load<Texture2D>("images/enemies/enemy_10_1");
        }

        public static void disposeImages()
        {
            enemy_10_0.Dispose();
            enemy_10_1.Dispose();
        }

        public EType10()
            : base()
        {
            this.initEnemy();
        }

        public EType10(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.citac = 0;
            this.act_b = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            if (this.act_b == 0)
            {
                g.Draw(enemy_10_0, this.position, this.col);
            }
            else if (this.act_b == 1)
            {
                g.Draw(enemy_10_1, this.position, this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
            //objekt o je nevyuzit
            this.citac++;
            if (this.citac == 500)
            {
                switch(act_b)
                {
                    case 0: act_b = 1; break;
                    case 1: act_b = 0; break;
                }
                this.citac = 0;
            }
        }

        public bool release()
        {
            if (this.citac == 0)
            {
                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_10);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
