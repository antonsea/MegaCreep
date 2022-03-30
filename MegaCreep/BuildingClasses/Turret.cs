using MegaCreep.AnimationClasses;
using MegaCreep.CreepClasses;
using MegaCreep.GameScreens;
using MegaCreep.GameScreens.GamePanels;
using MegaCreep.TerrainClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.BuildingClasses
{
    public class Turret : WeaponBuilding
    {
        //barrelOrigin is where we rotate the barel around - which should be the bottom center.
        //The image is 16x16 so we need the origin to be 8, 16
        Vector2 barrelOrigin = new Vector2(8, 16);
        Rectangle barrelDrawRectangle;
        float barrelRotation;
        public float BarrelRotation
        {
            get { return barrelRotation; }
        }

        Creep currentTarget;
        public Turret (BuildingManager manager, Tile[] destinations)
            :base(manager, destinations, BuildingSpecs.TurretIndices)
        {
            updateRate = 10;
            currentAmmo = 10;
            maxAmmo = 50;
            ammoPerRequest = 10;
            ammoRequested = 0;

            damage = 5;

            barrelDrawRectangle = new Rectangle((int)centerPixel.X, (int)centerPixel.Y, Game1.TileSize, Game1.TileSize * 3 / 2);
        }

        public override void Update()
        {
            if (ticksSinceUpdate >= updateRate)
            {
                ticksSinceUpdate = 0;
                base.Update();
            }
            else
                ticksSinceUpdate++;
        }

        protected override void Fire()
        {
            currentTarget = FindTarget();
            if(currentTarget != null)
            {
                RotateBarrel(currentTarget);
                Bullet bullet = new Bullet(this, currentTarget.Tile);
                bullet.OnAnimationEnd += TargetHit;
                bullet.Add();
            }

        }

        private Creep FindTarget()
        {
            //I think this current method is very inefficent and will need be to redone later
            List<Tile> currentRing = new List<Tile>();
            List<Tile> nextRing = new List<Tile>();
            List<Tile> checkedTiles = new List<Tile>();

            //maxsteps is number of "rings" the turret scans out
            int maxSteps = 5;
            int step = 0;

            foreach (Tile tile in tiles)
                currentRing.Add(tile);

            while(step < maxSteps)
            {
                foreach(Tile tile in currentRing)
                {
                    foreach(Tile neighbor in tile.Neighbors)
                    {
                        if (neighbor.Creep != null)
                            return neighbor.Creep;

                        else if(!checkedTiles.Contains(neighbor))
                        {
                            nextRing.Add(neighbor);
                        }
                    }
                }

                currentRing = nextRing;
                nextRing = new List<Tile>();
                step++;
            }

            return null;
        }

        private void RotateBarrel(Creep target)
        {
            double dx = target.Tile.CenterPixel.X - this.CenterPixel.X;
            double dy = target.Tile.CenterPixel.Y - this.CenterPixel.Y;

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
            barrelRotation = (float)angle;
        }

        private void TargetHit(object sender, EventArgs e)
        {
            World.CreepManager.RemoveCreep(currentTarget, damage);

            foreach(Tile neighbor in currentTarget.Tile.Neighbors)
            {
                World.CreepManager.RemoveCreep(neighbor, damage / 2);
            }
            currentAmmo--;
        }

        public override void Draw()
        {
            base.Draw();
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, barrelDrawRectangle, Game1.Tileset.SourceRectangles[BuildingSpecs.TurretBarrelIndex], Color.Black, barrelRotation, barrelOrigin, SpriteEffects.None, 1);

        }
        public override void PreviewDraw()
        {
            base.PreviewDraw();
            Game1.SpriteBatch.Draw(Game1.Tileset.Image, barrelDrawRectangle, Game1.Tileset.SourceRectangles[BuildingSpecs.TurretBarrelIndex], Color.Black*0.5f, barrelRotation, barrelOrigin, SpriteEffects.None, 1);

        }
    }
}
