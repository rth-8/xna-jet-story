using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Ship : GameObject
    {
        private float damage; //stit
        private float fuel;  //palivo

        //zadni plamen: r-doprava, l-doleva
        private int flamebr;
        private int flamebl;
        //dolni plameny: b-siroky, s-uzky
        private int flamedb;
        private int flameds;

        //vektory pro pusobeni sil pri stisku klaves
        private Vector2 sNahoru;
        private Vector2 sDoleva;
        private Vector2 sDoprava;

        //bitmapy lode
        private Texture2D shipl;
        private Texture2D shipr;
        private Texture2D active_ship; //reference only

        //dolni plameny: bd-siroky, sd-uzky
        private Texture2D fbd1;
        private Texture2D fbd2;
        private Texture2D fsd1;
        private Texture2D fsd2;

        //zadni plameny: br-doprava, bl-doleva
        private Texture2D fbr1;
        private Texture2D fbr2;
        private Texture2D fbl1;
        private Texture2D fbl2;

        private Cannon can;

        private Special spc;

        //indikuje stisk klaves
        private bool bNahoru;
        private bool bDoleva;
        private bool bDoprava;

        private Maze maze; //not owned

        private bool colliding;

        private static int thrust = 120;

        public Ship(Maze maze) : base()
        {
            damage = fuel = 1000;
            flamebr = flamebl = flamedb = flameds = 0;
            bNahoru = bDoleva = bDoprava = false;
            this.can = new Cannon();
            this.spc = new Special();
            this.sDoleva = new Vector2();
            this.sDoprava = new Vector2();
            this.sNahoru = new Vector2();
            this.maze = maze;
        }

        public Ship(Maze maze, float m, float x, float y, float z, int w, int h)
            : base(m, x, y, w, h)
        {
            damage = fuel = 1000;
            flamebr = flamebl = flamedb = flameds = 0;
            bNahoru = bDoleva = bDoprava = false;
            this.can = new Cannon();
            this.spc = new Special();
            this.sDoleva = new Vector2();
            this.sDoprava = new Vector2();
            this.sNahoru = new Vector2();
            this.maze = maze;
        }

        public void Dispose()
        {
            //bitmapy lode
            shipl.Dispose();
            shipr.Dispose();

            //dolni plameny: bd-siroky, sd-uzky
            fbd1.Dispose();
            fbd2.Dispose();
            fsd1.Dispose();
            fsd2.Dispose();

            //zadni plameny: br-doprava, bl-doleva
            fbr1.Dispose();
            fbr2.Dispose();
            fbl1.Dispose();
            fbl2.Dispose();
        }

        public bool nactiBitmapy(ContentManager content)
        {
            //nacteni bitmap
            shipl = content.Load<Texture2D>("images/ship/ship_l");
            shipr = content.Load<Texture2D>("images/ship/ship_r");
            fbd1 = content.Load<Texture2D>("images/ship/flame_bd1");
            fbd2 = content.Load<Texture2D>("images/ship/flame_bd2");
            fsd1 = content.Load<Texture2D>("images/ship/flame_sd1");
            fsd2 = content.Load<Texture2D>("images/ship/flame_sd2");
            fbr1 = content.Load<Texture2D>("images/ship/flame_br1");
            fbr2 = content.Load<Texture2D>("images/ship/flame_br2");
            fbl1 = content.Load<Texture2D>("images/ship/flame_bl1");
            fbl2 = content.Load<Texture2D>("images/ship/flame_bl2");

            this.setDimension(shipl.Width, shipl.Height);
            this.resetOrient();
            return true;
        }

        public void resetOrient()
        {
            active_ship = shipr;
            this.can.setOrient(true);//doprava
        }

        public override void draw(SpriteBatch g)
        {
            //zobrazeni lode
            g.Draw(active_ship, this.position, Color.White);

            //zobrzeni plamenu
            if (bNahoru == true) {//pri pohybu nahoru
                if (active_ship == shipl) {//lod je otocena doleva
                    if (flameds == 1)
                        g.Draw(this.fsd1,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(20),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flameds == 2)
                        g.Draw(this.fsd2,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(20),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flamedb == 1)
                        g.Draw(this.fbd1,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(54),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flamedb == 2)
                        g.Draw(this.fbd2,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(54),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                }
                if (active_ship == shipr) {//lod je otocena doprava
                    if (flameds == 1)
                        g.Draw(this.fsd1,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(63),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flameds == 2)
                        g.Draw(this.fsd2,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(63),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flamedb == 1)
                        g.Draw(this.fbd1,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(16),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                    if (flamedb == 2)
                        g.Draw(this.fbd2,
                            new Vector2(
                                (int)getPosition().X + Game.recalculate(16),
                                (int)getPosition().Y + Game.recalculate(50)),
                            Color.White);
                }
            }
            if (bDoleva == true) {//pri pohybu doleva
                active_ship = shipl;//nastavi se bitmapa pro zobrazeni lode podle smeru pohybu
                //
                if (flamebl == 1)
                    g.Draw(this.fbl1,
                        new Vector2(
                            (int)getPosition().X + this.getW() + 1,
                            (int)getPosition().Y),
                        Color.White);
                if (flamebl == 2)
                    g.Draw(this.fbl2,
                        new Vector2(
                            (int)getPosition().X + this.getW() + 1,
                            (int)getPosition().Y),
                        Color.White);

            }
            if (bDoprava == true) {//pri pohybu doprava
                active_ship = shipr;//nastavi se bitmapa pro zobrazeni lode podle smeru pohybu
                //
                if (flamebr == 1)
                    g.Draw(this.fbr1,
                        new Vector2(
                                (int)getPosition().X-1-fbr1.Width,
                                (int)getPosition().Y
                                ),
                            Color.White);
                if (flamebr == 2)
                    g.Draw(this.fbr2,
                        new Vector2(
                                (int)getPosition().X - 1 - fbr2.Width,
                                (int)getPosition().Y
                                ),
                            Color.White);
            }
            if (active_ship == shipl) {
                if (this.can.isShown() == false) {
                    this.can.setOrient(false);//doleva
                }
                if (this.spc.isShown() == false) {
                    this.spc.setOrient(false);//doleva
                }
            }
            if (active_ship == shipr) {
                if (this.can.isShown() == false) {
                    this.can.setOrient(true);//doprava
                }
                if (this.spc.isShown() == false) {
                    this.spc.setOrient(true);//doprava
                }
            }
        }

        public override void move(float cas) {
            //System.out.println("> Ship.move");

            Vector2 grav = new Vector2(0, 40); //jako gravitace

            //pusobici sila se vynuluje
            this.initMass();

            //aplikace gravitace
            grav *= this.getM();
            this.applyForce(grav);

            //aplikace brzdnych sil
            if (this.getVelocity().X > 0)
            {
                this.applyForce(-100, 0);
            }
            if (this.getVelocity().X < 0)
            {
                this.applyForce(100, 0);
            }
            if (this.getVelocity().Y < 0)
            {
                this.applyForce(0, 100);
            }

            //pokud jsou stisknute klavesy, pusobi dalsi sily

            if (bNahoru == true)
            {
                this.applyForce(sNahoru);
                //ubyvani paliva
                this.fuel *= 0.99999f;
            }
            else
            {
                sNahoru.X = 0;//po pusteni klaves se vynuluji
                sNahoru.Y = 0;
            }

            if (bDoleva == true)
            {
                this.applyForce(sDoleva);
                //ubyvani paliva
                this.fuel *= 0.99999f;
            }
            else
            {
                sDoleva.X = 0;
                sDoleva.Y = 0;
            }
            if (bDoprava == true)
            {
                this.applyForce(sDoprava);
                //ubyvani paliva
                this.fuel *= 0.99999f;
            }
            else
            {
                sDoprava.X = 0;
                sDoprava.Y = 0;
            }

            //vypocet nove pozice podle rovnice s=v*t
            //Console.WriteLine("old pos = " + this.position.ToString());
            this.moveMassToNewPosition(cas);
            //Console.WriteLine("new pos = " + this.position.ToString());
            this.updateMembers();
        }

        public int getFlameBl()
        {
            return this.flamebl;
        }

        public int getFlameBr()
        {
            return this.flamebr;
        }

        public int getFlameDb()
        {
            return this.flamedb;
        }

        public int getFlameDs()
        {
            return this.flameds;
        }

        public void setFlameBl(int val)
        {
            this.flamebl = val;
        }

        public void setFlameBr(int val)
        {
            this.flamebr = val;
        }

        public void setFlameDb(int val)
        {
            this.flamedb = val;
        }

        public void setFlameDs(int val)
        {
            this.flameds = val;
        }

        public Vector2 getSDoleva()
        {
            return this.sDoleva;
        }

        public Vector2 getSDoprava()
        {
            return this.sDoprava;
        }

        public Vector2 getSNahoru()
        {
            return this.sNahoru;
        }

        public void setSDoleva(Vector2 v)
        {
            this.sDoleva = v;
        }

        public void setSDoprava(Vector2 v)
        {
            this.sDoprava = v;
        }

        public void setSNahoru(Vector2 v)
        {
            this.sNahoru = v;
        }

        public float getDamage()
        {
            return this.damage;
        }

        public float getFuel()
        {
            return this.fuel;
        }

        public void setDamage(float d)
        {
            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_SHIP_DAMAGE);

            this.damage = d;
        }

        public void setFuel(float f)
        {
            this.fuel = f;
        }

        public Cannon getCannon()
        {
            return this.can;
        }

        public Special getSpecial()
        {
            return this.spc;
        }

        public void setMovementUp(bool bUp)
        {
            this.bNahoru = bUp;
        }

        public void setMovementLeft(bool bLeft)
        {
            this.bDoleva = bLeft;
        }

        public void setMovementRight(bool bRight)
        {
            this.bDoprava = bRight;
        }

        public void doMovementBetweenRooms() {
            //opusteni obrazu
            if (this.getPosition().X < 0)
            {//doleva
                if (Maze.getMazeInstance().getActiveRoomY() == 0)
                {
                    setPosition(
                            getPosition().X + 1,
                            getPosition().Y);
                    setVelocity(0, getVelocity().Y);
                    return;
                }
                else
                {
                    //System.out.println("+ Ship.move - next room left");
                    Maze.getMazeInstance().changeActiveRoom(
                            Maze.getMazeInstance().getActiveRoomX(),
                            Maze.getMazeInstance().getActiveRoomY() - 1);
                }
                this.setPosition(
                        Game.RES_X-this.getW(),
                        this.getPosition().Y);
                //System.out.println("changed ship pos = " + this.getPosition().toString());
                this.can.hide();
                switch (this.spc.getType())
                {
                    case 0:
                    case 1:
                        this.spc.hide();
                        this.spc.setType(this.spc.getToChange());
                        break;
                    case 2:
                        this.spc.setPosition(this.getPosition());
                        break;
                }
            }
            if (this.getPosition().X + this.getW() > Game.RES_X)
            {//doprava
                if (Maze.getMazeInstance().getActiveRoomY() ==
                        Maze.getMazeInstance().getMazeS())
                {
                    setPosition(
                            getPosition().X - 1,
                            getPosition().Y);
                    setVelocity(0, getVelocity().X);
                    return;
                }
                else
                {
                    //System.out.println("+ Ship.move - next room right");
                    Maze.getMazeInstance().changeActiveRoom(
                            Maze.getMazeInstance().getActiveRoomX(),
                            Maze.getMazeInstance().getActiveRoomY() + 1);
                }
                this.setPosition(
                        0,
                        this.getPosition().Y);
                //System.out.println("changed ship pos = " + this.getPosition().toString());
                this.can.hide();
                switch (this.spc.getType()) {
                    case 0:
                    case 1:
                        this.spc.hide();
                        this.spc.setType(this.spc.getToChange());
                        break;
                    case 2:
                        this.spc.setPosition(this.getPosition());
                        break;
                }
            }
            //100 - hranice info panelu !
            if (this.getPosition().Y <
                    Game.recalculate(Game.STATUS_BAR_H)) {//nahoru
                //zmena mistnosti
                if (Maze.getMazeInstance().getActiveRoomX() == 0) {
                    setPosition(
                            getPosition().X,
                            getPosition().Y + 1);
                    setVelocity(getVelocity().X, 0);
                    return;
                }
                else {
                    //System.out.println("+ Ship.move - next room up");
                    Maze.getMazeInstance().changeActiveRoom(
                            Maze.getMazeInstance().getActiveRoomX() - 1,
                            Maze.getMazeInstance().getActiveRoomY());
                }
                this.setPosition(
                        this.getPosition().X,
                        Game.RES_Y - this.getH());
                //System.out.println("changed ship pos = " + this.getPosition().toString());
                this.can.hide();
                switch (this.spc.getType()) {
                    case 0:
                    case 1:
                        this.spc.hide();
                        this.spc.setType(this.spc.getToChange());
                        break;
                    case 2:
                        this.spc.setPosition(this.getPosition());
                        break;
                }
            }
            if (this.getPosition().Y + this.getH() > Game.RES_Y) {//dolu
                //zmena mistnosti
                if (Maze.getMazeInstance().getActiveRoomX() ==
                        Maze.getMazeInstance().getMazeR()) {
                    setPosition(
                            getPosition().X,
                            getPosition().Y - 1);
                    setVelocity(getVelocity().X, 0);
                    return;
                }
                else {
                    Console.WriteLine("+ Ship.move - next room down");
                    Maze.getMazeInstance().changeActiveRoom(
                            Maze.getMazeInstance().getActiveRoomX() + 1,
                            Maze.getMazeInstance().getActiveRoomY());
                }
                this.setPosition(
                        this.getPosition().X,
                        Game.recalculate(Game.STATUS_BAR_H) + 1
                        );
                //System.out.println("changed ship pos = " + this.getPosition().toString());
                this.can.hide();
                switch (this.spc.getType()) {
                    case 0:
                    case 1:
                        this.spc.hide();
                        this.spc.setType(this.spc.getToChange());
                        break;
                    case 2:
                        this.spc.setPosition(this.getPosition());
                        break;
                }
            }
        }

        /*public void storeShip(QDataStream& out)
        {
            //qDebug () << "> Ship.storeShip";

            //m
            out << this.getM(); //float
            //dimension
            out << this.getW(); //int
            out << this.getH(); //int
            //position
            out << this.getPosition().X; //float
            out << this.getPosition().Y; //float
            //velocity
            out << this.getVelocity().X; //float
            out << this.getVelocity().Y; //float
            //force
            out << this.getForce().X; //float
            out << this.getForce().Y; //float
            //damage
            out << this.getDamage(); //float
            //fuel
            out << this.getFuel(); //float
            //cannon
            out << this.can.getAmmo(); //int
            out << this.can.getOrient(); //bool
            //special
            out << this.spc.getType(); //int
            out << this.spc.getAmmo(); //int

            //qDebug() << "< Ship.storeShip";
        }*/

        /*public void loadShip(QDataStream& in)
        {
            //qDebug() << "> Ship.loadShip";

            //m
            float tm;
            in >> tm;
            this.setM(tm);
            //qDebug() << "+ Ship.loadShip - m = " << this.getM();
            //dimension
            int tw, th;
            in >> tw;
            in >> th;
            this.setDimension(tw, th);
            //qDebug() << "+ Ship.loadShip - w = " << this.getW();
            //qDebug() << "+ Ship.loadShip - h = " << this.getH();
            //position
            float tx, ty;
            in >> tx;
            in >> ty;
            this.setPosition(tx, ty);
            //qDebug() << "+ Ship.loadShip - x = " << this.getPosition().X;
            //qDebug() << "+ Ship.loadShip - y = " << this.getPosition().Y;
            //velocity
            float vx, vy;
            in >> vx;
            in >> vy;
            this.setVelocity(vx, vy);
            //qDebug() << "+ Ship.loadShip - vx = " << this.getVelocity().X;
            //qDebug() << "+ Ship.loadShip - vy = " << this.getVelocity().Y;
            //force
            float fx, fy;
            in >> fx;
            in >> fy;
            this.setForce(fx, fy);
            //qDebug() << "+ Ship.loadShip - fx = " << this.getForce().X;
            //qDebug() << "+ Ship.loadShip - fy = " << this.getForce().Y;
            //damage
            float td;
            in >> td;
            this.setDamage(td);
            //qDebug() << "+ Ship.loadShip - damage = " << this.getDamage();
            //fuel
            float tf;
            in >> tf;
            this.setFuel(tf);
            //qDebug() << "+ Ship.loadShip - fuel = " << this.getFuel();
            //cannon
            int ca;
            bool o;
            in >> ca;
            in >> o;
            this.can.setAmmo(ca);
            this.can.setOrient(o);
            //qDebug() << "+ Ship.loadShip - cannon ammo = " << this.getCannon()->getAmmo();
            //special
            int st;
            int sa;
            in >> st;
            in >> sa;
            this.spc.setType(st);
            this.spc.setAmmo(sa);
            //qDebug() << "+ Ship.loadShip - special type = " << this.getSpecial()->getType();
            //qDebug() << "+ Ship.loadShip - special ammo = " << this.getSpecial()->getAmmo();
            //reset weapons
            this.can.hide();
            this.spc.hide();

            //qDebug() << "< Ship.loadShip";
        }*/

        public void cycleDownFlames()
        {
            switch (this.getFlameDb())
            {
                case 0: this.setFlameDb(1); break;
                case 1: this.setFlameDb(2); break;
                case 2: this.setFlameDb(0); break;
            }
            switch (this.getFlameDs())
            {
                case 0: this.setFlameDs(1); break;
                case 1: this.setFlameDs(2); break;
                case 2: this.setFlameDs(0); break;
            }
        }

        public void cycleLeftFlame()
        {
            switch (this.getFlameBl())
            {
                case 0: this.setFlameBl(1); break;
                case 1: this.setFlameBl(2); break;
                case 2: this.setFlameBl(0); break;
            }
        }

        public void cycleRightFlame()
        {
            switch (this.getFlameBr())
            {
                case 0: this.setFlameBr(1); break;
                case 1: this.setFlameBr(2); break;
                case 2: this.setFlameBr(0); break;
            }
        }

        public void setColliding(bool value)
        {
            this.colliding = value;
        }

        public bool isColliding()
        {
            return this.colliding;
        }

        public void thrustUp()
        {
            if (this.getFuel() > 0)
            {
                this.setMovementUp(true);
                this.sNahoru.Y -= Ship.thrust;
                this.cycleDownFlames();
            }
        }

        public void stopUp()
        {
            this.setMovementUp(false);
            this.setFlameDb(0);
            this.setFlameDs(0);
        }

        public void thrustLeft()
        {
            if (this.getFuel() > 0)
            {
                this.setMovementLeft(true);
                this.sDoleva.X -= Ship.thrust;
                this.cycleLeftFlame();
            }
        }

        public void stopLeft()
        {
            this.setMovementLeft(false);
            this.setFlameBl(0);
        }

        public void thrustRight()
        {
            if (this.getFuel() > 0)
            {
                this.setMovementRight(true);
                this.sDoprava.X += Ship.thrust;
                this.cycleRightFlame();
            }
        }

        public void stopRight()
        {
            this.setMovementRight(false);
            this.setFlameBr(0);
        }

        public void launch()
        {
            if (this.getSpecial().getAmmo() > 0 && this.getSpecial().isShown() == false)
            {
                this.getSpecial().setAmmo(this.getSpecial().getAmmo() - 1);
                this.getSpecial().start(this.getPosition());
                this.getSpecial().show();
            }
        }

        public void shoot()
        {
            if (this.getCannon().getAmmo() > 0 && this.getCannon().isShown() == false)
            {
                this.getCannon().setAmmo(this.getCannon().getAmmo() - 1);
                this.getCannon().start(this.getPosition());
                this.getCannon().show();
            }
        }
    }
}
