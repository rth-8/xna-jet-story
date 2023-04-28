using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story
{
    class InfoBarRect
    {
        private Vector2 pos;
        private Color col;
        private int citac;
        private int RECT_W;
        private int RECT_H;

        private const int prc = 100;

        private Texture2D rect;

        public InfoBarRect(Game parent)
        {
            this.pos = new Vector2(0, 0);
            RECT_W = Game.recalculate(21);
            RECT_H = Game.recalculate(21);

            this.col = Color.White;

            this.citac = 0;

            this.rect = this.createTexture(parent, RECT_W, RECT_H, Color.White);
        }

        public InfoBarRect(Game parent, int x, int y, Color c)
        {
            this.pos = new Vector2(x, y);
            RECT_W = Game.recalculate(21);
            RECT_H = Game.recalculate(21);

            this.setColor(c);

            this.citac = 0;

            this.rect = this.createTexture(parent, RECT_W, RECT_H, Color.White);
        }

        private Texture2D createTexture(Game parent, int w, int h, Color refColor)
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

        public void setColor(Color c)
        {
            this.col = c;
        }

        public void draw(SpriteBatch g)
        {
            this.citac++;
            if (this.citac == 5)
            {
                int change;
                double dif = 1.3;
                change = Game.rand.Next(prc);
                if (change > (prc / dif))
                {
                    this.col.R = (byte)(Game.rand.Next(234) + 20);
                    this.col.G = (byte)(Game.rand.Next(234) + 20);
                    this.col.B = (byte)(Game.rand.Next(234) + 20);
                }
                this.citac = 0;
            }
            g.Draw(this.rect, this.pos, this.col);
        }
    }
}
