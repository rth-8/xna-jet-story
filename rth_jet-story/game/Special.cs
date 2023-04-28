using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Special : Shot
    {
        public enum Type
        {
            SPECIAL_SIDE = 0,
            SPECIAL_DOWN = 1,
            SPECIAL_SPHERE = 2
        };
        const int SPHERE_DURATION = 700;

        private int type;
        private int toChange;

        public static Texture2D left;
        public static Texture2D right;
        public static Texture2D down;
        public static Texture2D circle;

        public static void loadImages(ContentManager content)
        {
            left = content.Load<Texture2D>("images/ship/special_left");
            right = content.Load<Texture2D>("images/ship/special_right");
            down = content.Load<Texture2D>("images/ship/special_down");
            circle = content.Load<Texture2D>("images/ship/special_circle");
        }

        public static void disposeImages()
        {
            left.Dispose();
            right.Dispose();
            down.Dispose();
            circle.Dispose();
        }

        public Special() : base()
        {
            this.toChange = -1;
        }

        public Special(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, z, w, h)
        {
            this.toChange = -1;
        }

        public override void start(Vector2 st)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_SPECIAL_LAUNCH);

            switch (this.type) {

                case (int)Type.SPECIAL_SIDE: //raketka do strany
                    if (this.getOrient() == false)
                    {//doleva
                        this.setPosition(
                                st.X + Game.recalculate(50-20),
                                st.Y + Game.recalculate(40));
                        this.setVelocity(-200, 200);
                    }
                    if (this.getOrient() == true)
                    {//doprava
                        this.setPosition(
                                st.X + Game.recalculate(50),
                                st.Y + Game.recalculate(40));
                        this.setVelocity(200, 200);
                    }
                    this.setDimension(left.Width, left.Height);
                    break;

                case (int)Type.SPECIAL_DOWN: //raketka dolu
                    if (this.getOrient() == false)
                    {//doleva
                        this.setPosition(
                                st.X + Game.recalculate(50-20),
                                st.Y + Game.recalculate(40));
                        this.setVelocity(0, 200);
                    }
                    if (this.getOrient() == true)
                    {//doprava
                        this.setPosition(
                                st.X + Game.recalculate(50),
                                st.Y + Game.recalculate(40));
                        this.setVelocity(0, 200);
                    }
                    this.setDimension(down.Width, down.Height);
                    break;

                case (int)Type.SPECIAL_SPHERE: //kulicka
                    if (this.getOrient() == false)
                    {//doleva
                        this.setPosition(
                                st.X + Game.recalculate(50-20),
                                st.Y + Game.recalculate(25));
                        this.setVelocity(-200, 250);
                    }
                    if (this.getOrient() == true)
                    {//doprava
                        this.setPosition(
                                st.X + Game.recalculate(50),
                                st.Y + Game.recalculate(25));
                        this.setVelocity(200, 250);
                    }
                    this.setDimension(circle.Width, circle.Height);
                    this.citac = 0;
                    break;
            }
        }

        public override void start(Vector2 o, Vector2 st)
        {
        }

        public override void draw(SpriteBatch g)
        {
            switch (this.type) {

                case (int)Type.SPECIAL_SIDE: //raketka do strany
                    if (this.getOrient() == false) { 
                        //doleva
                        g.Draw(left, new Vector2(position.X, position.Y), Color.Cyan);
                    }
                    if (this.getOrient() == true) { 
                        //doprava
                        g.Draw(right, new Vector2(position.X, position.Y), Color.Cyan);
                    }
                    break;

                case (int)Type.SPECIAL_DOWN: //raketka dolu
                    g.Draw(down, new Vector2(position.X, position.Y), Color.Cyan);
                    break;

                case (int)Type.SPECIAL_SPHERE: //kulicka
                    g.Draw(circle, new Vector2(position.X, position.Y), Color.White);
                    break;
            }
        }

        public override void move(float dt)
        {
            this.initMass();

            switch (this.type)
            {
                case (int)Type.SPECIAL_SIDE: //raketka do strany
                    if (this.getOrient() == false)
                    {//doleva
                        if (this.getVelocity().Y < -1 || this.getVelocity().Y > 1)
                        {
                            this.applyForce(0, -100);
                        }
                        this.applyForce(-100, 0);
                    }
                    if (this.getOrient() == true)
                    {//doprava
                        if (this.getVelocity().Y < -1 || this.getVelocity().Y > 1)
                        {
                            this.applyForce(0, -100);
                        }
                        this.applyForce(100, 0);
                    }
                    break;

                case (int)Type.SPECIAL_DOWN: //raketka dolu
                    this.applyForce(0, 150);
                    break;

                case (int)Type.SPECIAL_SPHERE: //kulicka
                    this.citac++;
                    //sphere exhaustion :)
                    this.velocity -= this.velocity / 1000;
                    break;
            }

            switch (this.type)
            {
                case (int)Type.SPECIAL_SIDE:
                case (int)Type.SPECIAL_DOWN:
                    if (this.getPosition().X < 0)
                    {
                        this.hide();
                        this.type = this.toChange;
                    }
                    if (this.getPosition().X + this.getW() > Game.RES_X)
                    {
                        this.hide();
                        this.type = this.toChange;
                    }
                    if (this.getPosition().Y <
                            Game.recalculate(Game.STATUS_BAR_H))
                    {
                        this.hide();
                        this.type = this.toChange;
                    }
                    if (this.getPosition().Y + this.getH() > Game.RES_Y)
                    {
                        this.hide();
                        this.type = this.toChange;
                    }
                    break;

                case (int)Type.SPECIAL_SPHERE: //kulicka
                    if (this.getPosition().X < 0)
                    {
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CIRLCE_BOUNCE);
                        this.setPosition(0, this.getPosition().Y);
                        this.velocity.X = -this.velocity.X;
                    }
                    if (this.getPosition().X + this.getW() > Game.RES_X)
                    {
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CIRLCE_BOUNCE);
                        this.setPosition(
                                Game.RES_X - this.getW(), this.getPosition().Y);
                        this.velocity.X = -this.velocity.X;
                    }
                    if (this.getPosition().Y < Game.recalculate(Game.STATUS_BAR_H))
                    {
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CIRLCE_BOUNCE);
                        this.setPosition(this.getPosition().X,
                                Game.recalculate(Game.STATUS_BAR_H));
                        this.velocity.Y = -this.velocity.Y;
                    }
                    if (this.getPosition().Y + this.getH() > Game.RES_Y)
                    {
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CIRLCE_BOUNCE);
                        this.setPosition(
                                this.getPosition().X, Game.RES_Y - this.getH());
                        this.velocity.Y = -this.velocity.Y;
                    }
                    if (this.citac == SPHERE_DURATION)
                    {
                        this.hide();
                        this.type = this.toChange;
                    }
                    break;
            }
            this.moveMassToNewPosition(dt);
        }

        public int getType()
        {
            return this.type;
        }

        public void setType(int t)
        {
            if (this.toChange == -1)
            {
                this.toChange = t;
            }
            if (this.isShown() == false)
            {
                this.type = t;
                this.toChange = t;
            }
            else
            {
                this.toChange = t;
            }
        }

        public int getToChange()
        {
            return this.toChange;
        }

    }
}
