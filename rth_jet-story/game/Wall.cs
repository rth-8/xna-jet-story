using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class Wall : GameObject
    {
        private Texture2D bWall;

        public const int WALLS = 30;
        private static Texture2D[] walls = new Texture2D[WALLS];

        public static void loadImages(ContentManager content)
        {
            for (int i = 0; i < WALLS; ++i)
            {
                string name = String.Format("images/walls/wall_{0:00}", i);
                //Console.WriteLine(name);
                walls[i] = content.Load<Texture2D>(name);
            }
        }

        public static void disposeImages()
        {
            for (int i = 0; i < WALLS; ++i)
            {
                walls[i].Dispose();
            }
        }

        public enum Ids
        {
            WALL_00,
            WALL_01,
            WALL_02,
            WALL_03,
            WALL_04,
            WALL_05,
            WALL_06,
            WALL_07,
            WALL_08,
            WALL_09,
            WALL_10,
            WALL_11,
            WALL_12,
            WALL_13,
            WALL_14,
            WALL_15,
            WALL_16,
            WALL_17,
            WALL_18,
            WALL_19,
            WALL_20,
            WALL_21,
            WALL_22,
            WALL_23,
            WALL_24,
            WALL_25,
            WALL_26,
            WALL_27,
            WALL_28,
            WALL_29
        };

        public Wall() : base()
        {
        }

        public Wall(float m, float x, float y, float z, int w, int h)
            : base(m, x, y, w, h)
        {
        }

        public void setBitmap(int id)
        {
            if (id >= (int)Ids.WALL_00 && id <= (int)Ids.WALL_29)
            {
                this.bWall = walls[id];
                this.setDimension(bWall.Width, bWall.Height);
            }
            else
            {
                //???
            }
        }

        public override void draw(SpriteBatch g)
        {
            g.Draw(
                this.bWall,
                new Vector2((int)getPosition().X, (int)getPosition().Y),
                Color.White);
        }

        public override void move(float dt)
        {
            //walls don't move
        }
    }
}
