using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{
    public class Tile
    {
        public bool Unknown { get; set; } = true;
        public int X { get; set; }
        public int Y { get; set; }

        private TileUpdate tileUpdate = new TileUpdate { Visible = false, Blocked = true };
        public TileUpdate TileUpdate
        {
            get { return tileUpdate; }
            set
            {
                /* If the tile is visible, we get good data about what's on it.
                   If it's not visible, we don't get any data at all.
                   We don't want to lose the 'last known' state of the tile so
                   we'll only update the visibility state if the tile is not visible. */
                if (value.Visible)
                {
                    Unknown = false;
                }

                if (tileUpdate == null || value.Visible)
                {
                    tileUpdate = value;
                }
                else
                {
                    tileUpdate.Visible = false;
                }
            }
        }
        public (int X, int Y) Location
        {
            get => (X, Y);
        }

        public bool Visible
        {
            get => TileUpdate.Visible;
        }

        public bool Walkable
        {
            get => !Unknown && !TileUpdate.Blocked;
        }

        public bool HasResource
        {
            get => TileUpdate.Resource != null;
        }

        public bool HasEnemies
        {
            get => TileUpdate.Units != null && TileUpdate.Units.Any(u => u.IsAlive);
        }

        public bool HasEnemyBase
        {
            get => (TileUpdate.Units != null) && TileUpdate.Units.Any(u => u.IsBase);
        }
    }
}