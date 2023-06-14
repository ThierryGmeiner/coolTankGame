namespace Game.AI
{
    public class Path
    {
        public AStarNode[] Nodes { get; set; }
        public bool IsOptimized { get; set; }

        public AStarNode Start { get => Nodes[0]; }
        public AStarNode Target { get => Nodes[Nodes.Length - 1]; }

        public Path(AStarNode[] path, bool optimized) {
            Nodes = path;
            IsOptimized = optimized;
        }
    }
}