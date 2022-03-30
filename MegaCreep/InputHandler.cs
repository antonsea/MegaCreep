using MegaCreep.GameScreens;
using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep
{
    public class InputHandler : GameComponent
    {
        static MouseState mouseState;
        static MouseState lastMouseState;

        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        public static MouseState LastMouseState
        {
            get { return lastMouseState; }
        }

        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyBoardState
        {
            get { return lastKeyboardState; }
        }

        public InputHandler (Game game)
            :base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public static void Flush()
        {
            lastMouseState = mouseState;
            lastKeyboardState = keyboardState;
        }

        public static bool LeftClick()
        {
            return mouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightClick()
        {
            return mouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released;

        }

        public static Vector2 MousePixelCoordinates()
        {
            return new Vector2(mouseState.X, mouseState.Y);
        }

        public static Tile MouseTile()
        {
            Vector2 mouseTileCoordinates = new Vector2(mouseState.X / Game1.TileSize, mouseState.Y / Game1.TileSize);
            return GameScreen.World.Map[(int)mouseTileCoordinates.X, (int)mouseTileCoordinates.Y];

        }

        public static bool MouseInWorldPanel(out Tile destination)
        {
            bool inPanel = mouseState.X >= 0 && mouseState.X < Game1.WorldPanel.Width && mouseState.Y >= 0 && mouseState.Y < Game1.WorldPanel.Height;
            if (inPanel)
                destination = MouseTile();
            else
                destination = null;
            return inPanel;
        }

        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }



    }
}
