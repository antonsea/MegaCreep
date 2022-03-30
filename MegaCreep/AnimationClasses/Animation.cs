using MegaCreep.GameScreens;
using MegaCreep.GameScreens.GamePanels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.AnimationClasses
{

    public abstract class Animation
    {
        protected AnimationManager manager;
        public event EventHandler OnAnimationEnd;
        public Animation()
        {
            this.manager = World.AnimationManager;
        }

        protected void EndAnimation()
        {
            manager.RemoveAnimation(this);
            OnAnimationEnd?.Invoke(this, null);
        }

        protected void ClearOnAnimationEnd()
        {
            OnAnimationEnd = null;
        }
       
        public void Add()
        {
            manager.AddAnimation(this);
        }
        public abstract void Update();

        public abstract void Draw();

    }
}
