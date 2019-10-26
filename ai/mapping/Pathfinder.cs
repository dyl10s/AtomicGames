using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{

    public class PathFinder
    {
        private class Node
        {
            public int KnownCostToStart { get; set; }
            public int EstimatedCostToEnd { get; set; }
            public int EstimatedTotalPathCost { get { return KnownCostToStart + EstimatedCostToEnd; } }
            public (int X, int Y) Location { get; set; }
            public Node Parent { get; set; }

            public Node((int X, int Y) location)
            {
                Location = location;
            }

            public bool Equals(Node node)
            {
                return node != null && node.Location.Equals(Location);
            }

            public override bool Equals(Object obj)
            {
                if (obj == null) return false;
                Node nodeObj = obj as Node;
                if (nodeObj == null) return false;
                return Equals(nodeObj);
            }

            public override int GetHashCode()
            {
                return this.Location.GetHashCode();
            }

            public static bool operator ==(Node node1, Node node2)
            {
                if (((object)node1) == null || ((object)node2) == null)
                    return Object.Equals(node1, node2);

                return node1.Equals(node2);
            }

            public static bool operator !=(Node node1, Node node2)
            {
                if (((object)node1) == null || ((object)node2) == null)
                    return !Object.Equals(node1, node2);

                return !(node1.Equals(node2));
            }
        }

        private readonly IMap Map;
        private List<Node> OpenNodes;
        private List<Node> ClosedNodes;
        private List<(int X, int Y)> WalkableDirections = new List<(int X, int Y)> { (0, -1), (-1, 0), (1, 0), (0, 1) };

        public PathFinder(IMap map)
        {
            Map = map;
        }

        public List<(int X, int Y)> FindPathToAdjacentTile((int X, int Y) start, (int X, int Y) destination)
        {
            return FindPath(start, destination, 1);
        }

        public List<(int X, int Y)> FindPath((int X, int Y) start, (int X, int Y) destination, int closeEnough = 0)
        {
            InitializeNodeLists(start, destination);

            while (OpenNodes.Count > 0)
            {
                var node = DequeueMostPromisingOpenNode();

                if (ReachedDestination(node.Location, destination, closeEnough))
                {
                    /* We found a path - build a list of locations from the node tree. */
                    return BuildPathFromLocationToNode(start, node);
                }

                WalkableNeighbors(node).ForEach(neighbor => WalkNeighbor(node, neighbor, destination));
            }

            /* No path found. */
            return null;
        }

        private bool ReachedDestination((int, int) location, (int, int) destination, int closeEnough)
        {
            return Map.CalculateEstimatedDistance(location, destination) <= closeEnough;
        }

        private List<(int X, int Y)> BuildPathFromLocationToNode((int X, int Y) location, Node node)
        {
            var path = new List<(int, int)>();
            var current = node;
            while (current.Location != location)
            {
                path.Add(current.Location);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        private void WalkNeighbor(Node parent, Node neighbor, (int X, int Y) destination)
        {
            if (OpenNodes.Contains(neighbor))
            {
                UpdateOpenNode(parent, neighbor);
            }
            else
            {
                AddNewOpenNode(parent, neighbor, destination);
            }
        }

        private static void UpdateOpenNode(Node parent, Node neighbor)
        {
            int costToStart = parent.KnownCostToStart + 1;
            if (costToStart < neighbor.KnownCostToStart)
            {
                neighbor.KnownCostToStart = costToStart;
                neighbor.Parent = parent;
            }
        }

        private void AddNewOpenNode(Node parent, Node neighbor, (int X, int Y) destination)
        {
            neighbor.KnownCostToStart = parent.KnownCostToStart + 1;
            neighbor.EstimatedCostToEnd = Map.CalculateEstimatedDistance(neighbor.Location, destination);
            neighbor.Parent = parent;
            OpenNodes.Add(neighbor);
        }

        private void InitializeNodeLists((int X, int Y) origin, (int X, int Y) destination)
        {
            OpenNodes = new List<Node>();
            ClosedNodes = new List<Node>();

            var start = new Node(origin);
            start.KnownCostToStart = 0;
            start.EstimatedCostToEnd = Map.CalculateEstimatedDistance(origin, destination);
            OpenNodes.Add(start);
        }

        private Node DequeueMostPromisingOpenNode()
        {
            var node = OpenNodes.OrderBy(n => n.EstimatedTotalPathCost).First();
            OpenNodes.Remove(node);
            ClosedNodes.Add(node);
            return node;
        }

        private List<Node> WalkableNeighbors(Node node)
        {
            return WalkableDirections.Select(offset => WalkableNeighborAt((node.Location.X + offset.X, node.Location.Y + offset.Y)))
                                     .Where(neighbor => neighbor != null)
                                     .Where(neighbor => !ClosedNodes.Contains(neighbor))
                                     .ToList();
        }

        private Node WalkableNeighborAt((int X, int Y) location)
        {
            return Map[location].Walkable ? new Node(location) : null;
        }
    }
}