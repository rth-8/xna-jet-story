using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType9 : Enemy
    {
        private EType9_Shot shot;

        public const int IMAGES = 8;
        private static Texture2D[] enemy_09 = new Texture2D[IMAGES];

        public static void loadImages(ContentManager content)
        {
            enemy_09[0] = content.Load<Texture2D>("images/enemies/enemy_09_0");
            enemy_09[1] = content.Load<Texture2D>("images/enemies/enemy_09_1");
            enemy_09[2] = content.Load<Texture2D>("images/enemies/enemy_09_2");
            enemy_09[3] = content.Load<Texture2D>("images/enemies/enemy_09_3");
            enemy_09[4] = content.Load<Texture2D>("images/enemies/enemy_09_4");
            enemy_09[5] = content.Load<Texture2D>("images/enemies/enemy_09_5");
            enemy_09[6] = content.Load<Texture2D>("images/enemies/enemy_09_6");
            enemy_09[7] = content.Load<Texture2D>("images/enemies/enemy_09_7");
        }

        public static void disposeImages()
        {
            for (int i = 0; i < IMAGES; ++i)
            {
                enemy_09[i].Dispose();
            }
        }

        public EType9() : base()
        {
            this.initEnemy();
        }

        public EType9(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy();
        }

        private void initEnemy()
        {
            this.shot = new EType9_Shot();

            this.act_b = 0;
            this.citac = 0;

            this.setColor(Game.getRandomColor());
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(enemy_09[this.act_b], this.position, this.col);
        }

        public override void move(GameObject o, float dt)
        {
            //nepritel samotny je staticky, pouze zajisteni strileni
            if (o.getPosition().Y + Game.recalculate(10) >= this.getPosition().Y)
            {
                if (this.shot.isShown() == false)
                {
                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ETYPE_9_LAUNCH);
                    
                    this.setDead();//po vystrelu umira
                    this.shot.start(this.getPosition());
                    this.shot.show();
                }
            }

            this.citac++;
            if (this.citac == 15)
            {
                switch(act_b)
                {
                    case 0: act_b = 1;
                        this.shot.setOrient(true);
                        break;
                    case 1: act_b = 2;
                        this.shot.setOrient(true);
                        break;
                    case 2: act_b = 3;
                        this.shot.setOrient(false);
                        break;
                    case 3: act_b = 4;
                        this.shot.setOrient(false);
                        break;
                    case 4: act_b = 5;
                        this.shot.setOrient(false);
                        break;
                    case 5: act_b = 6;
                        this.shot.setOrient(false);
                        break;
                    case 6: act_b = 7;
                        this.shot.setOrient(true);
                        break;
                    case 7: act_b = 0;
                        this.shot.setOrient(true);
                        break;
                }
                this.citac = 0;
            }
        }

        public override Shot getShot()
        {
            return this.shot;//vraci ukazatel na shot
        }
    }
}
