using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.GameScreens
{
    public abstract partial class BaseGameState : GameState
    {
        protected Game1 GameRef;

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            GameRef = (Game1)game;
        }

        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            base.LoadContent();
        }
    }
}
