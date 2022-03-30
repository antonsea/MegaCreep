using MegaCreep.GameScreens;
using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MegaCreep.BuildingClasses
{
    public abstract class Building
    {
        //The game sends 60 update ticks per second
        protected int updateRate;
        protected int ticksSinceUpdate = 0;


        protected BuildingManager manager;
        int[] tileIndices;

        //The actual map tiles this building takes up:
        protected Tile[] tiles;

        protected Vector2 centerPixel;
        public Vector2 CenterPixel
        {
            get { return centerPixel; }
        }

        protected Dictionary<Building, Connection> connections;

        public Dictionary<Building, Connection> Connections
        {
            get { return connections; }
        }
        
        protected Headquarters connectedHQ;

        public Headquarters ConnectedHQ
        {
            get { return connectedHQ; }

        }
        protected List<Building> pathToHQ;
        public List<Building> PathToHQ
        {
            get { return pathToHQ; }
        }
        protected float distanceToHQ;
        public float DistanceToHQ
        {
            get { return distanceToHQ; }

        }

        //Destroyed field/property is used for ammo/energy packets to check if their target exists. If true, then the packet is removed
        private bool destroyed;
        public bool Destroyed
        {
            get { return destroyed; }
        }

        public Building(BuildingManager manager, Tile[] tiles, int[] tileIndices)
        {
            this.manager = manager;
            this.tiles = tiles;
            this.tileIndices = tileIndices;

            centerPixel = new Vector2(
                ((1 + tiles.Last().X - tiles[0].X) / 2 + tiles[0].X)* Game1.TileSize,
                ((1 + tiles.Last().Y - tiles[0].Y) / 2 + tiles[0].Y) * Game1.TileSize);

            connections = new Dictionary<Building, Connection>();
            pathToHQ = new List<Building>();
            distanceToHQ = -1;
            FindConnections();
        }

        public void FindConnections()
        {
            //maxDistance is in tiles
            //Right now its hard coded in this method, but I'll likely change it where different buildings can have different connection lengths
            int maxDistance = 10;
            foreach (Building building in manager.Buildings)
            {
                float distance = Vector2.Distance(this.centerPixel, building.centerPixel);

                if (distance < maxDistance * Game1.TileSize)
                {
                    Connection connection = new Connection(this, building);
                    connections.Add(building, connection);

                }

            }
        }
        public void Build()
        {
            foreach (Tile tile in tiles)
            {
                tile.Building = this;
            }

            //Once we actually build this building, we need to make sure that buildings connected to this one know they are actually connected
            foreach (Building building in connections.Keys)
            {
                building.Connections.Add(this, connections[building]);
                manager.Connections.Add(connections[building]);
            }
            //Then we want to check to see if this building is actually connected to a headquarters, we do this by checking to see if its connected to a building that is already connected
            CheckPathToHQ();

        }
        public void CheckPathToHQ()
        {
            //We call CheckPathToHQ for two key reasons
            //1. We create a new building and need to see if its connected to a building that has a path to HQ
            //2. We change the path to HQ for a building (i.e. due to a more efficent route being built) and then ask all its neighbors to recalculate (and their neighbors if needed, etc)

            //First we set the variable shortestPathToHQ to the current distanceToHQ (which is -1 if this is a brand new building)
            float shortestPathToHQ = distanceToHQ;
            Building directionToHQ = null;
            //iterate through all the neighbors that are connected to HQ (no reason to check a building thats not connected, that connection won't lead anywhere).
            foreach (Building building in connections.Keys.Where(b => b.ConnectedHQ != null))
            {
                //Then we check the building we are currently iterating through and figure out how far away we would be from the HQ if we were connected through this building
                //which is the buildings distance to HQ + the lenght of connection between buildings
                float currentDistanceToHQ = building.DistanceToHQ + connections[building].Length;

                //if we iterate over a headquarters then stop, we are all set.
                //I added && connectedHQ == null because otherwise an infinite loop would occur if you connected two buildings to HQ that were connected to each other
                if(building is Headquarters && connectedHQ == null)
                {
                    directionToHQ = building;
                    shortestPathToHQ = currentDistanceToHQ;
                    break;
                }

                //otherwise, if connecting to HQ through the building being iterated yields the shortest path, then thats the one we want to connect to.
                else if (currentDistanceToHQ < shortestPathToHQ || shortestPathToHQ == -1)
                {
                    directionToHQ = building;
                    shortestPathToHQ = currentDistanceToHQ;
                }

            }

            //directionToHQ is null under two circumstances
            //1. We built a building that is connected to buildings that are NOT connected to HQ
            //2. We did not find a path that was shorter than the current path we had

            //If directionToHQ is not null, that means we have a new path to HQ and therefore need to ask all our neighbors to loop through and check to see if they have a more optimized path
            if (directionToHQ != null)
            {
                connectedHQ = directionToHQ.ConnectedHQ;
                pathToHQ = new List<Building>(directionToHQ.PathToHQ);
                pathToHQ.Add(this);
                distanceToHQ = shortestPathToHQ;
                foreach (Building building in connections.Keys)
                {
                    building.CheckPathToHQ();
                }
            }
        }

        public void ClearPathToHQ()
        {
            connectedHQ = null;
            pathToHQ = new List<Building>();
            distanceToHQ = -1;
        }
       

        public void Destroy()
        {
            destroyed = true;
            manager.Buildings.Remove(this);
            foreach(Tile tile in tiles)
            {
                tile.Building = null;
            }

            foreach (Building building in connections.Keys)
            {
                building.Connections.Remove(this);
                manager.Connections.Remove(connections[building]);
            }

        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            if(connectedHQ == null)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    Game1.SpriteBatch.Draw(Game1.Tileset.Image, tiles[i].DrawRectangle, Game1.Tileset.SourceRectangles[tileIndices[i]], Color.Black);
                }
            }

            else
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    Game1.SpriteBatch.Draw(Game1.Tileset.Image, tiles[i].DrawRectangle, Game1.Tileset.SourceRectangles[tileIndices[i]], Color.White);
                }
            }

        }

        public virtual void PreviewDraw()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Game1.SpriteBatch.Draw(Game1.Tileset.Image, tiles[i].DrawRectangle, Game1.Tileset.SourceRectangles[tileIndices[i]], Color.White*0.5f);
            }
        }

    }
}
