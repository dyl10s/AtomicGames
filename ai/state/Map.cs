using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{
    public class Map : IMap
    {

        private Dictionary<(int, int), Tile> Tiles = new Dictionary<(int, int), Tile>();
        public (int Width, int Height) Size { get; set; }

        public Tile this[(int X, int Y) location]
        {
            get
            {
                if (!Tiles.ContainsKey(location))
                {
                    Tiles[location] = new Tile { X = location.X, Y = location.Y };
                }
                return Tiles[location];
            }

            set
            {
                value.X = location.X;
                value.Y = location.Y;
                Tiles[location] = value;
            }
        }

        public void UpdateTiles(IEnumerable<TileUpdate> tileUpdates)
        {
            foreach (TileUpdate t in tileUpdates)
            {
                this[(t.X, t.Y)].TileUpdate = t;
            }
        }

        public List<(int X, int Y)> BuildNeighborLocationList(int range = 1)
        {
            int valueCount = range * 2 + 1;
            var xLocations = Enumerable.Range(-range, valueCount);
            var yLocations = Enumerable.Range(-range, valueCount);
            return xLocations.SelectMany(x =>
                              {
                                  return yLocations.Select(y => (x, y));
                              })
                              .Where(l => l != (0, 0))
                              .ToList();
        }

        public List<Tile> Neighbors((int X, int Y) target, int range = 1)
        {
            return BuildNeighborLocationList(range)
                .Select(location => this[(location.X + target.X, location.Y + target.Y)])
                .ToList();

        }

        public int CalculateEstimatedDistance((int X, int Y) from, (int X, int Y) to)
        {
            return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
        }

        public bool HasUnknownNeighbors((int X, int Y) location, int range = 1)
        {
            return Neighbors(location, range).Any(tile => tile.Unknown);
        }

        public bool CanMove((int X, int Y) location, MapDirections.Direction direction)
        {
            var offset = MapDirections.OffsetForDirection(direction);
            return this[(location.X + offset.X, location.Y + offset.Y)].Walkable;
        }

        public MapDirections.Direction DirectionToAdjacentResource((int X, int Y) location)
        {
            foreach (var direction in MapDirections.Directions)
            {
                if (NeighborInDirection(location, direction).HasResource)
                {
                    return direction;
                }
            }
            return MapDirections.Direction.None;
        }

        private Tile NeighborInDirection((int X, int Y) location, MapDirections.Direction direction)
        {
            var offset = MapDirections.OffsetForDirection(direction);
            return this[(location.X + offset.X, location.Y + offset.Y)];
        }

        public bool IsResourceAdjacentTo((int X, int Y) location)
        {
            return DirectionToAdjacentResource(location) != MapDirections.Direction.None;
        }

        public List<(int X, int Y)> ResourceLocationsNearest((int X, int Y) location)
        {
            /* This will throw if there are no resources on the map. */
            return AllResourceTiles()
                .OrderBy(t => CalculateEstimatedDistance(location, t.Location))
                .Select(t => t.Location)
                .ToList();
        }

        public bool HasResources
        {
            get { return Tiles.Values.Any(t => t.HasResource); }
        }

        private List<Tile> AllResourceTiles()
        {
            return Tiles.Values.Where(t => t.HasResource).ToList();
        }

        public (int X, int Y) HomeBaseLocation
        {
            get { return (0, 0); }
        }

        public bool HasEnemies
        {
            get { return Tiles.Values.Any(t => t.HasEnemies); }
        }

        public bool EnemyBaseFound
        {
            get { return Tiles.Values.Any(t => t.HasEnemyBase); }
        }

        public (int X, int Y) EnemyBaseLocation
        {
            /* This will throw if called before the enemy base is found. */
            get { return Tiles.Values.First(t => t.HasEnemyBase).Location; }
        }

        public bool LocationWithinRange((int X, int Y) from, (int X, int Y) to, int range)
        {
            return Math.Abs(to.X - from.X) <= range
                   && Math.Abs(to.Y - from.Y) <= range;
        }

        public List<(int X, int Y)> EnemyLocationsInRange((int X, int Y) start, int range = 1)
        {
            return Neighbors(start, range)
                .Where(t => t.Visible)
                .Where(t => t.HasEnemies)
                .Select(t => t.Location)
                .ToList();
        }
    }
}