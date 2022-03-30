using MegaCreep.AnimationClasses;
using MegaCreep.BuildingClasses;
using MegaCreep.CreepClasses;
using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.GameScreens.GamePanels
{
    public class World : BasePanel
    {
        //How quickly the game updates (in seconds). Not necessarily how quickly creep and other components update
        //This is necessary so faster computers won't play faster (i.e. creep flows faster) than slower computers
        double timePerUpdate = 0.016666; //Provided nothing that is eating up computational power, the game will try to send an update tick 60 times per second.
        double timeSinceLastUpdate = 0;
        #region Dimensions
        int tilesWide;
        public int TilesWide
        {
            get { return tilesWide; }
        }
        int tilesHigh;
        public int TilesHigh
        {
            get { return tilesHigh; }
        }

        #endregion
        public Tile[,] map;
        public Tile[,] Map
        {
            get { return map; }
        }

        #region Manager fields
        private static CreepManager creepManager;
        public static CreepManager CreepManager
        {
            get { return creepManager; }
        }

        private static BuildingManager buildingManager;
        public static BuildingManager BuidingManager
        {
            get { return buildingManager; }
        }

        private static AnimationManager animationManager;
        public static AnimationManager AnimationManager
        {
            get { return animationManager; }
        }
        #endregion

        public World(Game game, Rectangle rectangle)
            : base(game, rectangle)
        {

            tilesWide = rectangle.Width;
            tilesHigh = rectangle.Height;

            map = new Tile[tilesWide, tilesHigh];

            creepManager = new CreepManager();
            buildingManager = new BuildingManager();
            animationManager = new AnimationManager();
        }
        public void GenerateWorld()
        {
            //Generate an elevation map, e, using Perlin Noise. Each x, y coordinate is given a value between 0 - 1.0
            //I then make certain ranges correspond to certain elevations and generate a new terrain of the corresponing Terrian class.
            float[,] e = PerlinNoise.GenerateNoiseMap(tilesWide, tilesHigh, 2f, 1);
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    if (e[x, y] < 0.17f)
                    {
                        map[x, y] = new Ground1(x, y);
                    }

                    else if (e[x, y] < 0.34f)
                    {
                        map[x, y] = new Ground2(x, y);
                    }
                    else if (e[x, y] < 0.5f)
                    {
                        map[x, y] = new Ground3(x, y);
                    }
                    else if (e[x, y] < 0.68f)
                    {
                        map[x, y] = new Ground4(x, y);
                    }
                    else if (e[x, y] < 0.85f)
                    {
                        map[x, y] = new Ground5(x, y);
                    }
                    else
                    {
                        map[x, y] = new Ground6(x, y);
                    }

                }
            }

            //I then go through each terrain to keep track of its neighbors in a simple list. This simplifies later calculations.
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    map[x, y].DetermineNeighbors();
                }
            }
        }

        private void HandleInput()
        {
            Tile dest;
            if(InputHandler.RightClick() && InputHandler.MouseInWorldPanel(out dest))
            {
                creepManager.AddGenerator(dest);
            }

            else if(InputHandler.LeftClick())
            {
                //We don't need to check to see if the mouse is in the worldpanel because the act of creating the preview does so
                if (buildingManager.PreviewBuildingType != BuildingType.None)
                    buildingManager.AddBuilding();
            }

            else if(InputHandler.KeyPressed(Keys.X) && InputHandler.MouseInWorldPanel(out dest))
            {
                if(dest.Building != null)
                {
                    buildingManager.RemoveBuilding(dest.Building);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {

            HandleInput();
            

            timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            if(timeSinceLastUpdate >= timePerUpdate)
            {
                timeSinceLastUpdate = 0;
                creepManager.Update();
                buildingManager.Update();
                animationManager.Update();

            }
        }

        public void Draw ()
        {
            //I need to look up all the overloads to the SpriteBatch function and understand what they do
            Game1.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                null);

            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    map[x, y].Draw();
                }
            }

            creepManager.Draw();
            buildingManager.Draw();
            animationManager.Draw();
            //base.Draw(gameTime);

            Game1.SpriteBatch.End();
        }


    }
}
