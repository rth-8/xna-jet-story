using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace rth_jet_story
{
    class Mass
    {
        protected float m;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 force;

        public Mass()
        {
            this.m = 1;
            this.position = new Vector2();
            this.velocity = new Vector2();
            this.force = new Vector2();
        }

        public Mass(float m, float x, float y)
        {
            this.m = m;
            this.position = new Vector2(x, y);
            this.velocity = new Vector2();
            this.force = new Vector2();
        }

        public Mass(Mass old)
        {
            this.m = old.getM();
            this.position = old.getPosition();
            this.velocity = old.getVelocity();
            this.force = old.getForce();
        }

        public void setM(float m)
        {
            this.m = m;
        }

        public float getM()
        {
            return this.m;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public Vector2 getVelocity()
        {
            return this.velocity;
        }

        public Vector2 getForce()
        {
            return this.force;
        }

        public void setPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void setVelocity(float x, float y)
        {
            this.velocity.X = x;
            this.velocity.Y = y;
        }

        public void setVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void setForce(float x, float y)
        {
            this.force.X = x;
            this.force.Y = y;
        }

        public void setForce(Vector2 force)
        {
            this.force = force;
        }

        public void applyForce(float x, float y)
        {
            this.force.X += x;
            this.force.Y += y;
        }

        public void applyForce(Vector2 force)
        {
            this.force += force;
        }

        public void initMass()
        {
            this.force.X = 0;
            this.force.Y = 0;
        }

        public void moveMassToNewPosition(float dt)
        {
            this.velocity += (this.force / this.m) * dt;
            this.position += this.velocity * dt;
        }
    }
}
