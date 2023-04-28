using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Fragment : GameObject
    {
        private int citac;
        private int act_b;
        private int angle;
        private bool zobraz;

        public const int IMGS = 5; 
        public static Texture2D [] img = new Texture2D[IMGS];

        public static void loadImages(ContentManager content)
        {
            img[0] = content.Load<Texture2D>("images/fragment1");
            img[1] = content.Load<Texture2D>("images/fragment2");
            img[2] = content.Load<Texture2D>("images/fragment3");
            img[3] = content.Load<Texture2D>("images/fragment4");
            img[4] = content.Load<Texture2D>("images/fragment5");
        }

        public static void disposeImages()
        {
            for (int i = 0; i < IMGS; ++i)
            {
                img[i].Dispose();
            }
        }

        public Fragment()
            : base()
        {
            this.setDimension(img[0].Width, img[0].Height);
            this.citac = 0;
            this.act_b = Game.rand.Next(0, 4);
            this.angle = 0;
            this.zobraz = true;
            this.updateMembers();
        }

        public Fragment(float m, float x, float y, int w, int h)
            : base(m, x, y, w, h)
        {
            this.setDimension(img[0].Width, img[0].Height);
            this.citac = 0;
            this.act_b = Game.rand.Next(0, 4);
            this.angle = 0;
            this.zobraz = true;
            this.updateMembers();
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(
                img[this.act_b],
                new Vector2((int)this.getPosition().X, (int)this.getPosition().Y),
                Color.White);
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

            this.citac++;
            if (this.citac == 20)
            {
                this.act_b = Game.rand.Next(0, 4);
                this.citac = 0;
            }
            switch (this.angle)
            {
            case 0:
                this.angle = 90;
                break;
            case 90:
                this.angle = 180;
                break;
            case 180:
                this.angle = 270;
                break;
            case 270:
                this.angle = 0;
                break;
            }

            Vector2 grav = new Vector2(0, 200); //jako gravitace
            //aplikace gravitace
            grav *= this.getM();
            this.applyForce(grav);

            this.moveMassToNewPosition(dt);
            this.updateMembers();
        }

        public void show()
        {
            this.zobraz = true;
        }

        public void hide()
        {
            this.zobraz = false;
        }

        public bool isShown()
        {
            return this.zobraz;
        }
    }
}
