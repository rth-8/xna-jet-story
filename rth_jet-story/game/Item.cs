using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Item : GameObject
    {
        protected int type;
        protected bool visibility;
        protected int act_c;
        protected Color prev_c;
        protected int citac;

        public const int COLS = 7;
        protected Color [] col = new Color[COLS];

        protected Texture2D it; //reference only

        public Item()
            : base()
        {
            this.type = -1;
            this.visibility = true;
            this.setUpColors();
            this.act_c = 0;
            this.prev_c = Color.White;
            this.citac = 0;
        }

        public Item(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, w, h)
        {
            this.type = -1;
            this.visibility = true;
            this.setUpColors();
            this.act_c = 0;
            this.prev_c = Color.White;
            this.citac = 0;
        }

        public void setUpColors()
        {
            col[0] = new Color(0, 0, 160);
            col[1] = new Color(0, 0, 250);
            col[2] = new Color(0, 128, 255);
            col[3] = Color.Green;
            col[4] = new Color(0, 255, 255);
            col[5] = Color.Yellow;
            col[6] = Color.White;
        }

        public bool isShown()
        {
            return this.visibility;
        }

        public void hide()
        {
            this.visibility = false;
        }

        public void show()
        {
            this.visibility = true;
        }

        public override void draw(SpriteBatch g)
        {
            //Console.WriteLine("item draw");
            if (this.it != null)
            {
                g.Draw(it, this.position, this.prev_c);
            }
        }

        public override void move(float dt)
        {
            //items do not move, only color change
            this.citac++;
            if (this.citac == 5)
            {
                this.prev_c = this.col[act_c];
                this.act_c++;
                if (this.act_c == 7)
                {
                    this.act_c = 0;
                }
                this.citac = 0;
            }
        }

        public int getType()
        {
            return this.type;
        }

        public void setType(int t)
        {
            this.type = t;
        }

        /*public void storeItem(QDataStream& out)
        {
            //type (must be stored first every time !!!)
            out << this.getType();
            out << this.getPosition().getX();
            out << this.getPosition().getY();
            out << this.getW();
            out << this.getH();
            out << this.isShown();
        }*/

        /*public void loadItem(QDataStream& in)
        {
            //type is loaded before creation !!! and set after !!!
            //position
            float x;
            in >> x;
            float y;
            in >> y;
            this.setPosition(x, y);
            //dimension
            int w;
            in >> w;
            int h;
            in >> h;
            this.setDimension(w, h);
            //is shown
            bool s;
            in >> s;
            if (s == true)
            {
                this.show();
            }
            else
            {
                this.hide();
            }
        }*/
    }
}
