using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Boom
    {
        private float x;
        private float y;
        private int citac;
        private int act_b;
        private bool zobraz;
        private bool is_base;

        public const int FRAGS = 8;
        private Fragment[] fragments;

        public const int BOOM_IMGS = 6;
        public static Texture2D[] bboom = new Texture2D[BOOM_IMGS];
     
        public static void loadImages(ContentManager content)
        {
            bboom[0] = content.Load<Texture2D>("images/boom_1");
            bboom[1] = content.Load<Texture2D>("images/boom_2");
            bboom[2] = content.Load<Texture2D>("images/boom_3");
            bboom[3] = content.Load<Texture2D>("images/boom_4");
            bboom[4] = content.Load<Texture2D>("images/boom_5");
            bboom[5] = content.Load<Texture2D>("images/boom_6");
        }

        public static void disposeImages()
        {
            for (int i = 0; i < BOOM_IMGS; ++i)
            {
                bboom[i].Dispose();
            }
        }

        public Boom()
        {
            this.x = 0;
            this.y = 0;
            this.citac = 0;
            this.act_b = 0;
            this.zobraz = false;
            this.is_base = false;
        }

        public Boom(float x, float y, bool show)
        {
            this.x = x;
            this.y = y;
            this.citac = 0;
            this.act_b = 0;
            this.zobraz = show;
            this.is_base = false;

            this.setUpFragments();
        }

        public Boom(float x, float y, bool show, bool isBaseBoom)
        {
            this.x = x;
            this.y = y;
            this.citac = 0;
            this.act_b = 0;
            this.zobraz = show;
            this.is_base = isBaseBoom;

            this.setUpFragments();
        }

        private void setUpFragments()
        {
            this.fragments = new Fragment[FRAGS];

            // 0 1 2
            // 3   4
            // 5 6 7

            int w = bboom[0].Width;
            int h = bboom[0].Height;
            int cx = (int)(x + w / 2);
            int cy = (int)(y + h / 2);

            this.fragments[0] = new Fragment(
                    1, cx - Game.rand.Next(w), cy - Game.rand.Next(h), 1, 1);
            this.fragments[1] = new Fragment(
                    1, cx, cy - Game.rand.Next(h), 1, 1);
            this.fragments[2] = new Fragment(
                    1, cx + Game.rand.Next(w), cy - Game.rand.Next(h), 1, 1);
            this.fragments[3] = new Fragment(
                    1, cx - Game.rand.Next(w), cy, 1, 1);
            this.fragments[4] = new Fragment(
                    1, cx + Game.rand.Next(w), cy, 1, 1);
            this.fragments[5] = new Fragment(
                    1, cx - Game.rand.Next(w), cy + Game.rand.Next(h), 1, 1);
            this.fragments[6] = new Fragment(
                    1, cx, cy + Game.rand.Next(h), 1, 1);
            this.fragments[7] = new Fragment(
                    1, cx + Game.rand.Next(w), cy + Game.rand.Next(h), 1, 1);

            this.fragments[0].setVelocity(
                    -(Game.rand.Next(200) + 100), -(Game.rand.Next(200) + 100));
            this.fragments[1].setVelocity(
                    0, -(Game.rand.Next(200) + 100));
            this.fragments[2].setVelocity(
                    (Game.rand.Next(200) + 100), -(Game.rand.Next(200) + 100));
            this.fragments[3].setVelocity(
                    -(Game.rand.Next(200) + 100), 0);
            this.fragments[4].setVelocity(
                    (Game.rand.Next(200) + 100), 0);
            this.fragments[5].setVelocity(
                    -(Game.rand.Next(200) + 100), (Game.rand.Next(200) + 100));
            this.fragments[6].setVelocity(
                    0, (Game.rand.Next(200) + 100));
            this.fragments[7].setVelocity(
                    (Game.rand.Next(200) + 100), (Game.rand.Next(200) + 100));
        }

        public float getX()
        {
            return this.x;
        }

        public float getY()
        {
            return this.y;
        }

        public int getActB()
        {
            return this.act_b;
        }

        public bool isShown()
        {
            return this.zobraz;
        }

        public bool isBase()
        {
            return this.is_base;
        }

        public void draw(SpriteBatch g)
        {
            if (this.zobraz)
            {
                g.Draw(
                    bboom[this.getActB()],
                    new Vector2((int)this.getX(), (int)this.getY()),
                    Color.White
                    );
            }

            for (int i=0; i<FRAGS; ++i)
            {
                if (this.fragments[i].isShown())
                {
                    this.fragments[i].draw(g);
                }
            }
        }

        public void move(float dt)
        {
            this.citac++;
            if (this.citac == 3)
            {
                switch (this.act_b)
                {
                    case 0: this.act_b = 1;
                        break;
                    case 1: this.act_b = 2;
                        break;
                    case 2: this.act_b = 3;
                        break;
                    case 3: this.act_b = 4;
                        break;
                    case 4: this.act_b = 5;
                        break;
                    case 5: this.zobraz = false;
                        break;
                }
                this.citac = 0;
            }

            for (int i=0; i<FRAGS; ++i)
            {
                if (this.fragments[i].isShown())
                {
                    this.fragments[i].move(dt);
                }
            }
        }

        public bool isFinished()
        {
            bool allHidden = true;
            for (int i=0; i<FRAGS; ++i)
            {
                if (this.fragments[i].isShown())
                {
                    allHidden = false;
                    break;
                }
            }
            if (allHidden && this.zobraz)
            {
                return false;
            }
            return allHidden;
        }

        public Fragment getFragment(int idx)
        {
            return this.fragments[idx];
        }
    }
}
