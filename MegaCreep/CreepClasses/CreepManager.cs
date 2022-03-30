using MegaCreep.TerrainClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.CreepClasses
{
    public class CreepManager
    {
        private List<Creep> currentCreep;
        private List<CreepGenerator> creepGenerators;

        //The game sends 60 update ticks per second
        int creepUpdateRate = 5;
        int ticksSinceCreepUpdate = 0;
        int generatorUpdateRate = 60;
        int ticksSinceGeneratorUpdate = 0;

        public CreepManager()
        {
            currentCreep = new List<Creep>();
            creepGenerators = new List<CreepGenerator>();
        }

        public void Update()
        {
            ticksSinceCreepUpdate++;
            ticksSinceGeneratorUpdate++;

            if(ticksSinceGeneratorUpdate >= generatorUpdateRate)
            {
                UpdateGenerators();
                ticksSinceGeneratorUpdate = 0;
            }

            if(ticksSinceCreepUpdate >= creepUpdateRate)
            {
                UpdateCreep();
                ticksSinceCreepUpdate = 0;
            }
        }

        public void AddGenerator(Tile destination)
        { 
            if(!destination.IsOccupied())
            {
                creepGenerators.Add(new CreepGenerator(this, destination));
            }

        }

        private void UpdateGenerators()
        {
            foreach(CreepGenerator generator in creepGenerators)
            {
                generator.Update();
            }
        }

        private void UpdateCreep()
        {
            //Because I cannot iterate through a list and modify it at the same time, I need to make a placeholder list
            List<Creep> creepToFlow = new List<Creep>();

            foreach(Creep creep in currentCreep)
            {
                creepToFlow.Add(creep);
            }

            foreach(Creep creep in creepToFlow)
            {
                creep.Flow();
            }
        }

        public void TransferCreep(Creep source, Tile destination, float amount)
        {
            RemoveCreep(source, amount);
            AddCreep(destination, amount);
        }
        public void AddCreep(Tile destination, float amount)
        {
            //If the destination already has creep add to it. Otherwise we will create new creep
            if(destination.Creep != null)
            {
                destination.Creep.Amount += amount;
            }
            else
            {
                currentCreep.Add(new Creep(this, destination, amount));
            }
        }

        public void RemoveCreep(Tile destination, float amount)
        {
            if(destination.Creep != null)
            {
                destination.Creep.Amount -= amount;

                if(destination.Creep.Amount <= 0.0f)
                {
                    currentCreep.Remove(destination.Creep);
                    destination.Creep = null;
                }
            }
        }

        public void RemoveCreep(Creep creep, float amount)
        {
            //I have two methods to remove creep. This one just simplifies things if I know for sure that there is creep at the tile I am removing (i.e. in the case of creep flowing)
            creep.Amount -= amount;
            if(creep.Amount <= 0.0f)
            {
                currentCreep.Remove(creep);
                creep.Tile.Creep = null;
            }
        }

        public void Draw()
        {
            foreach(Creep creep in currentCreep)
            {
                creep.Draw();
            }
            foreach(CreepGenerator generator in creepGenerators)
            {
                generator.Draw();
            }
        }
    }
}
