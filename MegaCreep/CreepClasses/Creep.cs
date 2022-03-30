using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.CreepClasses
{
    public class Creep
    {
        float minTransferAmount = 0.15f;

        CreepManager manager;
        Tile tile;
        public Tile Tile
        {
            get { return tile; }
        }

        //The total amount of creep in the tile
        public float Amount
        {
            get; set;
        }

        //The number of "levels" of creep - which is the amount rounded up 
        //So for every 1.0 amount of creep, it will draw an extra layer
        public double Levels
        {
            get { return Math.Ceiling(Amount); }
        }

        public Creep(CreepManager manager, Tile tile, float amount)
        {
            this.manager = manager;
            //Again, I'm not sure this is best practice, but I have each creep and its corresponding terrain tile refer to each other
            this.tile = tile;
            tile.Creep = this;
            this.Amount = amount;
        }

        public void Flow()
        {
            //Consider randomizing the order of its neighbors so its not iterated through the same way each time which will add a slight bias to direction of flow
            foreach (Tile neighbor in tile.Neighbors)
            {
                //If the creep doesn't have enough material to flow, it wont flow. Even though the next line.
                //Right now I don't let creep flow, leaving behind an empty space
                if (Amount <= minTransferAmount)
                    break;

                //Compare the heights of this creep with its neighbor
                //If its higher (i.e. neighbors have same elevation but less creep, or neighbors are at lower elevation) and has enough creep to flow, then flow!
                if (this.tile.TotalHeight - neighbor.TotalHeight > minTransferAmount)
                    manager.TransferCreep(this, neighbor, minTransferAmount);

                //Consider adding line to make tall stacks of creep flow quicker.
            }
        }



        public void Draw()
        {
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, tile.DrawRectangle, Game1.Tileset.SourceRectangles[12], Color.White * 0.3f * (float)Levels);

        }

    }
}
