// @author Code Monkey
// @source https://www.youtube.com/watch?v=alU04hvz6L4
//
// NOTE: Added minor improvements (Readability and Performance)

namespace LAIeRS.Pathfinding
{
    public class Node2D
    {
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public int X;
        public int Y;

        public bool IsWalkable;
        
        public Node2D Parent;
        
        public Node2D(int x, int y, bool isWalkable = true)
        {
            this.X = x;
            this.Y = y;
            IsWalkable = isWalkable;
        }
        
        public void Reset()
        {
            GCost = int.MaxValue;
            Parent = null;
        }
    }
}