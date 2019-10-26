using System.Linq;

namespace ai.test
{

    class MapUtilities
    {
        private static (int X, int Y) FindHomeBase(char[,] data)
        {
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[x, y] == 'B')
                    {
                        return (x, y);
                    }

                }
            }
            return (0, 0);
        }

        private static bool TileIsBlocked(char tile)
        {
            return new char[] { 'B', 'X', 'R', 'H' }.Contains(tile);
        }

        private static ResourceUpdate TileResource(char tile)
        {
            return tile == 'R' ? new ResourceUpdate { Value = 10, Total = 10, Type = "small" } : null;

        }

        private static bool TileIsVisible(char tile)
        {
            return tile != 'H';
        }
        public static Map CreateMap(char[,] data)
        {
            var map = new Map();
            var homeBase = FindHomeBase(data);
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    var tile = data[y, x]; // Coordinates are reversed on purpose here
                    map[(x - homeBase.X, y - homeBase.Y)].TileUpdate = new TileUpdate
                    {
                        Visible = TileIsVisible(tile),
                        Blocked = TileIsBlocked(tile),
                        Resources = TileResource(tile)
                    };
                }
            }

            return map;
        }
    }
}