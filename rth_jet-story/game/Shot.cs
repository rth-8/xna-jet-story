using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    abstract class Shot : GameObject
    {
        protected int ammo;
        protected bool zobraz;
        protected bool orientace;
        protected Color col;
        protected int act_b;
        protected int citac; //resi zpomaleni vykreslovani

        public Shot() : base()
        {
            this.ammo = -1;
            this.zobraz = false;
            this.orientace = true;//doprava
            this.col = Color.White;
        }

        public Shot(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, w, h)
        {
            this.ammo = -1;
            this.zobraz = false;
            this.orientace = true;//doprava
            this.col = Color.White;
        }

        public void show()
        {
            this.zobraz = true;
        }

        public virtual void hide()
        {
            this.zobraz = false;
        }

        public bool isShown()
        {
            return this.zobraz;
        }

        public int getAmmo()
        {
            return this.ammo;
        }

        public void setAmmo(int a)
        {
            this.ammo = a;
        }

        /**
         *
         * @param or true = 'to left', false = 'to right'
         */
        public void setOrient(bool val)
        {
            this.orientace = val;
        }

        public void setColor(Color c)
        {
            this.col = c;
        }

        public Color getColor()
        {
            return this.col;
        }

        public bool getOrient()
        {
            return this.orientace;
        }

        public abstract void start(Vector2 o);

        public abstract void start(Vector2 o, Vector2 st);

    }
}
