using MegaCreep.AnimationClasses;
using MegaCreep.GameScreens;
using MegaCreep.TerrainClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.BuildingClasses
{
    public class Headquarters : Building
    {
        Queue<WeaponBuilding> ammoQueue;
        public Headquarters(BuildingManager manager, Tile[] destinations)
            : base(manager, destinations, BuildingSpecs.HeadquartersIndices)
        {
            updateRate = 5;
            connectedHQ = this;
            pathToHQ.Add(this);
            distanceToHQ = 0;

            ammoQueue = new Queue<WeaponBuilding>();
        }

        public override void Update()
        {
            if(ticksSinceUpdate >= updateRate && ammoQueue.Count > 0)
            {
                ticksSinceUpdate = 0;
                SendAmmo();
                base.Update();
            }

            else
            {
                ticksSinceUpdate++;
            }

        }


        public void AmmoRequested(WeaponBuilding building)
        {
            //Debug.WriteLine("Ammo request received");
            ammoQueue.Enqueue(building);
        }

        private void SendAmmo()
        {
            WeaponBuilding destination = null;
            bool dequeuing = true;
            //We can't just dequeue from the ammoQueue list because theres a chance the building was removed and no longer needs ammo
            while(dequeuing)
            {
                if(ammoQueue.Count == 0)
                {
                    dequeuing = false;
                    destination = null;
                    break;
                }
                destination = ammoQueue.Dequeue();
                if(manager.Buildings.Contains(destination) && destination.DistanceToHQ > 0)
                {
                    dequeuing = false;
                }
            }

            if(destination != null)
            {
                destination.AmmoInbound();
            }
        }
    }
}
