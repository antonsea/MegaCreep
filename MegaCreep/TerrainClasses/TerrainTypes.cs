using System;
using System.Collections.Generic;
using System.Text;

namespace MegaCreep.TerrainClasses
{
    public class Ground1 : Tile
    {
        public Ground1(int x, int y)
            : base(0, 0, x, y)
        {
        }
    }
    public class Ground2 : Tile
    {
        public Ground2(int x, int y)
            : base(1, 10, x, y)
        {
        }
    }
    public class Ground3 : Tile
    {
        public Ground3(int x, int y)
            : base(2, 20, x, y)
        {
        }
    }
    public class Ground4 : Tile
    {
        public Ground4(int x, int y)
            : base(3, 30, x, y)
        {
        }
    }
    public class Ground5 : Tile
    {
        public Ground5(int x, int y)
            : base(4, 40, x, y)
        {
        }
    }
    public class Ground6 : Tile
    {
        public Ground6(int x, int y)
            : base(5, 50, x, y)
        {
        }
    }

}
