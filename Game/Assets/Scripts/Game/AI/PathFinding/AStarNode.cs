using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStarNode
    {
        public Vector3 Position { get; private set; }
        public bool IsWalkable { get; private set; }

        public AStarNode(bool isWalkable, Vector3 position) {
            this.IsWalkable = isWalkable;
            this.Position = position;
        }

    }
}