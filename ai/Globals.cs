using System;
using System.Collections.Generic;
using System.Text;

namespace ai
{
    class Globals
    {

        public static List<(int x, int y)> hasResources = new List<(int x, int y)>();


        public static string North = "N";
        public static string South = "S";
        public static string East = "E";
        public static string West = "W";
      

        public static UnitManager um;

        public static string directionToAdjactentPoint((int x, int y)cur, (int x, int y) dest)
        {
            if(cur.x + 1 == dest.x)
            {
                return "E";
            }

            if (cur.x - 1 == dest.x)
            {
                return "W";
            }

            if (cur.y + 1 == dest.y)
            {
                return "S";
            }

            if (cur.y - 1 == dest.y)
            {
                return "N";
            }

            return "None";
        }
    }
}
