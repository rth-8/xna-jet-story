using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rth_jet_story.game;

namespace rth_jet_story
{
    abstract class GameObject : Mass
    {
        protected int w;
        protected int h;

        protected Vector2 [] points;
        protected Vector2 [] edges;
        protected Vector2 center;

        public enum Collisions
        {
            NO_COLLISION    = 0,
            COLLISION_UP    = 1,
            COLLISION_DOWN  = 2,
            COLLISION_LEFT  = 3,
            COLLISION_RIGHT = 4
        };

        public const int POINTS = 4;
        public enum Points
        {
            TL = 0,
            TR = 1,
            BL = 2,
            BR = 3
        };

        public const int EDGES = 4;
        public enum Edges
        {
            TE = 0,
            RE = 1,
            BE = 2,
            LE = 3
        };

        public GameObject() : base()
        {
            this.w = 1;
            this.h = 1;

            this.points = new Vector2[GameObject.POINTS];
            this.points[(int)Points.TL] = this.position;
            this.points[(int)Points.TR] = new Vector2();
            this.points[(int)Points.BL] = new Vector2();
            this.points[(int)Points.BR] = new Vector2();

            this.edges = new Vector2[GameObject.EDGES];
            this.edges[(int)Edges.TE] = new Vector2();
            this.edges[(int)Edges.RE] = new Vector2();
            this.edges[(int)Edges.BE] = new Vector2();
            this.edges[(int)Edges.LE] = new Vector2();

            this.center = new Vector2();
        }

        public GameObject(float m, float x, float y, int w, int h) : base(m, x, y)
        {
            this.w = w;
            this.h = h;

            this.points = new Vector2[GameObject.POINTS];
            this.points[(int)Points.TL] = this.position;
            this.points[(int)Points.TR] = new Vector2();
            this.points[(int)Points.BL] = new Vector2();
            this.points[(int)Points.BR] = new Vector2();

            this.edges = new Vector2[GameObject.EDGES];
            this.edges[(int)Edges.TE] = new Vector2();
            this.edges[(int)Edges.RE] = new Vector2();
            this.edges[(int)Edges.BE] = new Vector2();
            this.edges[(int)Edges.LE] = new Vector2();

            this.center = new Vector2();

            this.updateMembers();
        }

        

        //             TOP(0)
        //      TL(0)-------TR(1)
        //         ^           |
        // LEFT(3) |           | RIGHT(1)
        //         |           v
        //      BL(3)<--------BR(2)
        //            BOTTOM(2)

        public void updateMembers()
        {
            this.points[(int)Points.TL] = this.position;
            this.points[(int)Points.TR].X = this.position.X + this.w;
            this.points[(int)Points.TR].Y = this.position.Y;

            this.points[(int)Points.BL].X = this.position.X;
            this.points[(int)Points.BL].Y = this.position.Y + this.h;

            this.points[(int)Points.BR].X = this.position.X + this.w;
            this.points[(int)Points.BR].Y = this.position.Y + this.h;

            this.edges[(int)Edges.TE] = this.points[(int)Points.TR] - this.points[(int)Points.TL];
            this.edges[(int)Edges.RE] = this.points[(int)Points.BR] - this.points[(int)Points.TR];
            this.edges[(int)Edges.BE] = this.points[(int)Points.BL] - this.points[(int)Points.BR];
            this.edges[(int)Edges.LE] = this.points[(int)Points.TL] - this.points[(int)Points.BL];


            this.center.X = this.position.X + this.w / 2;
            this.center.Y = this.position.Y + this.h / 2;
        }

        public Vector2 getPoint(int point)
        {
            return this.points[point];
        }

        public Vector2 getEdge(int edge)
        {
            return this.edges[edge];
        }

        public Vector2 getCenter()
        {
            return this.center;
        }

        public void setDimension(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public int getW()
        {
            return this.w;
        }

        public int getH()
        {
            return this.h;
        }

        public bool contains(Vector2 p)
        {
            if (p.X >= this.points[(int)Points.TL].X
                && p.X <= this.points[(int)Points.TR].X
                && p.Y >= this.points[(int)Points.TL].Y
                && p.Y <= this.points[(int)Points.BL].Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Method evaluates if this object has collided with another object.
         * @param o
         * @return
         */
        public int collision(GameObject o)
        {
            float r1x1 = this.position.X;
            float r1y1 = this.position.Y;
            float r1x2 = this.position.X + this.getW();
            float r1y2 = this.position.Y + this.getH();

            float r2x1 = o.getPosition().X;
            float r2y1 = o.getPosition().Y;
            float r2x2 = o.getPosition().X + o.getW();
            float r2y2 = o.getPosition().Y + o.getH();

            float x1 = Math.Max(r1x1, r2x1);
            float x2 = Math.Min(r1x2, r2x2);
            float y1 = Math.Max(r1y1, r2y1);
            float y2 = Math.Min(r1y2, r2y2);

            if ((x1 >= x2) || (y1 >= y2))
            {
                return (int)Collisions.NO_COLLISION;
            }
            else {
                float w = Math.Abs(x2 - x1);
                float h = Math.Abs(y2 - y1);
                if (w > h) {
                    //up or down
                    if (this.center.Y < o.getCenter().Y) {
                        //down
                        return (int)Collisions.COLLISION_DOWN;
                    }
                    else {
                        //up
                        return (int)Collisions.COLLISION_UP;
                    }
                }
                else {
                    //left or right
                    if (this.center.X < o.getCenter().X) {
                        //right
                        return (int)Collisions.COLLISION_RIGHT;
                    }
                    else {
                        //left
                        return (int)Collisions.COLLISION_LEFT;
                    }
                }
            }
        }

        /**
         * Method moves object (this) to proper position after collision with another
         * object (refObject) in given direction.
         * @param refObject
         * @param direction
         */
        public void moveObjectToNewPosition(GameObject refObject, int direction)
        {
            switch (direction)
            {
                case (int)Collisions.COLLISION_DOWN:
                {
                    float y1 = this.getPosition().Y + this.getH();
                    float y2 = refObject.getPosition().Y;
                    float dy = Math.Abs(y1 - y2);
                    this.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y - dy - 1);
                    break;
                }
                case (int)Collisions.COLLISION_UP:
                {
                    float y1 = this.getPosition().Y;
                    float y2 = refObject.getPosition().Y + refObject.getH();
                    float dy = Math.Abs(y1 - y2);
                    this.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y + dy + 1);
                    break;
                }
                case (int)Collisions.COLLISION_LEFT:
                {
                    float x1 = this.getPosition().X;
                    float x2 = refObject.getPosition().X + refObject.getW();
                    float dx = Math.Abs(x1 - x2);
                    this.setPosition(
                            this.getPosition().X + dx + 1,
                            this.getPosition().Y);
                    break;
                }
                case (int)Collisions.COLLISION_RIGHT:
                {
                    float x1 = this.getPosition().X + this.getW();
                    float x2 = refObject.getPosition().X;
                    float dx = Math.Abs(x1 - x2);
                    this.setPosition(
                            this.getPosition().X - dx - 1,
                            this.getPosition().Y);
                    break;
                }
            }//switch
        }

        /**
         * Method covers special case of collision: Enemy or Item carried by
         * EType20 has collided with another object. Move distance is calculated
         * for this Enemy or Item, but actual moving object has to be EType20 (this).
         * @param collidingObject Enemy or Item carried by EType20 enemy
         * @param refObject Object that Enemy or Item collided with
         * @param direction direction of collision
         */
        public void moveObjectToNewPosition(
                GameObject collidingObject, GameObject refObject, int direction)
        {
            switch (direction)
            {
                case (int)Collisions.COLLISION_DOWN:
                {
                    float y1 = collidingObject.getPosition().Y + this.getH();
                    float y2 = refObject.getPosition().Y;
                    float dy = Math.Abs(y1 - y2);
                    this.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y - dy - 1);
                    break;
                }
                case (int)Collisions.COLLISION_UP:
                {
                    float y1 = collidingObject.getPosition().Y;
                    float y2 = refObject.getPosition().Y + refObject.getH();
                    float dy = Math.Abs(y1 - y2);
                    this.setPosition(
                        this.getPosition().X,
                        this.getPosition().Y + dy + 1);
                    break;
                }
                case (int)Collisions.COLLISION_LEFT:
                {
                    float x1 = collidingObject.getPosition().X;
                    float x2 = refObject.getPosition().X + refObject.getW();
                    float dx = Math.Abs(x1 - x2);
                    this.setPosition(
                            this.getPosition().X + dx + 1,
                            this.getPosition().Y);
                    break;
                }
                case (int)Collisions.COLLISION_RIGHT:
                {
                    float x1 = collidingObject.getPosition().X + this.getW();
                    float x2 = refObject.getPosition().X;
                    float dx = Math.Abs(x1 - x2);
                    this.setPosition(
                            this.getPosition().X - dx - 1,
                            this.getPosition().Y);
                    break;
                }
            }//switch
        }

        public int pointOfCollision(GameObject o, Vector2 p, ref Vector2 result)
        {
            //   TL     |     TR
            //    +-----|-----+
            //    | -,- | +,- |
            // ---------+---------
            //    | -,+ | +,+ |
            //    +-----|-----+
            //   BL     |     BR

            Vector2 point = new Vector2();
            bool pointset = false;
            Vector2 edge1 = new Vector2();
            bool edge1set = false;
            Vector2 edge2 = new Vector2();
            bool edge2set = false;
            int e1 = -1;
            int e2 = -1;

            // -,-
            if (o.getVelocity().X < 0 && o.getVelocity().Y < 0)
            {
                point = this.points[(int)Points.BR];
                edge1 = this.edges[(int)Edges.RE];
                edge2 = this.edges[(int)Edges.BE];
                e1 = (int)Edges.RE;
                e2 = (int)Edges.BE;
                pointset = true;
                edge1set = true;
                edge2set = true;
            }

            // +,-
            if (o.getVelocity().X > 0 && o.getVelocity().Y < 0)
            {
                point = this.points[(int)Points.BL];
                edge1 = this.edges[(int)Edges.LE];
                edge2 = this.edges[(int)Edges.BE];
                e1 = (int)Edges.LE;
                e2 = (int)Edges.BE;
                pointset = true;
                edge1set = true;
                edge2set = true;
            }

            // -,+
            if (o.getVelocity().X < 0 && o.getVelocity().Y > 0)
            {
                point = this.points[(int)Points.TR];
                edge1 = this.edges[(int)Edges.RE];
                edge2 = this.edges[(int)Edges.TE];
                e1 = (int)Edges.RE;
                e2 = (int)Edges.TE;
                pointset = true;
                edge1set = true;
                edge2set = true;
            }

            // +,+
            if (o.getVelocity().X > 0 && o.getVelocity().Y > 0)
            {
                point = this.points[(int)Points.TL];
                edge1 = this.edges[(int)Edges.LE];
                edge2 = this.edges[(int)Edges.TE];
                e1 = (int)Edges.LE;
                e2 = (int)Edges.TE;
                pointset = true;
                edge1set = true;
                edge2set = true;
            }

            // nahoru
            if (o.getVelocity().X == 0 && o.getVelocity().Y < 0)
            {
                point = this.points[(int)Points.BR];
                edge1 = this.edges[(int)Edges.BE];
                e1 = (int)Edges.BE;
                pointset = true;
                edge1set = true;
            }

            // dolu
            if (o.getVelocity().X == 0 && o.getVelocity().Y > 0)
            {
                point = this.points[(int)Points.TR];
                edge1 = this.edges[(int)Edges.TE];
                e1 = (int)Edges.TE;
                pointset = true;
                edge1set = true;
            }

            // doleva
            if (o.getVelocity().X < 0 && o.getVelocity().Y == 0)
            {
                point = this.points[(int)Points.BR];
                edge1 = this.edges[(int)Edges.RE];
                e1 = (int)Edges.RE;
                pointset = true;
                edge1set = true;
            }

            // doprava
            if (o.getVelocity().X > 0 && o.getVelocity().Y == 0)
            {
                point = this.points[(int)Points.TL];
                edge1 = this.edges[(int)Edges.LE];
                e1 = (int)Edges.LE;
                pointset = true;
                edge1set = true;
            }

            Vector2 p1 = new Vector2();
            bool p1set = false;
            Vector2 p2 = new Vector2();
            bool p2set = false;
            if (pointset && edge1set)
            {
                p1set = VectorMath.pointOfIntersection_2Lines2D(
                        point,
                        edge1,
                        p,
                        o.getVelocity(),
                        ref p1
                    );
            }
            if (pointset && edge2set)
            {
                p2set = VectorMath.pointOfIntersection_2Lines2D(
                        point,
                        edge2,
                        p,
                        o.getVelocity(),
                        ref p2
                    );
            }

            if (p1set && this.contains(p1) == true && 
                p2set && this.contains(p2) == false)
            {
                result = p1;
                return e1;
            }
            else 
            if (p2set && this.contains(p2) == true && 
                p1set && this.contains(p1) == false)
            {
                result = p2;
                return e2;
            }
            else
            {
                return -1;
            }
        }

        private float angleBetween2Vectors2D(Vector2 p1, Vector2 p2)
        {
            float a = Vector2.Dot(p1, p2);
            float b = p1.Length() * p2.Length();
            return (a/b);
        }

        public bool doCollisionAndBounce(List<Wall> objects, int arraylength, int decelerationFactor)
        {
            this.updateMembers();

            List<GameObject> po = new List<GameObject>();
            List<Vector2> pv = new List<Vector2>();

            bool tladded = false;
            bool tradded = false;
            bool bladded = false;
            bool bradded = false;

            for (int i=0; i<arraylength; i++)
            {
                if (objects[i].contains(this.getPoint((int)Points.TL)) == true)
                {
                    po.Add(objects[i]);
                    if (tladded == false) 
                    {
                        pv.Add(this.getPoint((int)Points.TL));
                    }
                }
                if (objects[i].contains(this.getPoint((int)Points.TR)) == true)
                {
                    po.Add(objects[i]);
                    if (tradded == false) 
                    {
                        pv.Add(this.getPoint((int)Points.TR));
                    }
                }
                if (objects[i].contains(this.getPoint((int)Points.BL)) == true)
                {
                    po.Add(objects[i]);
                    if (bladded == false) 
                    {
                        pv.Add(this.getPoint((int)Points.BL));
                    }
                }
                if (objects[i].contains(this.getPoint((int)Points.BR)) == true)
                {
                    po.Add(objects[i]);
                    if (bradded == false) 
                    {
                        pv.Add(this.getPoint((int)Points.BR));
                    }
                }
            }

            if (po.Count == 0) 
            {
                return false;
            }

            // merena vzdalenost mezi referencnim bodem a nalezenym prusecikem
            float length = float.MinValue;
            Vector2 p1 = new Vector2(); //prusecik
            Vector2 p2 = new Vector2(); //referencni bod
            int edge = -1; //typ hrany, na ktere lezi prusecik

            //pro kazdy vrchol, ktery kolidoval
            for (int i=0; i<pv.Count; i++)
            {
                Vector2 tv = pv[i];
                //pro kazdy objekt, se kterym nastala kolize
                for (int j=0; j<po.Count; j++)
                {
                    GameObject to = po[j];
                    //vypocte se prusecik mezi referencnim bodem a objektem
                    Vector2 p = new Vector2(); //vypocteny prusecik
                    int e = to.pointOfCollision(this, tv, ref p);
                    if (e != -1)
                    {
                        //Pokud je nalezen, vypocteme vzdalenost mezi ref. bodem a
                        //nalezenym prusecikem
                        //float refL = distanceBetween2Points2D(p, tv);
                        float refL = Vector2.Distance(p, tv);
                        if (refL > length)
                        {
                            //Pokud je vzdalenost nejvyssi nalezena, zjisti se,
                            //zda prusecik lezi ve spravnem smeru (presne opacnem)
                            //vuci vektoru rychlosti
                            //Vector2 refV = line2D(tv, p);
                            Vector2 refV = p - tv;
                            float u = angleBetween2Vectors2D(this.getVelocity(), refV);
                            if (u < -0.98 && u >= -1.1) //zhruba 180%
                            {
                                length = refL;
                                p1 = new Vector2(p.X, p.Y);
                                p2 = tv;
                                edge = e;
                            }
                        }
                        else if (refL == length)
                        {
                            //???
                        }
                    }
                }
            }

            if (length > float.MinValue)
            {
                //nejaky prusecik byl skutecne nalezen
                //...

                //Vypocteme posunuti, kterym se musis posunout kolidujici objekt
                //do takove pozice, aby uz nekolidoval. Pouziji se k tomu oba
                //nalezene body: referencni bod (p2) a prusecik s hranou (p1)
                int dx = Math.Abs((int)p2.X - (int)p1.X) + 1;
                int dy = Math.Abs((int)p2.Y - (int)p1.Y) + 1;

                //Posunuti se jeste musis nastavit tak, aby probehlo ve spravnem smeru.
                //Spravny smer se urci podle smeru stavajiciho vektoru rychlost
                // -,-
                if (this.getVelocity().X < 0 && this.getVelocity().Y < 0)
                {
                }
                // +,-
                if (this.getVelocity().X > 0 && this.getVelocity().Y < 0)
                {
                    dx *= -1;
                }
                // -,+
                if (this.getVelocity().X < 0 && this.getVelocity().Y > 0)
                {
                    dy *= -1;
                }
                // +,+
                if (this.getVelocity().X > 0 && this.getVelocity().Y > 0)
                {
                    dx *= -1;
                    dy *= -1;
                }
                // nahoru
                if (this.getVelocity().X == 0 && this.getVelocity().Y < 0)
                {

                }
                // dolu
                if (this.getVelocity().X == 0 && this.getVelocity().Y > 0)
                {
                    dy *= -1; //nahoru
                }
                // doleva
                if (this.getVelocity().X < 0 && this.getVelocity().Y == 0)
                {

                }
                // doprava
                if (this.getVelocity().X > 0 && this.getVelocity().Y == 0)
                {
                    dx *= -1; //doleva
                }

                //Kolidujici objekt se posune do nekolizni pozice
                this.setPosition(
                        this.getPosition().X + dx,
                        this.getPosition().Y + dy);
                this.updateMembers();

                //Dochazi k odrazu: podle toho, na ktere hrane lezi nalezeny prusecik,
                //dojde k obraceni prislusne slozky vektoru rychlosti
                switch (edge)
                {
                    case (int)Edges.TE:
                    case (int)Edges.BE:
                        this.setVelocity(
                                this.getVelocity().X,
                                -this.getVelocity().Y * decelerationFactor);
                        break;
                    case (int)Edges.LE:
                    case (int)Edges.RE:
                        this.setVelocity(
                                -this.getVelocity().X * decelerationFactor,
                                this.getVelocity().Y);
                        break;
                }
                return true;
            }
            return false;
        }

        public abstract void draw(SpriteBatch g);

        public abstract void move(float dt);

    }//class
}//namespace
