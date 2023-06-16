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

        public static Path operator+ (Path a, Path b) {
            AStarNode[] nodes = new AStarNode[a.Nodes.Length + b.Nodes.Length];
            a.Nodes.CopyTo(nodes, 0);
            b.Nodes.CopyTo(nodes, a.Nodes.Length);
            return new Path(nodes, a.IsOptimized && b.IsOptimized);
        }
    }
}