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
    class Room
    {
        private int bases;

        private List<Wall> walls;
        private List<Enemy> enemies;
        private Enemy[] enemies10;
        private List<Item> items;

        private int pocet_w;
        private int pocet_e;
        private int pocet_i;
        private int pocet_e10;

        public const int E10 = 5;

        public Room()
        {
            this.bases = 0;
            this.pocet_e = 0;
            this.pocet_w = 0;
            this.pocet_i = 0;
            this.pocet_e10 = 0;

            walls = new List<Wall>();
            this.enemies = new List<Enemy>();
            this.items = new List<Item>();

            enemies10 = new Enemy[E10];
            for (int i=0; i<E10; i++)
            {
                enemies10[i] = new Enemy();
                enemies10[i].setDamage(-1);
            }
        }

        public void Dispose()
        {
            this.walls.Clear();
            this.enemies.Clear();
            this.items.Clear();
        }

        public bool loadWallsFromFile(string s)
        {
            FileStream file = new FileStream(s, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            
            //wall count
            this.pocet_w = Int32.Parse(reader.ReadLine());

            for (int i=0; i<pocet_w; i++)
            {
                int t = 0, x = 0, y = 0;

                reader.ReadLine();	// ';', ktery oddeluje zdi v def souboru

                t = Int32.Parse(reader.ReadLine());

                x = Int32.Parse(reader.ReadLine());
                x = Game.recalculate(x);

                y = Int32.Parse(reader.ReadLine());
                y = Game.recalculate(y);
                y += Game.recalculate(Game.STATUS_BAR_H);

                Wall wall = new Wall(1, x, y, 0, 1, 1);
                switch (t)
                {
                    case 0: wall.setBitmap((int)Wall.Ids.WALL_00); break;
                    case 1: wall.setBitmap((int)Wall.Ids.WALL_01); break;
                    case 2: wall.setBitmap((int)Wall.Ids.WALL_02); break;
                    case 3: wall.setBitmap((int)Wall.Ids.WALL_03); break;
                    case 4: wall.setBitmap((int)Wall.Ids.WALL_04); break;
                    case 5: wall.setBitmap((int)Wall.Ids.WALL_05); break;
                    case 6: wall.setBitmap((int)Wall.Ids.WALL_06); break;
                    case 7: wall.setBitmap((int)Wall.Ids.WALL_07); break;
                    case 8: wall.setBitmap((int)Wall.Ids.WALL_08); break;
                    case 9: wall.setBitmap((int)Wall.Ids.WALL_09); break;
                    case 10: wall.setBitmap((int)Wall.Ids.WALL_10); break;
                    case 11: wall.setBitmap((int)Wall.Ids.WALL_11); break;
                    case 12: wall.setBitmap((int)Wall.Ids.WALL_12); break;
                    case 13: wall.setBitmap((int)Wall.Ids.WALL_13); break;
                    case 14: wall.setBitmap((int)Wall.Ids.WALL_14); break;
                    case 15: wall.setBitmap((int)Wall.Ids.WALL_15); break;
                    case 16: wall.setBitmap((int)Wall.Ids.WALL_16); break;
                    case 17: wall.setBitmap((int)Wall.Ids.WALL_17); break;
                    case 18: wall.setBitmap((int)Wall.Ids.WALL_18); break;
                    case 19: wall.setBitmap((int)Wall.Ids.WALL_19); break;
                    case 20: wall.setBitmap((int)Wall.Ids.WALL_20); break;
                    case 21: wall.setBitmap((int)Wall.Ids.WALL_21); break;
                    case 22: wall.setBitmap((int)Wall.Ids.WALL_22); break;
                    case 23: wall.setBitmap((int)Wall.Ids.WALL_23); break;
                    case 24: wall.setBitmap((int)Wall.Ids.WALL_24); break;
                    case 25: wall.setBitmap((int)Wall.Ids.WALL_25); break;
                    case 26: wall.setBitmap((int)Wall.Ids.WALL_26); break;
                    case 27: wall.setBitmap((int)Wall.Ids.WALL_27); break;
                    case 28: wall.setBitmap((int)Wall.Ids.WALL_28); break;
                    case 29: wall.setBitmap((int)Wall.Ids.WALL_29); break;
                }
                wall.updateMembers(); //!

                this.walls.Add(wall);
            }//for

            return true;
        }

        public bool loadEnemiesFromFile(string s)
        {
            FileStream file = new FileStream(s, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            pocet_e = Int32.Parse(reader.ReadLine());

            if (pocet_e == 0)
            {
                return true;
            }

            for (int i=0; i<pocet_e; i++)
            {
                int ex = 0, ey = 0, type = 0, subtype = 0;

                reader.ReadLine(); //;

                ex = Int32.Parse(reader.ReadLine());
                ex = Game.recalculate(ex);
                //qDebug() << "enemy x = " << ex;

                ey = Int32.Parse(reader.ReadLine());
                ey = Game.recalculate(ey);
                ey += Game.recalculate(Game.STATUS_BAR_H);

                type = Int32.Parse(reader.ReadLine());

                subtype = Int32.Parse(reader.ReadLine());

                Enemy enemy;
                switch(type)
                {
                    case 0://base
                        enemy = new EType0(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(200);
                        bases++;
                        break;
                    case 1://neco jako talirovita antena co strili mikrovlny :)
                        enemy = new EType1(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50),
                                subtype);
                        enemy.setDamage(60);
                        break;
                    case 2://kulovita antenka co strili kulicky za lodi
                        enemy = new EType2(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(90);
                        break;
                    case 3://asi tank ???
                        enemy = new EType3(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(30);
                        break;
                    case 4://ctverecek
                        enemy = new EType4(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(200);
                        break;
                    case 5://raketove silo :D
                        enemy = new EType5(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(90);
                        break;
                    case 6://vymetnice raket smerem dolu
                        enemy = new EType6(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(90);
                        break;
                    case 7://takova placata vec s necim co se toci a strili shluky castecek
                        enemy = new EType7(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(46));
                        enemy.setDamage(90);
                        break;
                    case 8://odpalovaci rampa s raketou
                        enemy = new EType8(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50),subtype);
                        enemy.setDamage(20);
                        break;
                    case 9://strela patriot :)
                        enemy = new EType9(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(90);
                        break;
                    case 10://vypostec dynamickych nepratel
                        enemy = new EType10(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(90);
                        break;
                    case 11:
                        enemy = new EType11(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 12:
                        enemy = new EType12(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 13:
                        enemy = new EType13(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 14:
                        enemy = new EType14(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 15:
                        enemy = new EType15(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 16://balonek
                        enemy = new EType16(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 17:
                        enemy = new EType17(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        enemy.setDamage(10);
                        break;
                    case 18://velka koula
                        enemy = new EType18(1,ex,ey,0,
                                Game.recalculate(100),
                                Game.recalculate(100));
                        enemy.setDamage(50);
                        break;
                    case 19:
                        enemy = new EType19(1,ex,ey,0,
                                Game.recalculate(96),
                                Game.recalculate(50));
                        enemy.setDamage(50);
                        break;
                    case 20://nosic
                        enemy = new EType20(1,ex,ey,0,
                                Game.recalculate(50),
                                Game.recalculate(50),subtype);
                        enemy.setDamage(10);
                        //souputnik:
                        int tt, ttt, st; //0-enemy, 1-item
                        reader.ReadLine(); //;
                        tt = Int32.Parse(reader.ReadLine());
                        ex = Int32.Parse(reader.ReadLine());
                        ex = Game.recalculate(ex);
                        ey = Int32.Parse(reader.ReadLine());
                        ey = Game.recalculate(ey);
                        ey += Game.recalculate(Game.STATUS_BAR_H);
                        ttt = Int32.Parse(reader.ReadLine());
                        st = Int32.Parse(reader.ReadLine());
                        if (tt == 0)
                        {
                            ((EType20)enemy).createFellowEnemy(1,ex,ey,0,
                                    Game.recalculate(50),
                                    Game.recalculate(50),ttt,st);
                        }
                        else if (tt == 1)
                        {
                            ((EType20)enemy).createFellowItem(1,ex,ey,0,
                                    Game.recalculate(50),
                                    Game.recalculate(50),ttt);
                        }
                        break;
                    default:
                        enemy = new Enemy(); 
                        break;
                }//switch

                enemy.setType(type);
                this.enemies.Add(enemy);

            }//for
            
            return true;
        }

        public bool loadItemsFromFile(string s)
        {
            //qDebug() << "> Room.loadItemsFromFile( " << s << " )";

            FileStream file = new FileStream(s, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            pocet_i = Int32.Parse(reader.ReadLine());

            if (pocet_i == 0)
            {
                return true;
            }

            for (int i=0; i<pocet_i; i++)
            {
                int ix = 0, iy = 0, type = 0;

                reader.ReadLine(); //;

                ix = Int32.Parse(reader.ReadLine());
                ix = Game.recalculate(ix);

                iy = Int32.Parse(reader.ReadLine());
                iy = Game.recalculate(iy);
                iy += Game.recalculate(Game.STATUS_BAR_H);

                type = Int32.Parse(reader.ReadLine());

                Item item;
                switch(type)
                {
                    case 0://ammo
                        item = new IType0(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    case 1://ball
                        item = new IType1(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    case 2://fuel
                        item = new IType2(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    case 3://missiles down
                        item = new IType3(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    case 4://missiles left-right
                        item = new IType4(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    case 5://shield
                        item = new IType5(1,ix,iy,0,
                                Game.recalculate(50),
                                Game.recalculate(50));
                        break;
                    default:
                        item = new Item();
                        break;
                }//switch

                item.setType(type);
                this.items.Add(item);

            }//for

            return true;
        }

        public void draw(SpriteBatch g)
        {
            //qDebug() << "> Room.draw";

            //kresli zdi
            //qDebug() << "+ Room.draw - walls";
            for (int i=0; i<this.pocet_w; i++)
            {
                walls[i].draw(g);
            }
            //kresli predmety
            //qDebug() << "+ Room.draw - items";
            if (pocet_i != 0)
            {
                for (int i=0; i<pocet_i; i++)
                {
                    if (items[i].isShown() == true)
                    {
                        items[i].draw(g);
                    }
                }
            }

            //qDebug() << "+ Room.draw - enemies";
            //kresli nepratele
            if (pocet_e != 0)
            {
                for (int i=0; i<pocet_e; i++)
                {
                    if (enemies[i].isDead() == false)
                    {
                        enemies[i].draw(g);

                        //narozeni noveho nepritele z nepritele c. 10
                        if (enemies[i].getType() == 10)
                        {
                            if ((this.pocet_e10 < E10) &&
                                    (((EType10)enemies[i]).release() == true))
                            {
                                int ktery = Game.rand.Next(6) + 11; //dynamicky
                                int j;
                                for (j=0; j<E10; j++)
                                {
                                    if (this.enemies10[j].getDamage() == -1) break;
                                }
                                switch (ktery)
                                {
                                    case 11:
                                        this.enemies10[j] = new EType11(
                                                1,enemies[i].getPosition().X,
                                                enemies[i].getPosition().Y,0,
                                                Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 12:
                                        this.enemies10[j] = new EType12(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 13:
                                        this.enemies10[j] = new EType13(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 14:
                                        this.enemies10[j] = new EType14(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 15:
                                        this.enemies10[j] = new EType15(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 16:
                                        this.enemies10[j] = new EType16(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                    case 17:
                                        this.enemies10[j] = new EType17(1,enemies[i].getPosition().X,enemies[i].getPosition().Y,0,Game.recalculate(50), Game.recalculate(50));
                                        break;
                                }
                                this.enemies10[j].setType(ktery);
                                this.enemies10[j].setDamage(10);
                                this.pocet_e10++;
                            }
                        }//konec - narozeni noveho nepritele z nepritele c. 10
                    }
                    //kresli strely nepratel
                    if (enemies[i].getShot() != null)
                    {
                        if (enemies[i].getShot().isShown() == true)
                        {
                            enemies[i].getShot().draw(g);
                        }
                    }
                }
            }

            //kresli nepratele z enemies10 !!!
            //qDebug() << "+ Room.draw - enemies from10";
            for (int i=0; i<E10; i++)
            {
                if (this.enemies10[i].isDead() == true)
                {
                    if (this.enemies10[i].getDamage() != -1)
                    {
                        this.enemies10[i].setDamage(-1);
                        this.pocet_e10--;
                    }
                    //continue;
                }
                else
                {
                    this.enemies10[i].draw(g);
                    if (this.enemies10[i].getShot() != null)
                    {
                        if (this.enemies10[i].getShot().isShown() == true)
                        {
                            this.enemies10[i].getShot().draw(g);
                        }
                    }
                }
            }

            //qDebug() << "< Room.draw";
        }

        public Wall getWall(int i)
        {
            return this.walls[i];
        }

        public Enemy getEnemy(int i)
        {
            return this.enemies[i];
        }

        public Item getItem(int i)
        {
            return this.items[i];
        }

        public int getWallsCount()
        {
            return this.walls.Count;
        }

        public int getEnemiesCount()
        {
            return this.enemies.Count;
        }

        public int getItemsCount()
        {
            return this.items.Count;
        }

        public Enemy getEnemy10(int i)
        {
            return this.enemies10[i];
        }

        public int getRoomBases()
        {
            return this.bases;
        }

        /*void storeRoom(QDataStream& out)
        {
            this.saveEnemies(out);
            this.saveItems(out);
        }

        void loadRoom(QDataStream& in)
        {
            this.loadEnemies(in);
            this.loadItems(in);
        }*/

        /*void saveEnemies(QDataStream& out)
        {
            int c_e = this.getEnemiesCount();

            out << c_e;
            for (int i=0; i<c_e; i++)
            {
                Enemy* te = this.getEnemy(i);
                te->storeEnemy(out);
                if (te->getType() == 10)
                {
                    for (int j=0; j<E10; j++)
                    {
                        Enemy* te10 = this.getEnemy10(j);
                        if (te10)
                        {
                            te10->storeEnemy(out);
                        }
                    }
                }
                if (te->getType() == 20)
                {
                    //stores item or enemy carried by EType20
                    Enemy* tee = ((EType20*)te)->getEnemy();
                    if (tee)
                    {
                        //0-enemy, 1-item
                        out << 0;
                        tee->storeEnemy(out);
                    }
                    Item* tii = ((EType20*)te)->getItem();
                    if (tii)
                    {
                        //0-enemy, 1-item
                        out << 1;
                        tii->storeItem(out);
                    }
                }
            }
        }*/

        /*bool loadEnemies(QDataStream& in)
        {
            bool ret = true;

            in >> pocet_e;
            this.enemies = new Enemy*[pocet_e];
            for (int i=0; i<pocet_e; i++)
            {
                int type;
                int tmp;
                in >> type;
                switch (type)
                {
                    case 0: enemies[i] = new EType0(); break;
                    case 1:
                        in >> tmp;
                        enemies[i] = new EType1(tmp);
                        break;
                    case 2: enemies[i] = new EType2(); break;
                    case 3: enemies[i] = new EType3(); break;
                    case 4: enemies[i] = new EType4(); break;
                    case 5: enemies[i] = new EType5(); break;
                    case 6: enemies[i] = new EType6(); break;
                    case 7: enemies[i] = new EType7(); break;
                    case 8:
                        in >> tmp;
                        enemies[i] = new EType8(tmp);
                        break;
                    case 9: enemies[i] = new EType9(); break;
                    case 10: enemies[i] = new EType10(); break;
                    case 11: enemies[i] = new EType11(); break;
                    case 12: enemies[i] = new EType12(); break;
                    case 13: enemies[i] = new EType13(); break;
                    case 14: enemies[i] = new EType14(); break;
                    case 15: enemies[i] = new EType15(); break;
                    case 16: enemies[i] = new EType16(); break;
                    case 17: enemies[i] = new EType17(); break;
                    case 18: enemies[i] = new EType18(); break;
                    case 19: enemies[i] = new EType19(); break;
                    case 20:
                        in >> tmp;
                        enemies[i] = new EType20(tmp);
                        break;
                }//switch
                enemies[i].loadEnemy(in);
                enemies[i].setType(type);

                if (enemies[i].getType() == 10)
                {
                    for (int j=0; j<E10; j++)
                    {
                        int type10;
                        in >> type10;
                        switch (type10)
                        {
                            case 11: enemies10[j] = new EType11(); break;
                            case 12: enemies10[j] = new EType12(); break;
                            case 13: enemies10[j] = new EType13(); break;
                            case 14: enemies10[j] = new EType14(); break;
                            case 15: enemies10[j] = new EType15(); break;
                            case 16: enemies10[j] = new EType16(); break;
                            case 17: enemies10[j] = new EType17(); break;
                            default: enemies10[j] = new Enemy(); break;
                        }
                        enemies10[j].loadEnemy(in);
                        enemies10[j].setType(type10);
                    }
                }

                if (enemies[i].getType() == 20)
                {
                    int t;
                    in >> t;
                    if (t == 0)
                    {
                        //enemy
                        ((EType20*)enemies[i])->createFellowEnemy(in);
                    }
                    else if (t == 1)
                    {
                        //item
                        ((EType20*)enemies[i])->createFellowItem(in);
                    }
                }
            }//for

            return ret;
        }*/

        /*void saveItems(QDataStream& out)
        {
            int c_i = this.getItemsCount();
            out << c_i;
            for (int i=0; i<c_i; i++)
            {
                Item* ti = this.getItem(i);
                ti->storeItem(out);
            }
        }*/

        /*bool loadItems(QDataStream& in)
        {
            bool ret = true;

            in >> pocet_i;
            items = new Item*[pocet_i];
            for (int i=0; i<pocet_i; i++)
            {
                int type;
                in >> type;
                switch (type)
                {
                    case 0: items[i] = new IType0(); break;
                    case 1: items[i] = new IType1(); break;
                    case 2: items[i] = new IType2(); break;
                    case 3: items[i] = new IType3(); break;
                    case 4: items[i] = new IType4(); break;
                    case 5: items[i] = new IType5(); break;
                }
                items[i].loadItem(in);
                items[i].setType(type);
            }

            return ret;
        }*/

        public List<Wall> getWalls()
        {
            return this.walls;
        }
    }
}
