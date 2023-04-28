using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType8 : Enemy
    {
        private EType8_Shot shot;
        private int subtype;

        private static Texture2D enemy_08_0;
        private static Texture2D enemy_08_1;

        public static void loadImages(ContentManager content)
        {
            enemy_08_0 = content.Load<Texture2D>("images/enemies/enemy_08_0");
            enemy_08_1 = content.Load<Texture2D>("images/enemies/enemy_08_1");
        }

        public static void disposeImages()
        {
            enemy_08_0.Dispose();
            enemy_08_1.Dispose();
        }

        public EType8(int sub)
            : base()
        {
            this.initEnemy(sub);
        }

        public EType8(float m, float x, float y, float z, int w, int h, int sub)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy(sub);
        }

        private void initEnemy(int sub)
        {
            this.shot = new EType8_Shot();

            this.subtype = sub;
            if (sub == 0)
            {
                this.shot.setOrient(false);//doleva
            }
            else if (sub == 1)
            {
                this.shot.setOrient(true);//doprava
            }

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            if (this.subtype == 0)
            {
                g.Draw(enemy_08_0, this.position, this.col);
            }
            else if (this.subtype == 1)
            {
                g.Draw(enemy_08_1, this.position, this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
            //nepritel samotny je staticky, pouze zajisteni strileni
            int dx = Math.Abs((int)this.getPosition().X-(int)o.getPosition().X);
            int dy = Math.Abs((int)this.getPosition().Y-(int)o.getPosition().Y);
            if (this.shot.getOrient() == false)
            {//doleva
                if (dx == dy && this.getPosition().X > o.getPosition().X)
                {
                    if (this.shot.isShown() == false)
                    {
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                        
                        this.setDead(); //po vystrelu umira
                        this.shot.start(o.getPosition(), this.getPosition());
                        this.shot.show();
                    }
                }
            }
            if (this.shot.getOrient() == true)
            {//doprava
                if (dx == dy && this.getPosition().X < o.getPosition().X)
                {
                    if (this.shot.isShown() == false)
                    {
                        this.shot.start(o.getPosition(), this.getPosition());
                        this.shot.show();
                        //po vystrelu umira
                        this.setDead();
                    }
                }
            }
        }

        public override Shot getShot()
        {
            return this.shot;//vraci ukazatel na shot
        }

        public int getSubType()
        {
            return this.subtype;
        }
    }
}
