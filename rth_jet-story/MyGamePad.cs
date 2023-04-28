using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace rth_jet_story
{
    class MyGamePad
    {
        private InputManager inputManager;
        private ExtendedPlayerIndex gamePad;

        public enum MyKeys
        {
            Up,
            Down,
            Left,
            Right,
            Key1,
            Key2,
            Key3,
            Key4,
            Key5,
            Key6,
            Key7,
            Key8,
            Key9,
            LStickUp,
            LStickDown,
            LStickLeft,
            LStickRight,
            RStickUp,
            RStickDown,
            RStickLeft,
            RStickRight,
            Back
        }

        public enum MyDirections
        {
            DirUp,
            DirDown,
            DirLeft,
            DirRight
        }

        public MyGamePad(GameServiceContainer container, IntPtr windowHandle)
        {
            //Console.WriteLine("> MyGamePad");

            this.inputManager = new InputManager(container, windowHandle);
            this.gamePad = ExtendedPlayerIndex.Five;

            Console.WriteLine(this.inputManager.GetGamePad(this.gamePad).Name);
            Console.WriteLine(this.inputManager.GetGamePad(this.gamePad).GetState().IsConnected);

            //Console.WriteLine("> MyGamePad");
        }

        public bool isPressed(MyKeys key, PlayerIndex playerIndex)
        {
            this.inputManager.Update();

            if (this.inputManager.GetGamePad(this.gamePad).GetState().IsConnected == false)
                return false;

            if (key == MyKeys.Back && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.Back == ButtonState.Pressed)
            {
                //Console.WriteLine("MyGamePad: BACK");
                return true;
            }

            if (key == MyKeys.Up && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.Y > 0)
            {
                //Console.WriteLine("MyGamePad: UP");
                return true;
            }
            if (key == MyKeys.Down && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.Y < 0)
            {
                //Console.WriteLine("MyGamePad: DOWN");
                return true;
            }
            if (key == MyKeys.Left && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.X < 0)
            {
                //Console.WriteLine("MyGamePad: LEFT");
                return true;
            }
            if (key == MyKeys.Right && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.X > 0)
            {
                //Console.WriteLine("MyGamePad: RIGHT");
                return true;
            }
            if (key == MyKeys.Key1 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.A == ButtonState.Pressed)
            {
                //Console.WriteLine("MyGamePad: A");
                return true;
            }
            if (key == MyKeys.Key2 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.B == ButtonState.Pressed)
            {
                //Console.WriteLine("MyGamePad: B");
                return true;
            }
            if (key == MyKeys.Key3 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.X == ButtonState.Pressed)
            {
                //Console.WriteLine("MyGamePad: X");
                return true;
            }
            if (key == MyKeys.Key4 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.Y == ButtonState.Pressed)
            {
                //Console.WriteLine("MyGamePad: Y");
                return true;
            }
            //TODO

            Vector2 ls = this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left;
            Vector2 rs = this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Right;

            if (key == MyKeys.LStickUp && inRange(ls.Y))
            {
                return true;
            }
            if (key == MyKeys.LStickDown && inRange(-ls.Y))
            {
                return true;
            }
            if (key == MyKeys.LStickLeft && inRange(-ls.X))
            {
                return true;
            }
            if (key == MyKeys.LStickRight && inRange(ls.X))
            {
                return true;
            }

            return false;
        }

        public bool isPressed(MyDirections dir, PlayerIndex playerIndex)
        {
            switch (dir)
            {
                case MyDirections.DirUp:
                    if (isPressed(MyGamePad.MyKeys.LStickUp, PlayerIndex.One) ||
                        isPressed(MyGamePad.MyKeys.Up, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirDown:
                    if (isPressed(MyGamePad.MyKeys.LStickDown, PlayerIndex.One) ||
                        isPressed(MyGamePad.MyKeys.Down, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirLeft:
                    if (isPressed(MyGamePad.MyKeys.LStickLeft, PlayerIndex.One) ||
                        isPressed(MyGamePad.MyKeys.Left, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirRight:
                    if (isPressed(MyGamePad.MyKeys.LStickRight, PlayerIndex.One) ||
                        isPressed(MyGamePad.MyKeys.Right, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public bool isReleased(MyKeys key, PlayerIndex playerIndex)
        {
            this.inputManager.Update();

            if (this.inputManager.GetGamePad(this.gamePad).GetState().IsConnected == false)
                return false;

            if (key == MyKeys.Back && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.Back == ButtonState.Released)
            {
                return true;
            }

            if (key == MyKeys.Up && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.Y == 0)
            {
                return true;
            }
            if (key == MyKeys.Down && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.Y == 0)
            {
                return true;
            }
            if (key == MyKeys.Left && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.X == 0)
            {
                return true;
            }
            if (key == MyKeys.Right && this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left.X == 0)
            {
                return true;
            }
            if (key == MyKeys.Key1 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.A == ButtonState.Released)
            {
                return true;
            }
            if (key == MyKeys.Key2 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.B == ButtonState.Released)
            {
                return true;
            }
            if (key == MyKeys.Key3 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.X == ButtonState.Released)
            {
                return true;
            }
            if (key == MyKeys.Key4 && 
                this.inputManager.GetGamePad(this.gamePad).GetState().Buttons.Y == ButtonState.Released)
            {
                return true;
            }

            Vector2 ls = this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Left;
            Vector2 rs = this.inputManager.GetGamePad(this.gamePad).GetState().ThumbSticks.Right;

            if (key == MyKeys.LStickUp && !inRange(ls.Y))
            {
                return true;
            }
            if (key == MyKeys.LStickDown && !inRange(-ls.Y))
            {
                return true;
            }
            if (key == MyKeys.LStickLeft && !inRange(-ls.X))
            {
                return true;
            }
            if (key == MyKeys.LStickRight && !inRange(ls.X))
            {
                return true;
            }

            return false;
        }

        public bool isReleased(MyDirections dir, PlayerIndex playerIndex)
        {
            switch (dir)
            {
                case MyDirections.DirUp:
                    if (isReleased(MyGamePad.MyKeys.LStickUp, PlayerIndex.One) ||
                        isReleased(MyGamePad.MyKeys.Up, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirDown:
                    if (isReleased(MyGamePad.MyKeys.LStickDown, PlayerIndex.One) ||
                        isReleased(MyGamePad.MyKeys.Down, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirLeft:
                    if (isReleased(MyGamePad.MyKeys.LStickLeft, PlayerIndex.One) ||
                        isReleased(MyGamePad.MyKeys.Left, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
                case MyDirections.DirRight:
                    if (isReleased(MyGamePad.MyKeys.LStickRight, PlayerIndex.One) ||
                        isReleased(MyGamePad.MyKeys.Right, PlayerIndex.One))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private bool inRange(float value)
        {
            if (value >= 0.2 && value <= 1) return true;
            return false;
        }
    }
}
