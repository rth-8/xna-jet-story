using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story
{
    class MenuItem
    {
        public enum Layouts
        {
            LEFT,
            RIGHT,
            CENTER
        }

        private int posx;
        public int X 
        {
            set { this.posx = value; }
            get { return this.posx; }
        }

        private int posy;
        public int Y
        {
            set { this.posy = value; }
            get { return this.posy; }
        }

        private int id;
        public int Id
        {
            get { return this.id; }
        }

        public Vector2 ItemBackgroundDrawingPosition
        {
            get { return new Vector2(
                posx - this.parent.Width/2 + parent.LeftMargin, 
                posy - this.parent.ItemHeight / 2); }
        }

        private Menu parent;

        private string[] texts;
        private Layouts[] layouts;
        private Vector2[] positions;

        private int text_height;

        public MenuItem(Menu parent, int id, string[] items)
        {
            this.parent = parent;

            this.id = id;

            this.text_height = (int)parent.Font.MeasureString("W").Y;

            this.texts = new string[items.Length];
            this.layouts = new Layouts[items.Length];
            this.positions = new Vector2[items.Length];

            for (int i = 0; i < items.Length; ++i)
            {
                this.texts[i] = items[i];
            }
            
            this.setLayout(Layouts.CENTER);
        }

        public MenuItem(Menu parent, int id, string[] items, Layouts layout)
            : this(parent, id, items)
        {
            this.setLayout(layout);
        }

        public MenuItem(Menu parent, int id, string[] items, Layouts[] layouts)
            : this(parent, id, items)
        {
            this.setLayouts(layouts);
        }

        public void setLayout(Layouts layout)
        {
            for (int i = 0; i < layouts.Length; ++i)
            {
                this.layouts[i] = layout;
            }
            //this.layout();
        }

        public void setLayouts(Layouts[] layouts)
        {
            for (int i = 0; i < layouts.Length; ++i)
            {
                this.layouts[i] = layouts[i];
            }
            //this.layout();
        }

        public void layout()
        {
            if (this.texts.Length == 0)
            {
                return;
            }

            if (this.texts.Length % 2 == 0)
            {
                //Console.WriteLine("layout even");
                this.layoutEven();
            }
            else
            {
                //Console.WriteLine("layout odd");
                this.layoutOdd();
            }
        }

        private void layoutEven() //sudy pocet
        {
            int idx = this.texts.Length / 2;

            int segW = (parent.Width - parent.LeftMargin - parent.RightMargin)
                / this.texts.Length;

            int y = posy - text_height/2;

            int baseW = segW / 2;

            this.layoutItem(idx, posx + baseW, y, segW);
            this.layoutItem(idx - 1, posx - baseW, y, segW);

            int cnt = 0;
            for (int i = idx - 2; i >= 0; --i)
            {
                Console.WriteLine(i + ": x = " + this.positions[i + 1].X);
                this.layoutItem(i, posx - (++cnt)*segW - baseW, y, segW);
            }

            cnt = 0;
            for (int i = idx + 1; i < texts.Length; ++i)
            {
                Console.WriteLine(i + ": x = " + this.positions[i - 1].X);
                this.layoutItem(i, posx + (++cnt)*segW + baseW, y, segW);
            }
        }

        private void layoutOdd() //lichy pocet
        {
            int idx = this.texts.Length / 2;
            //Console.WriteLine("idx = " + idx);

            int segW = (parent.Width - parent.LeftMargin - parent.RightMargin)
                / this.texts.Length;
            //Console.WriteLine("segW = " + segW);

            int y = posy - text_height / 2;

            this.layoutItem(idx, posx, y, segW);

            int cnt = 0;
            for (int i = idx - 1; i >= 0; --i)
            {
                Console.WriteLine(i + ": x = " + this.positions[i + 1].X);
                this.layoutItem(i, posx - (++cnt)*segW, y, segW);
            }

            cnt = 0;
            for (int i = idx + 1; i < texts.Length; ++i)
            {
                Console.WriteLine(i + ": x = " + this.positions[i - 1].X);
                this.layoutItem(i, posx + (++cnt)*segW, y, segW);
            }
        }

        private void layoutItem(int idx, int x, int y, int segmentWidth)
        {
            //Console.WriteLine("layoutItem(" + idx + ", " + x + ", " + y + ", " + segmentWidth + ")");
            switch (this.layouts[idx])
            {
                case Layouts.CENTER:
                    this.positions[idx] = new Vector2(
                        x - parent.Font.MeasureString(texts[idx]).X / 2,
                        y);
                    break;

                case Layouts.LEFT:
                    this.positions[idx] = new Vector2(
                        x - segmentWidth / 2,
                        y);
                    break;

                case Layouts.RIGHT:
                    this.positions[idx] = new Vector2(
                        x + segmentWidth / 2 
                            - parent.Font.MeasureString(texts[idx]).X,
                        y);
                    break;
            }
        }

        public void setText(int column, string newText)
        {
            this.texts[column] = newText;
            this.layout();
        }

        public void draw(SpriteBatch g, SpriteFont font)
        {
            for (int i = 0; i < this.texts.Length; ++i)
            {
                //Console.WriteLine("draw: " + this.texts[i]);
                g.DrawString(font, this.texts[i], this.positions[i], Color.White);
            }
        }
    }
}
