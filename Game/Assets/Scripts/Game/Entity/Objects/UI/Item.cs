using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(Item), menuName = nameof(Item))]
    public class Item : ScriptableObject
    {
        public Sprite Sprite;
        public Type ItemType;

        public enum Type { Bullet = 0, Mine, Defense }
    }
}