using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep
{
    public class Tileset
    {
        //At the moment I assume all tilesets will be squares. If this changes I will need to add tileWidth and tileHeight fields.
        int tileSize;

        public int TileSize
        {
            get { return tileSize; }
        }

        //The actual .PNG file that I use
        Texture2D image;
        public Texture2D Image
        {
            get { return image; }
        }

        //This class will cut up the image and store all the tiles (according to the set tile size) into an array
        Rectangle[] sourceRectangles;
        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        public Tileset(Texture2D image, int tileSize, int tilesWide, int tilesHigh)
        {
            this.tileSize = tileSize;
            this.image = image;
            sourceRectangles = new Rectangle[tilesWide * tilesHigh];

            int tile = 0;
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * tileSize,
                        y * tileSize,
                        tileSize,
                        tileSize);

                    tile++;
                }
            }

        }
    }
}
