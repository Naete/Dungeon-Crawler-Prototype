// @author Code Monkey
// @source https://www.youtube.com/watch?v=alU04hvz6L4
//
// NOTE: Added improvements (Readability and Performance)

using System;
using System.Collections.Generic;
using System.Linq;

using LAIeRS.ExtensiveMethods;
using LAIeRS.Miscellanious;

namespace LAIeRS.Pathfinding
{
    public static class AStarPathfinder2D
    {
        private const int STRAIGHT_MOVE_COST = 10;
        private const int DIAGONAL_MOVE_COST = 14;
        
        private static List<Node2D> _openNodes;
        private static List<Node2D> _closedNodes;
        
        private static Grid2D<Node2D> _grid2D;
        
        private static readonly (int x, int y)[] _directions = {
            (0, -1), (-1, 0), (1, 0), (0, 1), // Non-Diagonal
            //(-1, -1), (1, -1), (-1, 1), (1, 1), // Diagonal
        };

        public static List<Node2D> FindPath(
            float startPositionX, float startPositionY, 
            float endPositionX, float endPositionY, 
            Grid2D<Node2D> grid2D)
        {
            _grid2D = grid2D;
            
            Node2D startNode = _grid2D.GetItemAtPosition(startPositionX, startPositionY);
            Node2D endNode = _grid2D.GetItemAtPosition(endPositionX, endPositionY);
            
            if (startNode != null && endNode != null)
            {
                _openNodes = new List<Node2D> { startNode };
                _closedNodes = new List<Node2D>();
                
                InitializeNodesIn(grid2D);
                
                startNode.GCost = 0;
                startNode.HCost = CalculateDistanceCostBetween(startNode, endNode);
                
                while (_openNodes.Any())
                {
                    Node2D currentNode = GetLowestFCostNodeFrom(_openNodes);
                    
                    if (currentNode == endNode) 
                        return CalculatePathFrom(currentNode);
                    
                    _openNodes.Remove(currentNode);
                    _closedNodes.Add(currentNode);
                    
                    foreach (Node2D neighbourNode in GetNeighbourNodesOf(currentNode))
                    {
                        if (_closedNodes.Contains(neighbourNode)) 
                            continue;
                        
                        if (!neighbourNode.IsWalkable) 
                        {
                            _closedNodes.Add(neighbourNode);
                            continue;
                        }
                        
                        int currentGCost = currentNode.GCost + CalculateDistanceCostBetween(currentNode, neighbourNode);
                        
                        if (currentGCost.IsLessThan(neighbourNode.GCost))
                        {
                            neighbourNode.GCost = currentGCost;
                            neighbourNode.HCost = CalculateDistanceCostBetween(neighbourNode, endNode);
                            neighbourNode.Parent = currentNode;
                            
                            if (_openNodes.NotContains(neighbourNode)) 
                                _openNodes.Add(neighbourNode);
                        }
                    }
                }
            }
            
            return null;
        }

        private static int CalculateDistanceCostBetween(Node2D nodeA, Node2D nodeB)
        {
            int xDistance = Math.Abs(nodeA.X - nodeB.X);
            int yDistance = Math.Abs(nodeA.Y - nodeB.Y);
            int remaining = Math.Abs(xDistance - yDistance);
            return DIAGONAL_MOVE_COST * Math.Min(xDistance, yDistance) + 
                   STRAIGHT_MOVE_COST * remaining;
        }
        
        private static List<Node2D> CalculatePathFrom(Node2D currentNode)
        {
            var path = new List<Node2D> { currentNode };
            
            while (currentNode.Parent != null)
                path.Add(currentNode = currentNode.Parent);
            
            path.Reverse();
            return path;
        }
        
        private static Node2D GetLowestFCostNodeFrom(List<Node2D> nodes)
        {
            Node2D lowestFCostNode = nodes[0];

            for (int index = 1; index.IsLessThan(nodes.Count); index++)
            {
                Node2D currentNode = nodes[index];
                
                if (lowestFCostNode.FCost.IsGreaterThan(currentNode.FCost))
                    lowestFCostNode = currentNode;
            }

            return lowestFCostNode;
        }
        
        private static List<Node2D> GetNeighbourNodesOf(Node2D currentNode)
        {
            var neighbourNodeList = new List<Node2D>();
            
            foreach ((int x, int y) direction in _directions)
            {
                (int x, int y) index = (currentNode.X + direction.x, currentNode.Y + direction.y);
                
                Node2D neighbourNode = _grid2D.GetItemAtIndex(index.x, index.y);
                
                if (neighbourNode != null) 
                    neighbourNodeList.Add(neighbourNode);
            }
            
            return neighbourNodeList;
        }
        
        private static void InitializeNodesIn(Grid2D<Node2D> grid2D)
        {
            for (int y = 0; y < grid2D.Height; y++)
                for (int x = 0; x < grid2D.Width; x++)
                    grid2D.GetItemAtIndex(x, y).Reset();
        }
    }
}