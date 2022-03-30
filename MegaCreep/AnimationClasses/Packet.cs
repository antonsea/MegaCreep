using MegaCreep.BuildingClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.AnimationClasses
{
    public class Packet : Animation
    {
        List<Building> path;
        int tileIndex = 13;
        //packet offset is to center the packet so it follows the connection line
        Vector2 packetOffet = new Vector2(8, 8);

        //speed is the amount of pixels the packet will move per update
        int speed = 3;
        //all positions here are in terms of pixels
        Vector2 position;
        Vector2 destination;
        Vector2 unitVector;
        //node is used to keep track of where in the path list we are
        int node;

        Rectangle drawRectangle;

        public Packet(List<Building> path)
        {
            
            this.path = new List<Building>(path);
            position = path[0].CenterPixel;

            node = 0;
            CalculateLeg();
        }

        private void CalculateLeg()
        {
            //If the packet reached its last node, then the animation is done
            if(node == path.Count - 1)
            {
                EndAnimation();
                return;
            }

            //Otherwise we need to figure out where we are going next
            node++;
            destination = path[node].CenterPixel;
            unitVector = Vector2.Subtract(destination, position);
            unitVector.Normalize();
            
        }

        public override void Update()
        {
            //If either building on the packet's current leg is destroyed, then the packet must be destroyed (since the connection is gone)
            //This means that destroying a key building in the packets path will destroy the packet, even if there is an alternative path for the packet to take
            //I might change it in the future that the packet will recalculate its path if needed.
            if(path[node - 1].Destroyed || path[node].Destroyed)
            {
                ClearOnAnimationEnd();
                EndAnimation();
                return;
            }

            Vector2 deltaVector = Vector2.Subtract(destination, position);
            position.X += Math.Min(Math.Abs(deltaVector.X), Math.Abs(unitVector.X * speed)) * Math.Sign(deltaVector.X);
            position.Y += Math.Min(Math.Abs(deltaVector.Y), Math.Abs(unitVector.Y * speed)) * Math.Sign(deltaVector.Y);

            if(position == destination)
            {
                CalculateLeg();
            }
        }

        public override void Draw()
        {
            drawRectangle = new Rectangle((int)position.X, (int)position.Y, Game1.TileSize, Game1.TileSize);
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, drawRectangle, Game1.Tileset.SourceRectangles[tileIndex], Color.White, 0, packetOffet, SpriteEffects.None, 1);

        }

    }
}
