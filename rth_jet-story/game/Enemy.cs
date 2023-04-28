using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Enemy : GameObject
    {
        protected int type;
        protected Color col;
        protected float damage; //pocitadlo zasahu do smrti
        protected bool dead; //?mrtvy?
        protected bool orientace;
        protected int citac;
        protected int act_b;
        protected int citacPohybu;

        public Enemy() : base()
        {
            this.type = -1;
            this.col = Color.White;
            this.damage = -1;//nesmrtelny
            this.dead = false;
        }

        public Enemy(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, w, h)
        {
            this.type = -1;
            this.col = Color.White;
            this.damage = -1;//nesmrtelny
            this.dead = false;
        }

        public void setColor(Color c)
        {
            this.col = c;
        }

        public Color getColor()
        {
            return this.col;
        }

        public void setDamage(float d)
        {
            this.damage = d;
        }

        public float getDamage()
        {
            return this.damage;
        }

        public bool isDead()
        {
            return this.dead;
        }

        public virtual Shot getShot()
        {
            return null;
        }

        public void setDead()
        {
            this.dead = true;
        }

        public int getType()
        {
            return this.type;
        }

        public void setType(int t)
        {
            this.type = t;
        }

        public bool getOrient()
        {
            return this.orientace;
        }

        public void setOrient(bool o)
        {
            this.orientace = o;
        }

        /*public void storeEnemy(QDataStream& out)
        {
            //type
            out << this.getType();
            //some enemies have sub types, store it as second, if exists
            switch (this.getType())
            {
                case 1:
                    out << ((EType1*)this)->getSubType();
                    break;
                case 8:
                    out << ((EType8*)this)->getSubType();
                    break;
                case 20:
                    out << ((EType20*)this)->getSubType();
                    break;
            }
            //m
            out << this.getM();
            //dimension
            out << this.getW();
            out << this.getH();
            //position
            out << this.getPosition().getX();
            out << this.getPosition().getY();
            //velocity
            out << this.getVelocity().getX();
            out << this.getVelocity().getY();
            //force
            out << this.getForce().getX();
            out << this.getForce().getY();
            //damage
            out << this.getDamage();
            //orient
            out << this.getOrient();
            //dead status
            out << this.isDead();
        }*/

        /*public void loadEnemy(QDataStream& in)
        {
            //type is loaded before creation !!! and set after !!!
            //possible subtype is loaded before creation !!!
            //m
            float tm;
            in >> tm;
            this.setM(tm);
            //dimension
            int w;
            in >> w;
            int h;
            in >> h;
            this.setDimension(w, h);
            //position
            float x;
            in >> x;
            float y;
            in >> y;
            this.setPosition(x, y);
            //velocity
            float vx;
            in >> vx;
            float vy;
            in >> vy;
            this.setVelocity(vx, vy);
            //force
            float fx;
            in >> fx;
            float fy;
            in >> fy;
            this.setForce(fx, fy);
            //damage
            float td;
            in >> td;
            this.setDamage(td);
            //orient
            bool to;
            in >> to;
            this.setOrient(to);
            //dead status
            bool st;
            in >> st;
            if (st == true)
            {
                this.setDead();
            }
        }*/

        //from parent
        public override void draw(SpriteBatch g) { }

        //from parent
        public override void move(float dt) { }

        //special for Enemy
        public virtual void move(GameObject o, float dt) { }
    }
}
