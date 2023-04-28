using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace rth_jet_story
{
    class Menu
    {
        public enum Results
        {
            NONE        = -1,
            CONSUMED    = -2,
            WAIT        = -3
        };

        private Game parent;

        private Rectangle clipRect;

        protected List<MenuItem> items = new List<MenuItem>();
        protected int act_item;

        private SpriteFont font;
        private int text_height;
        private int top_margin;
        private int bottom_margin;
        private int item_height;

        private int height;
        private int heightOfAllItems;

        private Texture2D background;
        private Texture2D item_bg;

        private bool keyUp;
        private bool keyDown;
        protected bool keyEnter;

        private SoundEffect menuMove;

        private const float ALPHA = 0.3f;

        public Menu(Game parent, int x, int y, int w, int h, SpriteFont font)
        {
            this.parent = parent;

            this.act_item = 0;

            this.height = h;
            this.clipRect = new Rectangle(x, y, w, h);
            this.font = font;
            Vector2 f = font.MeasureString("W");
            this.text_height = (int)f.Y;
            this.top_margin = this.text_height / 4;
            this.bottom_margin = this.text_height / 4;
            this.item_height = this.top_margin + this.text_height + this.bottom_margin;
            this.heightOfAllItems = 0;

            Color menuBgCol = new Color(200, 200, 255, 75);
            this.background = this.createTexture(w, h, menuBgCol);

            Color itemBgCol = new Color(200, 200, 255, 75);
            this.item_bg = this.createTexture(
                this.clipRect.Width - LeftMargin - RightMargin, 
                ItemHeight, itemBgCol);

            this.keyUp = false;
            this.keyDown = false;
            this.keyEnter = false;

            this.menuMove = parent.Content.Load<SoundEffect>("sounds/enemy_10");
        }

        public int Width
        {
            get { return this.clipRect.Width; }
        }

        public int TextHeight
        {
            get { return this.text_height; }
        }

        public int TopMargin
        {
            get { return this.top_margin; }
        }

        public int BottomMargin
        {
            get { return this.bottom_margin; }
        }

        public int LeftMargin
        {
            get { return 10; }
        }

        public int RightMargin
        {
            get { return 10; }
        }

        public int ItemHeight
        {
            get { return this.item_height; }
        }

        public SpriteFont Font
        {
            get { return this.font; }
        }

        public Texture2D createTexture(int w, int h, Color refColor)
        {
            //Texture2D t = new Texture2D(parent.GraphicsDevice, w, h, 1, TextureUsage.None, SurfaceFormat.Color);
            Texture2D t = new Texture2D(parent.GraphicsDevice, w, h, false, SurfaceFormat.Color);
            Color[] color = new Color[w * h];
            for (int i = 0; i < color.Length; ++i)
            {
                color[i] = refColor;
            }
            t.SetData(color);
            return t;
        }

        public virtual void add(MenuItem item)
        {
            this.items.Add(item);
            this.heightOfAllItems += ItemHeight;
            this.layout();
        }

        public void insert(int pos, MenuItem item)
        {
            this.items.Insert(pos, item);
            this.heightOfAllItems += ItemHeight;
            this.layout();
        }

        public void insertAtTheBegining(MenuItem item)
        {
            this.items.Insert(0, item);
            this.heightOfAllItems += ItemHeight;
            this.layout();
        }

        private void layout()
        {
            if (this.items.Count == 1)
            {
                this.items[0].X = this.clipRect.Center.X;
                this.items[0].Y = this.clipRect.Center.Y;
            }
            else
            {
                if (this.items.Count % 2 == 0)
                {
                    this.layoutEven();
                }
                else
                {
                    this.layoutOdd();
                }
            }

            if (this.heightOfAllItems > this.height)
            {
                this.scrollDownTo(this.items[this.act_item]);
            }

        }

        private void scrollUpTo(MenuItem item)
        {
            while (!this.isFullyVisible(item))
            {
                this.scrollUp();
            }
        }

        private void scrollDownTo(MenuItem item)
        {
            while (!this.isFullyVisible(item))
            {
                this.scrollDown();
            }
        }

        private void layoutEven() //sudy pocet
        {
            int idx = this.items.Count / 2;
            //Console.WriteLine("idx = " + idx);

            this.items[idx - 1].X = this.clipRect.Center.X;
            this.items[idx - 1].Y = this.clipRect.Center.Y - BottomMargin - (TextHeight / 2);
            this.items[idx - 1].layout();

            this.items[idx].X = this.clipRect.Center.X;
            this.items[idx].Y = this.clipRect.Center.Y + TopMargin + (TextHeight / 2);
            this.items[idx].layout();

            for (int i = idx - 2; i >= 0; --i)
            {
                this.items[i].X = this.clipRect.Center.X;
                this.items[i].Y = this.items[i + 1].Y - ItemHeight;
                this.items[i].layout();
            }

            for (int i = idx + 1; i < this.items.Count; ++i)
            {
                this.items[i].X = this.clipRect.Center.X;
                this.items[i].Y = this.items[i - 1].Y + ItemHeight;
                this.items[i].layout();
            }
        }

        private void layoutOdd() //lichy pocet
        {
            int idx = this.items.Count / 2;
            //Console.WriteLine("idx = " + idx);

            this.items[idx].X = this.clipRect.Center.X;
            this.items[idx].Y = this.clipRect.Center.Y;
            this.items[idx].layout();

            for (int i = idx - 1; i >= 0; --i)
            {
                this.items[i].X = this.clipRect.Center.X;
                this.items[i].Y = this.items[i + 1].Y - ItemHeight;
                this.items[i].layout();
            }

            for (int i = idx + 1; i < this.items.Count; ++i)
            {
                this.items[i].X = this.clipRect.Center.X;
                this.items[i].Y = this.items[i - 1].Y + ItemHeight;
                this.items[i].layout();
            }
        }

        private void scrollUp()
        {
            for (int i = 0; i < this.items.Count; ++i)
            {
                this.items[i].Y -= ItemHeight;
            }
        }

        private void scrollDown()
        {
            for (int i = 0; i < this.items.Count; ++i)
            {
                this.items[i].Y += ItemHeight;
            }
        }

        private bool isFullyVisible(MenuItem item)
        {
            if ((item.Y - TextHeight / 2) >= this.clipRect.Top &&
                (item.Y + TextHeight / 2) <= this.clipRect.Bottom)
            {
                return true;
            }
            return false;
        }

        public void draw(SpriteBatch g)
        {
            //g.GraphicsDevice.RenderState.ScissorTestEnable = true;
            g.GraphicsDevice.ScissorRectangle = this.clipRect;

            g.Draw(this.background, this.clipRect, Color.White * ALPHA);

            g.Draw(this.item_bg, this.items[this.act_item].ItemBackgroundDrawingPosition, Color.White * ALPHA);

            foreach (MenuItem mi in this.items)
            {
                mi.draw(g, font);
            }

        }

        public virtual int processKey(KeyboardState kbs)
        {
            if (kbs.IsKeyDown(Keys.Up) && !this.keyUp)
            {
                if (this.act_item > 0)
                {
                    //Console.WriteLine(">>> menu up (K)");
                    this.act_item--;
                    this.menuMove.Play();
                    this.scrollDownTo(this.items[this.act_item]);
                    this.keyUp = true;
                    return (int)Results.CONSUMED;
                }
            }
            else if (kbs.IsKeyUp(Keys.Up) && this.keyUp)
            {
                //Console.WriteLine("<<< menu up (K)");
                this.keyUp = false;
                return (int)Results.CONSUMED;
            }
            else if (kbs.IsKeyDown(Keys.Up) && this.keyUp)
            {
                return (int)Results.WAIT;
            }

            if (kbs.IsKeyDown(Keys.Down) && !this.keyDown)
            {
                if (this.act_item < this.items.Count-1)
                {
                    //Console.WriteLine(">>> menu down (K)");
                    this.act_item++;
                    this.menuMove.Play();
                    this.scrollUpTo(this.items[this.act_item]);
                    this.keyDown = true;
                    return (int)Results.CONSUMED;
                }
            }
            else if (kbs.IsKeyUp(Keys.Down) && this.keyDown)
            {
                //Console.WriteLine("<<< menu down (K)");
                this.keyDown = false;
                return (int)Results.CONSUMED;
            }
            else if (kbs.IsKeyDown(Keys.Down) && this.keyDown)
            {
                return (int)Results.WAIT;
            }

            if (kbs.IsKeyDown(Keys.Enter) && !this.keyEnter)
            {
                //Console.WriteLine(">>> menu enter (K)");
                this.keyEnter = true;
                return this.items[this.act_item].Id;
            }
            else if (kbs.IsKeyUp(Keys.Enter) && this.keyEnter)
            {
                //Console.WriteLine("<<< menu enter (K)");
                this.keyEnter = false;
                return (int)Results.CONSUMED;
            }
            else if (kbs.IsKeyDown(Keys.Enter) && this.keyEnter)
            {
                return (int)Results.WAIT;
            }

            return (int)Results.NONE;
        }

        public virtual int processGamePad(MyGamePad gPad, PlayerIndex playerIndex)
        {
            if (gPad.isPressed(MyGamePad.MyDirections.DirUp, playerIndex) && !this.keyUp)
            {
                if (this.act_item > 0)
                {
                    //Console.WriteLine(">>> menu up (G)");
                    this.act_item--;
                    this.menuMove.Play();
                    this.scrollDownTo(this.items[this.act_item]);
                    this.keyUp = true;
                    return (int)Results.CONSUMED;
                }
            }
            else if (gPad.isReleased(MyGamePad.MyDirections.DirUp, playerIndex) && this.keyUp)
            {
                //Console.WriteLine("<<< menu up (G)");
                this.keyUp = false;
                return (int)Results.CONSUMED;
            }
            else if (gPad.isPressed(MyGamePad.MyDirections.DirUp, playerIndex) && this.keyUp)
            {
                return (int)Results.WAIT;
            }

            if (gPad.isPressed(MyGamePad.MyDirections.DirDown, playerIndex) && !this.keyDown)
            {
                if (this.act_item < this.items.Count - 1)
                {
                    //Console.WriteLine(">>> menu down (G)");
                    this.act_item++;
                    this.menuMove.Play();
                    this.scrollUpTo(this.items[this.act_item]);
                    this.keyDown = true;
                    return (int)Results.CONSUMED;
                }
            }
            else if (gPad.isReleased(MyGamePad.MyDirections.DirDown, playerIndex) && this.keyDown)
            {
                //Console.WriteLine("<<< menu down (G)");
                this.keyDown = false;
                return (int)Results.CONSUMED;
            }
            else if (gPad.isPressed(MyGamePad.MyDirections.DirDown, playerIndex) && this.keyDown)
            {
                return (int)Results.WAIT;
            }

            if (gPad.isPressed(MyGamePad.MyKeys.Key1, playerIndex) && !this.keyEnter)
            {
                //Console.WriteLine(">>> menu enter (G)");
                this.keyEnter = true;
                return this.items[this.act_item].Id;
            }
            else if (gPad.isReleased(MyGamePad.MyKeys.Key1, playerIndex) && this.keyEnter)
            {
                //Console.WriteLine("<<< menu enter (G)");
                this.keyEnter = false;
                return (int)Results.CONSUMED;
            }
            else if (gPad.isPressed(MyGamePad.MyKeys.Key1, playerIndex) && this.keyEnter)
            {
                return (int)Results.WAIT;
            }

            return (int)Results.NONE;
        }

        public void dispose()
        {
            this.items.Clear();

            this.background.Dispose();
            this.item_bg.Dispose();

            this.menuMove.Dispose();
        }
    }
}
