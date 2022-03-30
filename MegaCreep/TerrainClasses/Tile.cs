using MegaCreep.BuildingClasses;
using MegaCreep.CreepClasses;
using MegaCreep.GameScreens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.TerrainClasses
{
    public class Tile
    {
        #region Tileset region
        int tileIndex;
        List<int> tileEdges;
        #endregion
        #region Spatial Fields and Properties
        Vector2 coordinates;
        public Vector2 Coordinates
        {
            get { return coordinates; }
        }
        //Calling these X & Y properties just makes the code look cleaner later on in the future
        public int X
        {
            get { return (int)coordinates.X; }
        }
        public int Y
        {
            get { return (int)coordinates.Y; }
        }

        //Keeping track of the center pixel is useful for calculations like bullets firing, sending packets, etc.
        Vector2 centerPixel;
        public Vector2 CenterPixel
        {
            get { return centerPixel; }
        }

        //Keeping track of the draw rectangle helps figuring out where to draw the tile
        Rectangle drawRectangle;
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }
        #endregion

        private List<Tile> neighbors;
        public List<Tile> Neighbors
        {
            get { return neighbors; }
        }

        //I have a feeling this might be bad code practice, but I will have each Terrain will refer to its own creep and each creep refers to each terrain.
        //I don't know if having two things reffering to each other is good or bad, but it makes the code easier to read later on
        public Creep Creep
        {
            get; set;
        }
        public CreepGenerator CreepGenerator
        {
            get; set;
        }

        public Building Building
        {
            get; set;
        }

        private int elevation;
        public int Elevation
        {
            get { return elevation; }
        }

        //TotalHeight property returns elevation + creep thickness
        public float TotalHeight
        {
            get
            {
                if(Creep != null)
                {
                    return elevation + Creep.Amount;
                }
                else
                {
                    return elevation;
                }
            }
        }

        public Tile(int tileIndex, int elevation, int x, int y)
        {
            //This sets which image is used to draw the tile
            this.tileIndex = tileIndex;
            //If this terrain borders a terrain that is lower than it, it will need to draw border lines. This list keeps track of which lines to draw
            tileEdges = new List<int>();

            coordinates = new Vector2(x, y);
            centerPixel = new Vector2(x * Game1.TileSize + Game1.TileSize / 2, y * Game1.TileSize + Game1.TileSize / 2);
            drawRectangle = new Rectangle(x * Game1.TileSize, y * Game1.TileSize, Game1.TileSize, Game1.TileSize);

            this.elevation = elevation;
            CreepGenerator = null;
            Building = null;
        }

        public void DetermineNeighbors()
        {
            //I call this method once I generate the corresponding terrain map in the world panel
            //By having a list of all the neighbors of this tile make it easy to cycle through when deteriming things like creep flow
            //At the same time I will determine if any of the neighbors are at a lower elevation at this tile, if so I will add the corresponding line edge to outline the terrain
            neighbors = new List<Tile>();

            //I also need to make sure the neighbors are within the actual game map
            //Orthogonal neighbors

            if (Y > 0)
            {
                neighbors.Add(GameScreen.World.Map[X, Y - 1]);
                if (elevation > GameScreen.World.Map[X, Y - 1].elevation)
                    tileEdges.Add(8);
            }

            if (Y < GameScreen.World.TilesHigh - 1)
            {
                neighbors.Add(GameScreen.World.Map[X, Y + 1]);
                if (elevation > GameScreen.World.Map[X, Y + 1].elevation)
                    tileEdges.Add(9);
            }

            if (X > 0)
            {
                neighbors.Add(GameScreen.World.Map[X - 1, Y]);
                if (this.elevation > GameScreen.World.Map[X - 1, Y].elevation)
                    tileEdges.Add(6);
            }
            if (X < GameScreen.World.TilesWide - 1)
            {
                neighbors.Add(GameScreen.World.Map[X + 1, Y]);
                if (this.elevation > GameScreen.World.Map[X + 1, Y].elevation)
                    tileEdges.Add(7);
            }

            //Diagonal neighbors
            if (Y > 0 && X > 0)
                neighbors.Add(GameScreen.World.Map[X - 1, Y - 1]);
            if (Y > 0 && X < GameScreen.World.TilesWide - 1)
                neighbors.Add(GameScreen.World.Map[X + 1, Y - 1]);
            if (Y < GameScreen.World.TilesHigh - 1 && X > 0)
                neighbors.Add(GameScreen.World.Map[X - 1, Y + 1]);
            if (Y < GameScreen.World.TilesHigh - 1 && X < GameScreen.World.TilesWide - 1)
                neighbors.Add(GameScreen.World.Map[X + 1, Y + 1]);

        }

        public bool IsOccupied()
        {
            //Call this method to see you can add another structure to this tile
            return CreepGenerator != null || Building != null;
        }

        public void Draw()
        {
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[tileIndex], Color.White);
            foreach (int index in tileEdges)
                Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[index], Color.White);
        }
    }
}
