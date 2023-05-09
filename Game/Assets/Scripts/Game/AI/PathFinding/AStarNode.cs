using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStarNode
    {
        private int gCost; // cost of the path from the startnode
        private int hCost; // estimatet cost to the targetnode

        public int fCost { get => gCost + hCost; }
        public Vector3 Position { get; private set; }
        public bool IsWalkable { get; private set; }
        public AStarNode LastNodeInPath { get; set; } = null;

        public AStarNode(bool isWalkable, Vector3 position) {
            this.IsWalkable = isWalkable;
            this.Position = position;
        }

        public void UpdateCost(int gCost, int hCost) {
            this.gCost = gCost;
            this.hCost = hCost;
        }

        public void Clear() {
            LastNodeInPath = null;
            gCost = 0;
            hCost = 0;
        }
    }
}