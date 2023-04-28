using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType20 : Enemy
    {
        private Enemy ee;
        private Item ii;
        private int subtype;

        public const int IMAGES = 2;
        private static Texture2D[] enemy_20_0 = new Texture2D[IMAGES]; //2
        private static Texture2D[] enemy_20_1 = new Texture2D[IMAGES]; //2

        public static void loadImages(ContentManager content)
        {
            enemy_20_0[0] = content.Load<Texture2D>("images/enemies/enemy_20_0_0");
            enemy_20_0[1] = content.Load<Texture2D>("images/enemies/enemy_20_0_1");

            enemy_20_1[0] = content.Load<Texture2D>("images/enemies/enemy_20_1_0");
            enemy_20_1[1] = content.Load<Texture2D>("images/enemies/enemy_20_1_1");
        }

        public static void disposeImages()
        {
            enemy_20_0[0].Dispose();
            enemy_20_0[1].Dispose();

            enemy_20_1[0].Dispose();
            enemy_20_1[1].Dispose();
        }

        public EType20(int sub) : base()
        {
            this.initEnemy(sub);
        }

        public EType20(float m, float x, float y, float z, int w, int h, int sub)
            : base(m, x, y, z, w, h)
        {
            this.initEnemy(sub);
        }

        private void initEnemy(int sub)
        {
            this.ee = null;

            this.ii = null;

            this.subtype = sub;
            this.citacPohybu = 5;
            this.act_b = 0;

            this.setColor(Game.getRandomColor());
        }

        public void createFellowEnemy(
                float m, float x, float y, float z, int w, int h, int t, int sub)
        {
            this.destroyFellowEnemy();
            switch (t)
            {
                case 1:
                    this.ee = new EType1(m,x,y,z,w,h,sub);
                    this.ee.setDamage(10);
                    break;
                case 2:
                    this.ee = new EType2(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 3:
                    this.ee = new EType3(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 4:
                    this.ee = new EType4(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 5:
                    this.ee = new EType5(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 7:
                    this.ee = new EType7(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 8:
                    this.ee = new EType8(m,x,y,z,w,h,sub);
                    this.ee.setDamage(10);
                    break;
                case 9:
                    this.ee = new EType9(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
                case 10:
                    this.ee = new EType10(m,x,y,z,w,h);
                    this.ee.setDamage(10);
                    break;
            }
            this.ee.setType(t);
        }

        public void createFellowItem(
                float m, float x, float y, float z, int w, int h, int t)
        {
            this.destroyFellowItem();
            switch (t) {
                case 0:
                    this.ii = new IType0(m,x,y,z,w,h);
                    break;
                case 1:
                    this.ii = new IType1(m,x,y,z,w,h);
                    break;
                case 2:
                    this.ii = new IType2(m,x,y,z,w,h);
                    break;
                case 3:
                    this.ii = new IType3(m,x,y,z,w,h);
                    break;
                case 4:
                    this.ii = new IType4(m,x,y,z,w,h);
                    break;
                case 5:
                    this.ii = new IType5(m,x,y,z,w,h);
                    break;
            }
            this.ii.setType(t);
        }

        public override void draw(SpriteBatch g)
        {
            if (this.subtype == 0)
            {
                g.Draw(enemy_20_0[this.act_b], this.position, this.col);
            }
            else if (this.subtype == 1)
            {
                g.Draw(enemy_20_1[this.act_b], this.position, this.col);
            }

            //Enemy
            if (this.ee != null && this.ee.isDead() == false)
            {
                this.ee.draw(g);
                if (this.ee.getShot() != null
                    && this.ee.getShot().isShown() == true)
                {
                    this.ee.getShot().draw(g);
                }
            }
            //Item
            if (this.ii != null && this.ii.isShown() == true)
            {
                this.ii.draw(g);
            }
        }

        public override void move(GameObject o, float dt)
        {
            this.initMass();
            if (this.citacPohybu == 5)
            {
                switch(act_b)
                {
                    case 0: act_b = 1; break;
                    case 1: act_b = 0; break;
                }
                this.applyForce(
                        Game.rand.Next(200) - 100,
                        Game.rand.Next(200) - 100
                        );
                this.citacPohybu = 0;
            }
            this.citacPohybu++;
            this.applyForce(0, 100);
            this.applyForce(0, -100);
            this.applyForce(100, 0);
            this.applyForce(-100, 0);
            //
            //odrazy od okraju screenu
            if (this.getPosition().X < 0)
            {
                this.setPosition(0, this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().X+this.getW() > Game.RES_X)
            {
                this.setPosition(Game.RES_X-this.getW(), this.getPosition().Y);
                this.setVelocity(-this.getVelocity().X, this.getVelocity().Y);
            }
            if (this.getPosition().Y <
                    Game.recalculate(Game.STATUS_BAR_H))
            {
                this.setPosition(
                        this.getPosition().X,
                        Game.recalculate(Game.STATUS_BAR_H));
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            if (this.getPosition().Y+this.getH() > Game.RES_Y) {
                this.setPosition(this.getPosition().X, Game.RES_Y-this.getH());
                this.setVelocity(this.getVelocity().X, -this.getVelocity().Y);
            }
            this.moveMassToNewPosition(dt);
            //
            if (this.ee != null)
            {
                this.ee.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y - Game.recalculate(50));
                this.ee.move(o, dt);
                this.ee.updateMembers();
                if (this.ee.getShot() != null 
                    && this.ee.getShot().isShown() == true)
                {
                    this.ee.getShot().move(dt);
                    this.ee.getShot().updateMembers();
                }
            }
            if (this.ii != null)
            {
                this.ii.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y - Game.recalculate(50));
                this.ii.move(dt);
                this.ii.updateMembers();
            }
        }

        public Enemy getEnemy()
        {
            return this.ee;
        }

        public Item getItem()
        {
            return this.ii;
        }

        public int getSubType()
        {
            return this.subtype;
        }

        /*public void createFellowEnemy(StreamReader reader)
        {
            this.destroyFellowEnemy();
            int type;
            int tmp;
            in >> type;
            switch (type)
            {
                case 1:
                    in >> tmp;
                    this.ee = new EType1(tmp);
                    break;
                case 2: this.ee = new EType2(); break;
                case 3:	this.ee = new EType3(); break;
                case 4: this.ee = new EType4(); break;
                case 5: this.ee = new EType5(); break;
                case 7: this.ee = new EType7(); break;
                case 8:
                    in >> tmp;
                    this.ee = new EType8(tmp);
                    break;
                case 9: this.ee = new EType9(); break;
                case 10: this.ee = new EType10(); break;
            }
            this.ee.loadEnemy(in);
            this.ee.setType(type);
        }*/

        /*public void createFellowItem(QDataStream& in)
        {
            this.destroyFellowItem();
            int type;
            in >> type;
            switch (type)
            {
                case 0: this.ii = new IType0(); break;
                case 1: this.ii = new IType1(); break;
                case 2: this.ii = new IType2(); break;
                case 3: this.ii = new IType3(); break;
                case 4: this.ii = new IType4(); break;
                case 5: this.ii = new IType5(); break;
            }
            this.ii.loadItem(in);
            this.ii.setType(type);
        }*/

        public void destroyFellowEnemy()
        {
            this.ee = null;
        }

        public void destroyFellowItem()
        {
            this.ii = null;
        }
    }
}
