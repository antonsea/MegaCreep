using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep
{
    public class GameStateManager : GameComponent
    {
        public event EventHandler OnStateChange;

        Stack<GameState> gameStates = new Stack<GameState>();

        const int startDrawOrder = 5000;
        const int drawOrderInc = 100;
        int drawOrder;

        public GameState CurrentState
        {
            get { return gameStates.Peek(); }
        }

        public GameStateManager (Game game)
            : base(game)
        {
            drawOrder = startDrawOrder;
        }
        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region Method Region
        public void PopState()
        {
            if(gameStates.Count > 0)
            {
                RemoveState();
                drawOrder -= drawOrderInc;

                OnStateChange?.Invoke(this, null);
            }
        }

        private void RemoveState()
        {
            GameState state = gameStates.Peek();
            OnStateChange -= state.StateChange;
            Game.Components.Remove(state);
            gameStates.Pop();
        }

        public void PushState(GameState newState)
        {
            drawOrder += drawOrderInc;
            newState.DrawOrder = drawOrder;

            AddState(newState);

            OnStateChange?.Invoke(this, null);
        }

        private void AddState(GameState newState)
        {
            gameStates.Push(newState);
            Game.Components.Add(newState);
            OnStateChange += newState.StateChange;
        }

        public void ChangeState(GameState newState)
        {
            while (gameStates.Count > 0)
                RemoveState();

            newState.DrawOrder = startDrawOrder;
            drawOrder = startDrawOrder;

            AddState(newState);
            //Old comment from from "Monster Arena": I don't think OnStateChange will be subscribed to in this method...
            OnStateChange?.Invoke(this, null);

        }

        #endregion
    }
}
