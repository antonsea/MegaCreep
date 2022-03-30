using MegaCreep.BuildingClasses;
using MegaCreep.GameScreens.GamePanels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.GameScreens
{
    public class GameScreen : BaseGameState
    {
        //How quickly the game updates (in seconds). Not necessarily how quickly creep and other components update
        //This is necessary so faster computers won't play faster (i.e. creep flows faster) than slower computers
        double timePerUpdate = 0.016666; //Provided nothing that is eating up computational power, the game will try to send an update tick 60 times per second.
        double timeSinceLastUpdate = 0;
        #region Panel Fields/Properties
        static World world;
        public static World World
        {
            get { return world; }
        }

        static BuildPanel buildPanel;
        public static BuildPanel BuildPanel
        {
            get { return buildPanel; }
        }

        static ResourcePanel resourcePanel;
        public static ResourcePanel ResourcePanel
        {
            get { return resourcePanel; }
        }
        #endregion
        public GameScreen(Game game, GameStateManager manager)
            :base(game, manager)
        {
            //The game screen will be made up a few panels that will update independtly of each other:
            //World (where most of the game play will be)
            //Build panel (where the player will select buildings to build)
            //Resource Panel (info about current resource supply and production)
            //And in the future I will add a right side panel that will include mini map (to have larger game maps) and info and building options when selecting a building
            Rectangle worldRectangle = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight);

            world = new World(game, worldRectangle);
            Components.Add(world);
        }

        #region XNA Method Region

        public override void Initialize()
        {
            world.GenerateWorld();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboard();
            //World.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            world.Draw();
            base.Draw(gameTime);
        }
        #endregion

        private void HandleKeyboard()
        {
            if(InputHandler.KeyPressed(Keys.D1))
            {
                World.BuidingManager.SetPreview(BuildingType.Turret);
            }

            if (InputHandler.KeyPressed(Keys.D2))
            {
                World.BuidingManager.SetPreview(BuildingType.Headquarters);
            }

            else if(InputHandler.KeyPressed(Keys.Escape))
            {
                //If we are already previewing a building, we will cancel that preview when we hit escape
                if(World.BuidingManager.PreviewBuildingType != BuildingType.None)
                {
                    World.BuidingManager.SetPreview(BuildingType.None);
                }
                //Otherwise we will exit the game
                else
                    Game.Exit();
            }
           
        }
    }
}
