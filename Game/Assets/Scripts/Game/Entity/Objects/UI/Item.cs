using UnityEngine;
using UnityEngine.UI;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(Item), menuName = nameof(Item))]
    public class Item : ScriptableObject
    {
        public Sprite Sprite;
        public Sprite SpriteWhenSelected;
        [Space]
        public Type ItemType;

        public enum Type { Bullet = 0, Mine, Defense }
    }
}