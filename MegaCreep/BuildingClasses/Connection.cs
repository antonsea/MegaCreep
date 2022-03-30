using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.BuildingClasses
{
    public class Connection
    {
        Vector2 start;
        Vector2 end;
        //Length is in pixels
        float length;
        public float Length
        {
            get { return length; }
        }

        float rotation;
        Rectangle drawRectangle;
        //The image is 16x16 so we need the origin to be 8, 16 to rotate at the base of the connection line
        Vector2 drawOrigin = new Vector2(8, 16);
        int tileIndex = 20;

        public Connection(Building startBuilding, Building endBuilding)
        {
            start = startBuilding.CenterPixel;
            end = endBuilding.CenterPixel;
            length = Vector2.Distance(start, end);

            CalculateAngle();

            drawRectangle = new Rectangle((int)start.X, (int)start.Y, Game1.TileSize, (int)length);
        }

        private void CalculateAngle()
        {
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;

            double angle = Math.Atan(Math.Abs(dy / dx));

            if (dy < 0)
            {
                angle = MathHelper.PiOver2 - angle;
            }
            else
            {
                angle = MathHelper.PiOver2 + angle;
            }

            if (dx < 0)
            {
                angle = angle * -1;
            }

            rotation = (float)angle;
        }

        public void Draw()
        {
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[tileIndex], Color.Gray, rotation, drawOrigin, SpriteEffects.None, 1);
        }

        public void PreviewDraw()
        {
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[tileIndex], Color.Gray * 0.5f, rotation, drawOrigin, SpriteEffects.None, 1);
        }

    }
}
