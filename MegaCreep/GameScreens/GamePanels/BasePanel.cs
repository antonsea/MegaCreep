using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.GameScreens.GamePanels
{
    public abstract class BasePanel : DrawableGameComponent
    {
        //This rectangle gives us location and size information
        protected Rectangle PanelRectangle;

        public BasePanel(Game game, Rectangle rectangle)
            : base(game)
        {
            PanelRectangle = rectangle;
        }
    }
}
