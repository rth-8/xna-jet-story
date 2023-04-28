using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class EType1 : Enemy
    {
        private EType1_Shot shot;
        private int shot_counter;

        private int subtype;

        private static Texture2D enemy_01_left;
        private static Texture2D enemy_01_right;

        public EType1(int sub) : base() 
        {
            this.initEnemy(sub);
        }

        public EType1(float m, float x, float y, float z, int w, int h, int sub) 
            : base(m, x, y, z, w, h)
        {
            this.initEnemy(sub);
        }

        private void initEnemy(int sub) 
        {
	        this.subtype = sub;
        	
	        this.shot = new EType1_Shot();
            this.shot_counter = 0;

	        if (sub == 0) 
            {
		        this.shot.setOrient(false); //doleva
	        }
	        else if (sub == 1) 
            {
		        this.shot.setOrient(true); //doprava
	        }

            this.col = Game.getRandomColor();
        }

        public override void draw(SpriteBatch g) 
        {
            if (this.orientace) //doprava
            {
                g.Draw(enemy_01_right,
                    new Vector2((int)this.getPosition().X, (int)this.getPosition().Y),
                    this.col);
            }
            else
            {
                g.Draw(enemy_01_left,
                    new Vector2((int)this.getPosition().X, (int)this.getPosition().Y),
                    this.col);
            }
        }

        public override void move(GameObject o, float dt)
        {
	        //nepritel samotny je staticky, pouze zajisteni strileni
	        if (
		        (
		         (o.getPosition().Y + Game.recalculate(10) >= 
			         this.getPosition().Y) &&
		         (o.getPosition().Y + Game.recalculate(10) <= 
			         this.getPosition().Y + this.getH())
		        ) ||
		        (
		         (o.getPosition().Y + o.getH() - Game.recalculate(10) >= 
			         this.getPosition().Y) &&
	             (o.getPosition().Y + o.getH() - Game.recalculate(10) <= 
	    	         this.getPosition().Y + this.getH())
	            )
	           ) 
	        {
                if (this.shot.isShown() == false
                    && this.shot_counter == 0)
                {
			        this.shot.start(this.getPosition());
			        this.shot.show();
                    this.shot_counter = 50;
		        }
	        }

            if (this.shot_counter > 0)
            {
                this.shot_counter--;
            }
        }

        public override Shot getShot()
        {
	        return this.shot;
        }

        public int getSubType()
        {
	        return this.subtype;
        }

        public static void loadImages(ContentManager content)
        {
            enemy_01_left = content.Load<Texture2D>("images/enemies/enemy_01_0");
            enemy_01_right = content.Load<Texture2D>("images/enemies/enemy_01_1");
        }

        public static void disposeImages()
        {
            enemy_01_left.Dispose();
            enemy_01_right.Dispose();
        }
    }
}
