using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStarNode
    {
        public int gCost { get; set; } = 0; // cost of the path from the startnode
        public int hCost { get; set; } = 0; // estimatet cost to the targetnode
        public int fCost { get => gCost + hCost; }

        public Vector2Int ArrayIndex { get; private set; }
        public Vector2 Position { get; private set; }
        public AStarNode LastNodeInPath { get; set; } = null;
        public bool IsWalkable { get; set; }
        public bool AllNeighborsAreDiscovered { get; set; } = false;

        public AStarNode(bool isWalkable, Vector2 position) : this(isWalkable, position, new Vector2Int(-1, -1)) { }
        public AStarNode(bool isWalkable, Vector2 position, Vector2Int arrayIndex) {
            this.IsWalkable = isWalkable;
            this.Position = position;
            this.ArrayIndex = arrayIndex;
        }

        public void Clear() {
            LastNodeInPath = null;
            AllNeighborsAreDiscovered = false;
            gCost = 0;
            hCost = 0;
        }

        public bool Equals(AStarNode node) {
            if (ArrayIndex != node.ArrayIndex) return false;
            if (this.IsWalkable != node.IsWalkable) return false;
            if (this.Position != node.Position) return false;
            if (gCost != node.gCost) return false;
            if (hCost != node.hCost) return false;
            return true;
        }
    }
}