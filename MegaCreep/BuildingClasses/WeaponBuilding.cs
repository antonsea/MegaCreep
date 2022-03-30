using MegaCreep.AnimationClasses;
using MegaCreep.GameScreens;
using MegaCreep.TerrainClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.BuildingClasses
{
    public abstract class WeaponBuilding : Building
    {
        protected int currentAmmo;
        protected int maxAmmo;
        protected int ammoPerRequest;
        protected int ammoRequested;

        protected float damage;
        public WeaponBuilding(BuildingManager manager, Tile[] tiles, int[] tileIndices)
            :base(manager, tiles, tileIndices)
        {

        }

        public override void Update()
        {
            if(connectedHQ != null)
            {
                CheckAmmo();
            }

            if(currentAmmo > 0)
            {
                Fire();
            }

            base.Update();
        }

        public virtual void CheckAmmo()
        {
            int ammoDeficit = maxAmmo - currentAmmo - ammoRequested;
            if (ammoDeficit >= ammoPerRequest)
            {
                ammoRequested += ammoPerRequest;
                connectedHQ.AmmoRequested(this);
            }
        }

        public virtual void AmmoInbound()
        {
            Animation ammo = new Packet(pathToHQ);
            ammo.OnAnimationEnd += AmmoRecieved;
            ammo.Add();
        }

        protected virtual void AmmoRecieved(object sender, EventArgs e)
        {
            currentAmmo += ammoPerRequest;
            ammoRequested -= ammoPerRequest;
        }

        protected abstract void Fire();

    }
}
