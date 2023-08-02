using UnityEngine;
using Magic;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(Item), menuName = nameof(Item))]
    public class Item : ScriptableObject 
    {
        public Sprite Sprite;
        public Sprite SpriteWhenSelected;
        [Space]
        public Type ItemType;

        public ObjectPooling objectPool;

        public enum Type { Bullet = 0, Mine = 1, Defense = 2}
    }
}