using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep
{
    public abstract partial class GameState : DrawableGameComponent
    {
        List<GameComponent> childComponents;

        public List<GameComponent> Components
        {
            get { return childComponents; }
        }

        protected GameStateManager GameStateManager;
        GameState tag;

        public GameState Tag
        {
            get { return tag; }
        }

        public GameState(Game game, GameStateManager manager)
            : base(game)
        {
            GameStateManager = manager;
            childComponents = new List<GameComponent>();
            tag = this;
        }

        #region XNA Drawable Game Component Methods
        public override void Initialize()
        {
            foreach (GameComponent component in childComponents)
                component.Initialize();
            base.Initialize();  
        }

        public override void Update(GameTime gameTime)
        {
            foreach(GameComponent component in childComponents)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawComponent;
            foreach(GameComponent component in childComponents)
            {
                if(component is DrawableGameComponent)
                {
                    drawComponent = component as DrawableGameComponent;
                    if (drawComponent.Visible)
                        drawComponent.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
        #endregion

        #region GameState Method Region

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (GameStateManager.CurrentState == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach(GameComponent component in childComponents)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;

            foreach(GameComponent component in childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
        #endregion
    }
}
