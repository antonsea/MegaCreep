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
    public enum BuildingType
    {
        None,
        Headquarters,
        Turret
    }
    
    public class BuildingManager
    {
        List<Building> buildings;
        public List<Building> Buildings
        {
            get { return buildings; }
        }

        //This connections list is really only useful for drawing the connections between buildings
        //I can handle drawing the connections in the builing class, but because two buildings have the same "Connection" I would need a way to make sure that I don't draw the connection twice (for optimization purposes)
        //I might make this change later
        List<Connection> connections;
        public List<Connection> Connections
        {
            get { return connections; }
        }


        #region Building Preview Region
        BuildingType previewBuildingType;
        public BuildingType PreviewBuildingType
        {
            get { return previewBuildingType; }
        }
        Building buildingPreview;
        #endregion

        public BuildingManager()
        {
            buildings = new List<Building>();
            connections = new List<Connection>();

            //These are place holders for any building we are previewing when we are trying to place a new one
            previewBuildingType = BuildingType.None;
        }

        public void SetPreview(BuildingType buildingType)
        {
            previewBuildingType = buildingType;
            if (buildingType == BuildingType.None)
                buildingPreview = null;
        }

        public void AddBuilding()
        {
            if(buildingPreview != null)
            {
                buildingPreview.Build();
                buildings.Add(buildingPreview);
                buildingPreview = null;
            }

        }
        
        public void RemoveBuilding(Building buildingToRemove)
        {
            buildingToRemove.Destroy();

            //When we destroy a building that is connected to HQ, we have to check all buildings that are farther away to make sure their connection to HQ was not severed.
            //I do this by clearing all of their paths to HQ and making them recheck their path to HQ.
            if(buildingToRemove.ConnectedHQ != null)
            {
                float distanceToHQ = buildingToRemove.DistanceToHQ;
                List<Building> buildingsToCheck = new List<Building>(buildings.Where(b => b.DistanceToHQ > distanceToHQ));
                foreach(Building building in buildingsToCheck)
                {
                    building.ClearPathToHQ();
                }

                foreach (Building building in buildingsToCheck)
                {
                    building.CheckPathToHQ();
                }

            }
            
        }

        private bool IsSpotLegal(Tile startTile, int width, int height, out Tile[] allTiles)
        {
            //We need to go through all the tiles that this building would take up and make sure they are all on the same elevation and not already occupied
            //As soon as we find that is not the case, we return false
            allTiles = new Tile[width * height];

            //If the spot we are looking at is at the edge of the map meaning that we won't be able to check all the neighbor tiles (since they dont exist) we have to return false
            if (startTile.X + width > GameScreen.World.TilesWide || startTile.Y + height > GameScreen.World.TilesHigh)
                return false;

            int i = 0;
            for (int y = startTile.Y; y < startTile.Y + height; y++)
            {
                for (int x = startTile.X; x < startTile.X + width; x++)
                {
                    Tile tile = GameScreen.World.Map[x, y];
                    if (tile.Elevation != startTile.Elevation || tile.IsOccupied())
                    {
                        return false;
                    }
                    allTiles[i] = tile;
                    i++;
                }
            }

            //If we get to this point, then we have gathered up all the tiles that will be used and the spot appears to be legal
            //We return true (and have made an array of the relevant tiles at the same time)
            return true;
        }
        private void CreatePreview()
        {
            if (InputHandler.MouseInWorldPanel(out Tile dest))
            {
                Tile[] allTiles;
                if(previewBuildingType == BuildingType.Turret && IsSpotLegal(dest, BuildingSpecs.TurretWidth, BuildingSpecs.TurretHeight, out allTiles))
                {
                    buildingPreview = new Turret(this, allTiles);
                }

                else if(previewBuildingType == BuildingType.Headquarters && IsSpotLegal(dest, BuildingSpecs.HeadquartersWidth, BuildingSpecs.HeadquartersHeight, out allTiles))
                {
                    buildingPreview = new Headquarters(this, allTiles);
                }
                else
                {
                    buildingPreview = null;
                }
            }
            else
                buildingPreview = null;
        }
        public void Update()
        {
            if(previewBuildingType != BuildingType.None)
            {
                CreatePreview();
            }
            foreach(Building building in buildings)
            {
                building.Update();
            }
        }

        public void Draw()
        {
            //I always draw the connections first, so the buildings can be drawn over them
            foreach (Connection connection in connections)
                connection.Draw();

            if (buildingPreview != null)
            {
                foreach (Connection connection in buildingPreview.Connections.Values)
                    connection.PreviewDraw();
                buildingPreview.PreviewDraw();
            }

            foreach (Building building in buildings)
                building.Draw();
        }
    }
}
