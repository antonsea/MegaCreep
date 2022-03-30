using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.CreepClasses
{
    public class CreepGenerator
    {
        private int tileIndex = 13;
        private CreepManager manager;
        private Tile tile;

        public CreepGenerator(CreepManager manager, Tile tile)
        {
            this.manager = manager;
            this.tile = tile;
        }

        public void Update()
        {
            manager.AddCreep(tile, 5);
            foreach(Tile neighbor in tile.Neighbors)
            {
                manager.AddCreep(neighbor, 5);
            }
        }

        public void Draw()
        {
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, tile.DrawRectangle, Game1.Tileset.SourceRectangles[13], Color.White);

        }
    }
}
