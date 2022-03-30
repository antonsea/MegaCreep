using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep.AnimationClasses
{
    public class AnimationManager
    {
        private List<Animation> animations;
        
        public AnimationManager()
        {
            animations = new List<Animation>();
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        public void RemoveAnimation(Animation animation)
        {
            animations.Remove(animation);
        }

        public void Update()
        {
            //Because we can't remove a list as we iterate it, I need to make a place holder list
            List<Animation> currentAnimations = new List<Animation>(animations);

            foreach(Animation animation in currentAnimations)
            {
                animation.Update();
            }
        }

        public void Draw()
        {
            foreach(Animation animation in animations)
            {
                animation.Draw();
            }
        }
    }
}
