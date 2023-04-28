using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

using rth_jet_story.game;

namespace rth_jet_story
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public const int RES_X = 800;
        public const int RES_Y = 600;
        public const int RATIO = 100;
        //original height of status bar on top of screen (from 800x600)
        //value has to be recalculated every time !!!
        public const int STATUS_BAR_H = 100;

        public static Random rand = new Random(Environment.TickCount);

        bool key_pressed_Esc = false;
        bool key_pressed_Enter = false;
        bool button_pressed_Esc = false;
        bool button_pressed_Action = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D splashScreen;

        enum Modes
        {
            MODE_SPLASH,
            MODE_MENU,
            MODE_CONTROLS,
            MODE_GAME,
            MODE_PAUSED,
            MODE_NEW,
            //MODE_LOAD,
            //MODE_SAVE
        };
        int mode;

        bool paused;

        private Menu mainMenu;
        private ControlsMenu controlsMenu;
        SpriteFont menuFont;

        enum MainMenuOptions
        {
            MM_CONTINUE,
            MM_NEW_GAME,
            MM_SOUNDS,
            MM_MUSIC,
            MM_CONTROLS,
            MM_EXIT
        };

        private const int PINFO = 84;
        private InfoBarRect[] infoBarRects = new InfoBarRect[PINFO];
        SpriteFont infoBarFont;
        private int infBFHeight;

        private Texture2D infob;

        public enum Controls
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            FIRE,
            PAUSE
        };
        private ControlsMenuItem keyUp;
        private ControlsMenuItem keyDown;
        private ControlsMenuItem keyLeft;
        private ControlsMenuItem keyRight;
        private ControlsMenuItem keyFire;
        //private ControlsMenuItem keyPause;

        Ship lod = null; //not owned, in Maze
        int smrt;

        private List<Boom> booms;

        Color background = Color.Black;

        Texture2D bar;

        private bool continueNeeded;

        private SwitchMenuItem soundsSwitch;
        private SwitchMenuItem musicSwitch;

        private MyGamePad myGamePad;
        private bool gamePadInUse = false;

        private bool shipUp = false;
        private bool shipLeft = false;
        private bool shipRight = false;
        private bool shipShot = false;
        private bool shipLaunch = false;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "rth jet-story";

            this.booms = new List<Boom>();

            this.mode = (int)Modes.MODE_SPLASH;

            this.continueNeeded = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = RES_X;
            graphics.PreferredBackBufferHeight = RES_Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            this.myGamePad = new MyGamePad(Services, Window.Handle);
        }

        protected override void LoadContent()
        {
            Console.WriteLine("> LoadContent");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.splashScreen = Content.Load<Texture2D>("images/jet-story_loadscr");
            this.infob = Content.Load<Texture2D>("images/info");

            Wall.loadImages(Content);
            Special.loadImages(Content);
            Fragment.loadImages(Content);
            Cannon.loadImages(Content);
            Boom.loadImages(Content);
            EType0.loadImages(Content);
            EType1.loadImages(Content);
            EType1_Shot.loadImages(Content);
            EType2.loadImages(Content);
            EType2_Shot.loadImages(Content);
            EType3.loadImages(Content);
            EType3_Shot.loadImages(Content);
            EType4.loadImages(Content);
            EType5.loadImages(Content);
            EType5_Shot.loadImages(Content);
            EType6.loadImages(Content);
            EType6_Shot.loadImages(Content);
            EType7.loadImages(Content);
            EType7_Shot.loadImages(Content);
            EType8.loadImages(Content);
            EType8_Shot.loadImages(Content);
            EType9.loadImages(Content);
            EType9_Shot.loadImages(Content);
            EType10.loadImages(Content);
            EType11.loadImages(Content);
            EType12.loadImages(Content);
            EType13.loadImages(Content);
            EType13_Shot.loadImages(Content);
            EType14.loadImages(Content);
            EType15.loadImages(Content);
            EType16.loadImages(Content);
            EType17.loadImages(Content);
            EType18.loadImages(Content);
            EType19.loadImages(Content);
            EType20.loadImages(Content);
            IType0.loadImages(Content);
            IType1.loadImages(Content);
            IType2.loadImages(Content);
            IType3.loadImages(Content);
            IType4.loadImages(Content);
            IType5.loadImages(Content);

            GameSounds.getInstance().loadSounds(Content);

            // Loads font from the esp_20.bmp file. 
            // Content processor has to be set to 'Sprite Font Texture' !!!
            menuFont = Content.Load<SpriteFont>("font/esp_20");
            this.createMenus();

            infoBarFont = Content.Load<SpriteFont>("font/esp_15");
            this.infBFHeight = (int)this.infoBarFont.MeasureString("W").Y;
            this.createInfoBarRects();

            this.bar = new Texture2D(GraphicsDevice, 1, 1);
            this.bar.SetData(new[] { Color.White });

            Maze.getMazeInstance().resetMaze();
            Maze.getMazeInstance().setParent(this);

            Console.WriteLine("< LoadContent");
        }

        private void createMenus()
        {
            this.mainMenu = new Menu(
                this, 100, 100, RES_X - 200, RES_Y - 200, menuFont);

            this.soundsSwitch = new SwitchMenuItem(this.mainMenu, (int)MainMenuOptions.MM_SOUNDS,
                new string[] { "Sounds", "ON" }, true);
            this.musicSwitch = new SwitchMenuItem(this.mainMenu, (int)MainMenuOptions.MM_MUSIC,
                new string[] { "Music", "OFF" }, false);

            this.mainMenu.add(
                new MenuItem(this.mainMenu, (int)MainMenuOptions.MM_NEW_GAME, 
                    new string[]{"New Game"}));
            this.mainMenu.add(this.soundsSwitch);
            this.mainMenu.add(this.musicSwitch);
            this.mainMenu.add(
                new MenuItem(this.mainMenu, (int)MainMenuOptions.MM_CONTROLS, 
                    new string[]{"Redefine Keys"}));
            this.mainMenu.add(
                new MenuItem(this.mainMenu, (int)MainMenuOptions.MM_EXIT, 
                    new string[]{"Exit"}));

            this.controlsMenu = new ControlsMenu(this, 
                100, 100, RES_X - 200, RES_Y - 200, menuFont);

            this.keyUp = new ControlsMenuItem(
                    this.controlsMenu, (int)Controls.UP, new string[] { "UP:", "Up" },
                    MenuItem.Layouts.LEFT,
                    Keys.Up);
            this.keyDown = new ControlsMenuItem(
                    this.controlsMenu, (int)Controls.DOWN, new string[] { "DOWN:", "Down" },
                    MenuItem.Layouts.LEFT,
                    Keys.Down);
            this.keyLeft = new ControlsMenuItem(
                    this.controlsMenu, (int)Controls.LEFT, new string[] { "LEFT:", "Left" },
                    MenuItem.Layouts.LEFT,
                    Keys.Left);
            this.keyRight = new ControlsMenuItem(
                    this.controlsMenu, (int)Controls.RIGHT, new string[] { "RIGHT:", "Right" },
                    MenuItem.Layouts.LEFT,
                    Keys.Right);
            this.keyFire = new ControlsMenuItem(
                    this.controlsMenu, (int)Controls.FIRE, new string[] { "FIRE:", "A" },
                    MenuItem.Layouts.LEFT,
                    Keys.A);
            this.controlsMenu.add(this.keyUp);
            this.controlsMenu.add(this.keyDown);
            this.controlsMenu.add(this.keyLeft);
            this.controlsMenu.add(this.keyRight);
            this.controlsMenu.add(this.keyFire);
        }

        private void createInfoBarRects()
        {
            //1. rada - cela
            for (int i=0; i<32; i++)
            {
                infoBarRects[i] = new InfoBarRect(this, i*25, 0, Color.Cyan);
            }
            //4. rada - cela
            for (int i=32; i<64; i++)
            {
                infoBarRects[i] = new InfoBarRect(this, (i-32)*25, 75, Color.Cyan);
            }
            //3. rada - pod ukazatelem score
            for (int i=64; i<72; i++)
            {
                infoBarRects[i] = new InfoBarRect(this, 525+((i-64)*25), 50, Color.Cyan);
            }
            infoBarRects[72] = new InfoBarRect(this, 0,25, Color.Cyan); //2.rada
            infoBarRects[73] = new InfoBarRect(this, 0,50, Color.Cyan); //3.rada
            infoBarRects[74] = new InfoBarRect(this, 250,25, Color.Cyan); //2.rada
            infoBarRects[75] = new InfoBarRect(this, 250,50, Color.Cyan); //3.rada
            infoBarRects[76] = new InfoBarRect(this, 325,25, Color.Cyan); //2.rada
            infoBarRects[77] = new InfoBarRect(this, 325,50, Color.Cyan); //3.rada
            infoBarRects[78] = new InfoBarRect(this, 500,25, Color.Cyan); //2.rada
            infoBarRects[79] = new InfoBarRect(this, 500,50, Color.Cyan); //3.rada
            infoBarRects[80] = new InfoBarRect(this, 525,25, Color.Cyan); //2.rada
            infoBarRects[81] = new InfoBarRect(this, 700,25, Color.Cyan); //2.rada
            infoBarRects[82] = new InfoBarRect(this, 775,25, Color.Cyan); //2.rada
            infoBarRects[83] = new InfoBarRect(this, 775,50, Color.Cyan); //3.rada
        }

        public void setUpInfoBarRects()
        {
            for (int i=0; i<PINFO; i++)
            {
                if (infoBarRects[i] != null)
                {
                    infoBarRects[i].setColor(Color.Cyan);
                }
            }
        }

        void initGame()
        {
            Maze.getMazeInstance().doNewGame();
            Maze.getMazeInstance().setScore(0);

            this.lod = Maze.getMazeInstance().ship(); //mandatory
            this.lod.nactiBitmapy(this.Content);

            this.smrt = 0;
            this.clearBooms();
            
            /*this.keyUpPressed = false;
            this.keyDownPressed = false;
            this.keyLeftPressed = false;
            this.keyRightPressed = false;
            this.keyFirePressed = false;*/
        }

        public int getMode()
        {
            return this.mode;
        }

        protected override void UnloadContent()
        {
            Wall.disposeImages();
            Special.disposeImages();
            Fragment.disposeImages();
            Cannon.disposeImages();
            Boom.disposeImages();
            EType0.disposeImages();
            EType1.disposeImages();
            EType1_Shot.disposeImages();
            EType2.disposeImages();
            EType2_Shot.disposeImages();
            EType3.disposeImages();
            EType3_Shot.disposeImages();
            EType4.disposeImages();
            EType5.disposeImages();
            EType5_Shot.disposeImages();
            EType6.disposeImages();
            EType6_Shot.disposeImages();
            EType7.disposeImages();
            EType7_Shot.disposeImages();
            EType8.disposeImages();
            EType8_Shot.disposeImages();
            EType9.disposeImages();
            EType9_Shot.disposeImages();
            EType10.disposeImages();
            EType11.disposeImages();
            EType12.disposeImages();
            EType13.disposeImages();
            EType13_Shot.disposeImages();
            EType14.disposeImages();
            EType15.disposeImages();
            EType16.disposeImages();
            EType17.disposeImages();
            EType18.disposeImages();
            EType19.disposeImages();
            EType20.disposeImages();
            IType0.disposeImages();
            IType1.disposeImages();
            IType2.disposeImages();
            IType3.disposeImages();
            IType4.disposeImages();
            IType5.disposeImages();

            GameSounds.getInstance().disposeSounds();

            this.splashScreen.Dispose();
            this.infob.Dispose();
            this.bar.Dispose();
        }

        private void doMovements(float t)
        {
            if (paused == false)
            {
                Room temp = Maze.getMazeInstance().getActiveRoom();

                lod.move(t);//pohyb lode
                lod.updateMembers();

                lod.setColliding(false);

                MovementsAndCollisions.collisions_Ship_Walls(temp, lod);

                MovementsAndCollisions.collisions_Ship_Enemies(this, temp, lod, booms);

                MovementsAndCollisions.collisions_Ship_EnemiesFrom10(temp, lod, booms);

                MovementsAndCollisions.collisions_Ship_Items(this, temp, lod);

                //this must go after collisions !!!
                lod.doMovementBetweenRooms();

                MovementsAndCollisions.movement_Cannon(temp, lod, booms, t);

                MovementsAndCollisions.movement_Special(temp, lod, booms, t);

                MovementsAndCollisions.movement_Enemies(temp, lod, booms, t);

                MovementsAndCollisions.movement_EnemiesFrom10(temp, lod, booms, t);

                MovementsAndCollisions.movement_Items(temp);

                MovementsAndCollisions.collisions_Fragments_Walls(temp, booms, t);

                MovementsAndCollisions.movement_Booms(booms, ref this.background, t);

                if (lod.isColliding())
                {
                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_DAMAGE);
                }
                else
                {
                    GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_DAMAGE);
                }
            }
        }

        private void doGame(float t)
        {
            if (smrt == 0)
            {
                this.doMovements(t);
            }

            //smrt
            if (lod.getDamage() <= 0)
            {
                GameSounds.getInstance().stopSound(GameSounds.SoundsIDs.SOUND_DAMAGE);

                MovementsAndCollisions.collisions_Fragments_Walls(
                        Maze.getMazeInstance().getActiveRoom(), booms, t);
                MovementsAndCollisions.movement_Booms(booms, ref this.background, t);

                if (smrt < 5)
                {
                    booms.Add(
                        new Boom(
                            lod.getPosition().X + rand.Next(recalculate(90)) - recalculate(10),
                            lod.getPosition().Y + rand.Next(recalculate(40)) - recalculate(10),
                            true, false));
                    smrt++;
                }
                if (booms.Count == 0)
                {
                    //TODO: sleep 1s
                    //TODO: death
                    //score
                    //this->doScore();
                }
            }//smrt

            //vitezstvi
            if (Maze.getMazeInstance().getBases() == 0)
            {
                MovementsAndCollisions.collisions_Fragments_Walls(
                        Maze.getMazeInstance().getActiveRoom(), booms, t);
                MovementsAndCollisions.movement_Booms(booms, ref this.background, t);

                if (smrt < 500)
                {
                    booms.Add(
                        new Boom(
                            rand.Next(recalculate(700)) + recalculate(50),
                            rand.Next(recalculate(400)) + recalculate(150),
                            true, false));
                    smrt++;
                }
                if (booms.Count == 0)
                {
                    //TODO: sleep 1s
                    //TODO: victory
                    //score
                    //this->doScore();
                }
            }//vitezstvi
        }

        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine("> Update");

            // Allows the game to exit
            if (this.myGamePad.isPressed(MyGamePad.MyKeys.Back, PlayerIndex.One) == true)
                this.Exit();

            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                key_pressed_Esc = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                key_pressed_Enter = false;
            }
            if (this.myGamePad.isReleased(MyGamePad.MyKeys.Key1, PlayerIndex.One))
            {
                button_pressed_Action = false;
            }
            if (this.myGamePad.isReleased(MyGamePad.MyKeys.Key4, PlayerIndex.One))
            {
                button_pressed_Esc = false;
            }

            if (this.gamePadInUse)
            {
                bool res = this.gamePadAction();
                if (!res)
                {
                    this.gamePadInUse = false;
                }
            }
            else
            {
                if (!this.keyAction())
                {
                    this.gamePadInUse = this.gamePadAction();
                }
            }

            if (this.mode == (int)Modes.MODE_GAME)
            {
                float t = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

                if (paused == false)
                {
                    //Console.WriteLine("t = " + t);
                    this.doGame(t);
                }
            }

            base.Update(gameTime);

            //Console.WriteLine("< Update");
        }

        private void doShipAction()
        {
            //NAHORU
            if (this.shipUp)
            {
                lod.thrustUp();
            }
            else
            {
                lod.stopUp();
            }

            //DOLEVA
            if (this.shipLeft)
            {
                lod.thrustLeft();
            }
            else
            {
                lod.stopLeft();
            }

            //DOPRAVA
            if (this.shipRight)
            {
                lod.thrustRight();
            }
            else
            {
                lod.stopRight();
            }

            //strelba Special
            if (this.shipLaunch)
            {
                lod.launch();
            }

            //strelba Cannon
            if (this.shipShot)
            {
                lod.shoot();
            }
        }

        private bool gameAction()
        {
            //this.shipUp = this.myGamePad.isPressed(MyGamePad.MyKeys.Up, PlayerIndex.One);
            this.shipUp = this.myGamePad.isPressed(MyGamePad.MyKeys.LStickUp, PlayerIndex.One);
            //this.shipLeft = this.myGamePad.isPressed(MyGamePad.MyKeys.Left, PlayerIndex.One);
            this.shipLeft = this.myGamePad.isPressed(MyGamePad.MyKeys.LStickLeft, PlayerIndex.One);
            //this.shipRight = this.myGamePad.isPressed(MyGamePad.MyKeys.Right, PlayerIndex.One);
            this.shipRight = this.myGamePad.isPressed(MyGamePad.MyKeys.LStickRight, PlayerIndex.One);

            this.shipLaunch = this.myGamePad.isPressed(MyGamePad.MyKeys.Key1, PlayerIndex.One);
            this.shipShot = this.myGamePad.isPressed(MyGamePad.MyKeys.Key2, PlayerIndex.One);

            this.doShipAction();

            return this.shipUp || this.shipLeft || this.shipRight || this.shipLaunch || this.shipShot;
        }

        private bool gameAction(KeyboardState kbs)
        {
            this.shipUp = kbs.IsKeyDown(this.keyUp.Key);
            this.shipLeft = kbs.IsKeyDown(this.keyLeft.Key);
            this.shipRight = kbs.IsKeyDown(this.keyRight.Key);
            this.shipLaunch = kbs.IsKeyDown(this.keyDown.Key);
            this.shipShot = kbs.IsKeyDown(this.keyFire.Key);

            this.doShipAction();

            return this.shipUp || this.shipLeft || this.shipRight || this.shipLaunch || this.shipShot;
        }

        private bool keyAction()
        {
            //Console.WriteLine("keyAction");
            bool result = false;
            switch (this.mode)
            {
                case (int)Modes.MODE_SPLASH:
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            Console.WriteLine("KEYS: splash -> exit");
                            this.Exit();
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !key_pressed_Enter)
                        {
                            key_pressed_Enter = true; //avoid multiple ENTER
                        }
                        Maze.getMazeInstance().initMaze();
                        this.mode = (int)Modes.MODE_MENU;
                        result = true;
                    }
                    break;

                case (int)Modes.MODE_MENU:
                    {
                        KeyboardState kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Escape) && !key_pressed_Esc)
                        {
                            Console.WriteLine("KEYS: menu -> exit");
                            result = true;
                            this.Exit();
                        }
                        int ret = this.mainMenu.processKey(kbs);
                        if (key_pressed_Enter)
                        {
                            result = true;
                            break;
                        }
                        if (ret == (int)Menu.Results.NONE)
                        {
                            break;
                        }
                        //Console.WriteLine("menu ret = " + ret);
                        result = true;
                        switch (ret)
                        {
                            case (int)MainMenuOptions.MM_CONTINUE:
                                this.mode = (int)Modes.MODE_GAME;
                                this.paused = false;
                                result = true;
                                break;

                            case (int)MainMenuOptions.MM_NEW_GAME:
                                Console.WriteLine("NEW GAME");
                                this.mode = (int)Modes.MODE_GAME;
                                this.paused = false;
                                this.initGame();
                                Maze.getMazeInstance().startTheGame();
                                result = true;
                                break;

                            case (int)MainMenuOptions.MM_CONTROLS:
                                this.mode = (int)Modes.MODE_CONTROLS;
                                result = true;
                                break;

                            case (int)MainMenuOptions.MM_EXIT:
                                result = true;
                                this.Exit();
                                break;

                            case (int)MainMenuOptions.MM_SOUNDS:
                                this.soundsSwitch.toggle();
                                if (this.soundsSwitch.Status)
                                {
                                    this.soundsSwitch.setText(1, "ON");
                                }
                                else
                                {
                                    this.soundsSwitch.setText(1, "OFF");
                                }
                                GameSounds.getInstance().enableSounds(this.soundsSwitch.Status);
                                result = true;
                                break;

                            case (int)MainMenuOptions.MM_MUSIC:
                                this.musicSwitch.toggle();
                                if (this.musicSwitch.Status)
                                {
                                    this.musicSwitch.setText(1, "ON");
                                }
                                else
                                {
                                    this.musicSwitch.setText(1, "OFF");
                                }
                                //TODO
                                result = true;
                                break;
                        }
                        break;
                    }

                case (int)Modes.MODE_CONTROLS:
                    {
                        KeyboardState kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Escape) && !key_pressed_Esc)
                        {
                            this.mode = (int)Modes.MODE_MENU;
                            key_pressed_Esc = true;
                            result = true;
                        }
                        else
                        {
                            int ret = this.controlsMenu.processKey(kbs);
                            result = true;
                        }
                        break;
                    }

                case (int)Modes.MODE_GAME:
                    {
                        KeyboardState kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Escape) && !key_pressed_Esc)
                        {
                            this.mode = (int)Modes.MODE_MENU;
                            key_pressed_Esc = true;
                            GameSounds.getInstance().stopAll();
                            if (this.continueNeeded)
                            {
                                this.mainMenu.insertAtTheBegining(
                                    new MenuItem(this.mainMenu, (int)MainMenuOptions.MM_CONTINUE,
                                        new string[] { "Continue" }));
                                this.continueNeeded = false;
                            }
                            result = true;
                            Console.WriteLine("KEYS: game -> menu");
                        }
                        else
                        {
                            result = this.gameAction(kbs);
                        }
                        break;
                    }
            }//switch
            //Console.WriteLine("key action = " + result);
            return result;
        }

        private bool gamePadAction()
        {
            //Console.WriteLine("> gamePadAction");

            bool result = false;
            switch (this.mode)
            {
                case (int)Modes.MODE_SPLASH:
                    {
                        //Console.WriteLine("+ gamePadAction: SPLASH");
                        if (this.myGamePad.isPressed(MyGamePad.MyKeys.Back, PlayerIndex.One))
                        {
                            Console.WriteLine("GPAD: splash -> exit");
                            this.Exit();
                            break;
                        }
                        if (this.myGamePad.isPressed(MyGamePad.MyKeys.Key1, PlayerIndex.One) && 
                            !button_pressed_Action)
                        {
                            button_pressed_Action = true; //avoid multiple ENTER
                        }
                        if (this.myGamePad.isPressed(MyGamePad.MyKeys.Key1, PlayerIndex.One))
                        {
                            Maze.getMazeInstance().initMaze();
                            this.mode = (int)Modes.MODE_MENU;
                            result = true;
                        }
                        break;
                    }

                case (int)Modes.MODE_MENU:
                    {
                        //Console.WriteLine("+ gamePadAction: MENU");
                        if (this.myGamePad.isPressed(MyGamePad.MyKeys.Back, PlayerIndex.One) &&
                            !button_pressed_Esc)
                        {
                            Console.WriteLine("GPAD: menu -> exit");
                            this.Exit();
                            result = true;
                            break;
                        }
                        int ret = this.mainMenu.processGamePad(this.myGamePad, PlayerIndex.One);
                        if (button_pressed_Action)
                        {
                            result = true;
                            break;
                        }
                        if (ret == (int)Menu.Results.NONE)
                        {
                            break;
                        }
                        result = true;
                        switch (ret)
                        {
                            case (int)MainMenuOptions.MM_CONTINUE:
                                this.mode = (int)Modes.MODE_GAME;
                                this.paused = false;
                                break;

                            case (int)MainMenuOptions.MM_NEW_GAME:
                                Console.WriteLine("gpad: NEW GAME");
                                this.mode = (int)Modes.MODE_GAME;
                                this.paused = false;
                                this.initGame();
                                Maze.getMazeInstance().startTheGame();
                                break;

                            case (int)MainMenuOptions.MM_CONTROLS:
                                //this.mode = (int)Modes.MODE_CONTROLS; //no controls change for GamePad
                                break;

                            case (int)MainMenuOptions.MM_EXIT:
                                this.Exit();
                                break;

                            case (int)MainMenuOptions.MM_SOUNDS:
                                this.soundsSwitch.toggle();
                                if (this.soundsSwitch.Status)
                                {
                                    this.soundsSwitch.setText(1, "ON");
                                }
                                else
                                {
                                    this.soundsSwitch.setText(1, "OFF");
                                }
                                GameSounds.getInstance().enableSounds(this.soundsSwitch.Status);
                                break;

                            case (int)MainMenuOptions.MM_MUSIC:
                                this.musicSwitch.toggle();
                                if (this.musicSwitch.Status)
                                {
                                    this.musicSwitch.setText(1, "ON");
                                }
                                else
                                {
                                    this.musicSwitch.setText(1, "OFF");
                                }
                                //TODO
                                break;
                        }
                        break;
                    }

                case (int)Modes.MODE_CONTROLS:
                    {
                        //no controls change for GamePad
                        break;
                    }

                case (int)Modes.MODE_GAME:
                    {
                        //Console.WriteLine("+ gamePadAction: GAME");
                        if (this.myGamePad.isPressed(MyGamePad.MyKeys.Back, PlayerIndex.One) &&
                            !button_pressed_Esc)
                        {
                            this.mode = (int)Modes.MODE_MENU;
                            button_pressed_Esc = true;
                            GameSounds.getInstance().stopAll();
                            if (this.continueNeeded)
                            {
                                this.mainMenu.insertAtTheBegining(
                                    new MenuItem(this.mainMenu, (int)MainMenuOptions.MM_CONTINUE,
                                        new string[] { "Continue" }));
                                this.continueNeeded = false;
                            }
                            result = true;
                            Console.WriteLine("GPAD: game -> menu");
                        }
                        else
                        {
                            result = this.gameAction();
                        }
                        break;
                    }
            }//switch
            return result;
        }

        private void drawInfoBar(SpriteBatch g)
        {
            //info bar image
            g.Draw(infob, new Vector2(0, 0), Color.White);

            if (lod != null)
            {
                //special ammo
                g.DrawString(this.infoBarFont, lod.getSpecial().getAmmo().ToString(),
                    new Vector2(recalculate(275), recalculate(50)),
                    Color.White);
                //bases
                g.DrawString(this.infoBarFont, Maze.getMazeInstance().getBases().ToString(),
                    new Vector2(recalculate(725), recalculate(50)),
                    Color.White);
                //score
                g.DrawString(this.infoBarFont, Maze.getMazeInstance().getScore().ToString(),
                    new Vector2(recalculate(550), recalculate(25)),
                    Color.White);

                int pomt = -1;
                if (lod.getSpecial().getType() == lod.getSpecial().getToChange())
                {
                    pomt = lod.getSpecial().getType();
                }
                else
                {
                    pomt = lod.getSpecial().getToChange();
                }

                //special images
                switch (pomt)
                {
                    case (int)Special.Type.SPECIAL_SIDE:
                        g.Draw(Special.left, new Vector2(275, 25), Color.White);
                        g.Draw(Special.left, new Vector2(300, 25), Color.White);
                        break;
                    case (int)Special.Type.SPECIAL_DOWN:
                        g.Draw(Special.down, new Vector2(275, 25), Color.White);
                        g.Draw(Special.down, new Vector2(300, 25), Color.White);
                        break;
                    case (int)Special.Type.SPECIAL_SPHERE:
                        g.Draw(Special.circle, new Vector2(275, 25), Color.White);
                        g.Draw(Special.circle, new Vector2(300, 25), Color.White);
                        break;
                }

                //fuel bar
                int fuelw = (int)((lod.getFuel() * recalculate(101)) / 1000);
                g.Draw(this.bar, 
                    new Rectangle(
                        recalculate(125), recalculate(25),
                        fuelw, recalculate(21)), 
                    Color.White);
                //ammo bar
                int ammow = (int)((lod.getCannon().getAmmo() * recalculate(101)) / 1000);
                g.Draw(this.bar, 
                    new Rectangle(
                        recalculate(125), recalculate(50),
                        ammow, recalculate(21)), 
                    Color.White);
                //shield bar
                int damagew = (int)((lod.getDamage() * recalculate(101)) / 1000);
                g.Draw(this.bar, 
                    new Rectangle(
                        recalculate(375), recalculate(50),
                        damagew, recalculate(21)), 
                    Color.White);
            }

            //vyplne:
            for (int i=0; i<PINFO; i++)
            {
                if (infoBarRects[i] != null)
                {
                    infoBarRects[i].draw(g);
                }
            }
        }

        private void drawScene(SpriteBatch g)
        {
            //bludiste
            Maze.getMazeInstance().draw(g);

            if (lod != null && smrt == 0)
            {
                //kreslim lod
                lod.draw(g);
                //kreslim strelu Cannon lode
                if (lod.getCannon().isShown() == true)
                {
                    lod.getCannon().draw(g);
                }
                //kreslim strelu Special lode
                if (lod.getSpecial().isShown() == true)
                {
                    lod.getSpecial().draw(g);
                }
            }

            //vybuchy
            if (paused == false)
            {
                for (int pos = 0; pos < booms.Count; pos++)
                {
                    if (!booms[pos].isFinished())
                    {
                        booms[pos].draw(g);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //Console.WriteLine("> Draw");

            GraphicsDevice.Clear(this.background);

            //this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            switch (this.mode)
            {
                case (int)Modes.MODE_SPLASH:
                    this.spriteBatch.Draw(this.splashScreen,
                        new Vector2(0, 0), Color.White);
                    break;

                case (int)Modes.MODE_MENU:
                    this.mainMenu.draw(this.spriteBatch);
                    break;

                case (int)Modes.MODE_CONTROLS:
                    this.controlsMenu.draw(this.spriteBatch);
                    break;

                case (int)Modes.MODE_GAME:
                    //GraphicsDevice.RenderState.ScissorTestEnable = false;
                    //g.GraphicsDevice.ScissorRectangle = this.clipRect;
                    this.drawInfoBar(this.spriteBatch);
                    this.drawScene(this.spriteBatch);
                    //TODO: paused string
                    break;

            }

            this.spriteBatch.End();

            base.Draw(gameTime);

            //Console.WriteLine("< Draw");
        }

        public void clearBooms()
        {
            this.booms.Clear();
        }

        public static int recalculate(int x)
        {
            float nx = ( (float)x * RATIO ) / (float)100;
            return ((int)Math.Ceiling(nx));
        }

        public static float recalculate(float x)
        {
            return ((x * RATIO) / (float)100);
        }

        public static Color getRandomColor()
        {
            int r = rand.Next(234) + 20;
            int g = rand.Next(234) + 20;
            int b = rand.Next(234) + 20;
            //Console.WriteLine(r + "," + g + "," + b);
            return new Color((byte)r, (byte)g, (byte)b);
        }
    }
}
