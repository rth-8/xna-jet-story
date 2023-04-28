using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Maze
    {
        private static Maze mazeInstance = null;

        public static Maze getMazeInstance()
        {
            if (mazeInstance == null)
            {
                mazeInstance = new Maze();
            }
            return mazeInstance;
        }

        private Game parent;

        private int mode;

        private int r, s;
        Room[,] rooms;
        private bool[,] visited;

        //stored original starting active room coordinates
        private int active_room_x;
        private int active_room_y;

        //current active room coordinates
        private int currentActRoomX;
        private int currentActRoomY;

        private int bases; //stored original value
        private int currentBases; //current maze value        

        private bool loaded;

        private Ship lod;
        private int score;


        private Maze()
        {
            //this.initMaze();
            this.mode = -1;
            this.loaded = false;
            this.score = 0;
            this.lod = null;
            this.parent = null;
        }

        public void resetMaze()
        {
            this.loaded = false;
        }

        public void destroyMaze()
        {
            for (int i=0; i<r; i++)
            {
                for (int j=0; j<s; j++)
                {
                    if (rooms[i,j] != null)
                    {
                        rooms[i,j] = null;
                    }
                }
            }

            this.lod = null;
        }

        public void initMaze()
        {
            Console.WriteLine("> Maze.initMaze");
            this.bases = 0;

            string filename = "Content/data/maze.txt";

            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            this.r = Int32.Parse(reader.ReadLine());
            this.s = Int32.Parse(reader.ReadLine());

            this.visited = new bool[r,s];

            for (int i=0; i<this.r; i++)
            {
                for (int j=0; j<this.s; j++)
                {
                    //set to 'true' for the first time
                    //-> if new game is chosen, all rooms will be loaded
                    this.visited[i,j] = true;
                }
            }

            this.active_room_x = Int32.Parse(reader.ReadLine());
            this.active_room_y = Int32.Parse(reader.ReadLine());
            this.bases = Int32.Parse(reader.ReadLine());

            //Alokuji pamet pro radky
            this.rooms = new Room[r,s];
            //Alokuji pamet pro sloupce
            for (int i = 0; i < this.r; i++)
            {
                for (int j = 0; j < this.s; j++)
                {
                    this.rooms[i,j] = null;
                }
            }

            Console.WriteLine("< Maze.initMaze");
        }

        public void startTheGame()
        {
            this.changeActiveRoom(this.currentActRoomX, this.currentActRoomY);
        }

        public void doLoadRoomFromData(int i, int j)
        {
            if (this.rooms[i,j] != null)
            {
                this.rooms[i,j] = null;
            }
            this.rooms[i,j] = new Room();
            string id = i.ToString() + j.ToString();
            string buf_w = "Content/data/rooms/room" + id + ".txt";
            string buf_e = "Content/data/enemies/enemy" + id + ".txt";
            string buf_i = "Content/data/items/item" + id + ".txt";
            rooms[i,j].loadWallsFromFile(buf_w);
            rooms[i,j].loadEnemiesFromFile(buf_e);
            rooms[i,j].loadItemsFromFile(buf_i);
        }

        /**
         * NEW GAME
         */
        public void doNewGame()
        {
            Console.WriteLine("> Maze.doNewGame");

            this.currentBases = this.bases;
            int actRoomX = this.active_room_x;
            int actRoomY = this.active_room_y;

            int cnt = 0;
            for(int i=0; i<r; i++)
            {
                for(int j=0; j<s; j++)
                {
                    if (this.visited[i,j] == true)
                    {
                        this.doLoadRoomFromData(i, j);
                        this.visited[i,j] = false;
                        cnt++;
                    }
                }//for j
            }//for i

            //end of loading
            this.lod = new Ship(this);
            this.initShip();
            this.currentActRoomX = actRoomX;
            this.currentActRoomY = actRoomY;
            this.loaded = true;

            Console.WriteLine("< Maze.doNewGame");
        }

        /**
         * LOAD GAME
         */
        /*void doLoadGame(QString fileName)
        {
            int actRoomX = -1;
            int actRoomY = -1;

            QFile file(fileName);
            if (file.open(QIODevice::ReadOnly))
            {
                //qDebug() << "Loading ...";

                QDataStream in(&file);

                //first load visited to tmp
                bool** tmpv = new bool*[r];
                for (int i=0; i<r; i++)
                {
                    tmpv[i] = new bool[s];
                }
                for (int i=0; i<r; i++)
                {
                    for (int j=0; j<s; j++)
                    {
                        in >> tmpv[i,j]; // r*s bool
                    }
                }
                in >> actRoomX; //int
                in >> actRoomY; //int
                in >> this.currentBases; //int
                in >> this.score; //int
                if (this.lod)
                {
                    delete this.lod;
                    this.lod = null;
                }
                this.lod = new Ship(this);
                this.lod.loadShip(in);

                //then load rooms
                for (int i=0; i<r; i++) {
                    for (int j=0; j<s; j++) {
                        if (this.visited[i,j] == true && tmpv[i,j] == false)
                        {
                            // Reload room from previous play (this deletes old data)
                            //qDebug() << ">>> loading visited room [" << i << "," << j << "]";
                            this.doLoadRoomFromData(i, j);
                            // This kind of room is assumed to be not visited
                            // in saved game
                            this.visited[i,j] = false;
                        }
                        if (tmpv[i,j] == true)
                        {
                            // load saved room
                            //qDebug() << ">>> loading SAVED room [" << i << "," << j << "]";
                            if (this.rooms[i,j])
                            {
                                delete this.rooms[i,j];
                            }
                            this.rooms[i,j] = new Room();
                            this.rooms[i,j]->loadRoom(in);
                            // Because walls are not stored (they are not changing
                            // during play), so it has to be reloaded from data
                            QString id = QString::number(i) + QString::number(j);
                            QString buf_w("./data/rooms/room" + id + ".txt");
                            //qDebug() << buf_w;
                            rooms[i,j]->loadWallsFromFile(buf_w);
                            // Saved room is assumed to be visited in saved game
                            this.visited[i,j] = true;
                        }
                    }
                }

                // clean up
                for (int i=0; i<r; i++)
                {
                    delete tmpv[i];
                }
                delete tmpv;
            }
            else
            {
                //open file failed
                //TODO
            }

            this.currentActRoomX = actRoomX;
            this.currentActRoomY = actRoomY;
            this.loaded = true;
        }*/

        /**
         * SAVE
         */
        /*void doSaveGame(QString fileName)
        {
            //qDebug() << "> Maze.doSaveGame";

            QFile file(fileName);
            if (file.open(QIODevice::WriteOnly))
            {
                QDataStream out(&file);

                //qDebug() << "+ Maze.doSaveGame - store visited rooms array";
                for (int i=0; i<r; i++)
                {
                    for (int j=0; j<s; j++)
                    {
                        out << this.visited[i,j];
                    }
                }

                //qDebug() << "+ Maze.doSaveGame - store others ...";
                out << this.currentActRoomX;
                out << this.currentActRoomY;
                out << this.currentBases;
                out << this.score;

                this.ship()->storeShip(out);

                //then store each visited room
                for (int i=0; i<r; i++)
                {
                    for (int j=0; j<s; j++)
                    {
                        if (this.visited[i,j] == true)
                        {
                            //qDebug() << "+ Maze.doSaveGame - store room [" << i << "," << j << "]";
                            this.rooms[i,j]->storeRoom(out);
                        }
                    }
                }
            }
            else
            {
                //open file failed
                //TODO
            }

            //qDebug() << "< Maze.doSaveGame";
        }*/

        public void draw(SpriteBatch g)
        {
            this.rooms[this.currentActRoomX,this.currentActRoomY].draw(g);
        }

        public Room getActiveRoom()
        {
            return this.rooms[this.currentActRoomX,this.currentActRoomY];
        }

        public int getMazeR()
        {
            return this.r;
        }

        public int getMazeS()
        {
            return this.s;
        }

        public int getActiveRoomX()
        {
            return this.currentActRoomX;
        }

        public int getActiveRoomY()
        {
            return this.currentActRoomY;
        }

        public int getBases()
        {
            return this.currentBases;
        }

        public void decBases()
        {
            this.currentBases--;
        }

        public int getVisitedCount()
        {
            int cnt = 0;
            for(int i=0; i<r; i++) {
                for(int j=0; j<s; j++) {
                    if (this.visited[i,j] == true) {
                        cnt++;
                    }
                }
            }
            return cnt;
        }

        public void changeActiveRoom(int newx, int newy)
        {
            this.parent.clearBooms();

            GameSounds.getInstance().stopAll();

            this.currentActRoomX = newx;
            this.currentActRoomY = newy;
            this.visited[this.currentActRoomX,this.currentActRoomY] = true;
        }

        public bool isLoaded()
        {
            return this.loaded;
        }

        public void initShip()
        {
            //TODO: starting position should be part of loaded data
            lod.setPosition(
                    Game.recalculate(110),
                    Game.recalculate(300)
                    );
            lod.setVelocity(0, 0);
            lod.applyForce(0, 0);
            lod.setM(4.0f);
            lod.setDamage(1000);
            lod.getCannon().setAmmo(1000);
            lod.getSpecial().setType(2);
            lod.getSpecial().setAmmo(4);
            lod.getSpecial().hide();
            lod.resetOrient();
        }

        public Ship ship()
        {
            return this.lod;
        }

        public void setScore(int val)
        {
            this.score = val;
        }

        public void addToScore(int val)
        {
            this.score += val;
        }

        public int getScore()
        {
            return this.score;
        }

        public void setParent(Game game)
        {
            this.parent = game;
        }
    }
}
