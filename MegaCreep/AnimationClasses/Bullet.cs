using MegaCreep.BuildingClasses;
using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.AnimationClasses
{
    public class Bullet : Animation
    {
        int tileIndex = 21;
        //amount of pixels moved per tick (60 ticks per second)
        int speed = 15;
        //all positons here are in terms of pixels
        Vector2 position;
        Vector2 destination;
        Vector2 unitVector;

        Rectangle drawRectangle;
        Vector2 origin = new Vector2(8, 16);
        float bulletRotation;


        public Bullet(Turret source, Tile tileDestination)
        {

            destination = tileDestination.CenterPixel;
            position = source.CenterPixel;
            unitVector = Vector2.Subtract(destination, position);
            unitVector.Normalize();
            bulletRotation = source.BarrelRotation;
        }
        public override void Update()
        {
            Vector2 deltaVector = Vector2.Subtract(destination, position);
            position.X += Math.Min(Math.Abs(deltaVector.X), Math.Abs(unitVector.X * speed)) * Math.Sign(deltaVector.X);
            position.Y += Math.Min(Math.Abs(deltaVector.Y), Math.Abs(unitVector.Y * speed)) * Math.Sign(deltaVector.Y);

            if (position == destination)
            {
                EndAnimation();
            }
        }
        public override void Draw()
        {
            drawRectangle = new Rectangle((int)position.X, (int)position.Y, Game1.TileSize, Game1.TileSize);
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[tileIndex], Color.White, bulletRotation, origin, SpriteEffects.None, 1);
        }
    }
}
