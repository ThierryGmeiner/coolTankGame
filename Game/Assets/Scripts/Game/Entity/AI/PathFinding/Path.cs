namespace Game.AI
{
    public class Path
    {
        public AStarNode[] Nodes { get; set; }
        public bool IsOptimized { get; set; }

        public Path(AStarNode[] path, Optimized optimization) {
            Nodes = path;
            IsOptimized = optimization == Optimized.True;
        }

        public enum Optimized { True, False }
    }
}