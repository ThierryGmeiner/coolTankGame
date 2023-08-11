using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(Item), menuName = nameof(Item))]
    public class Item : ScriptableObject 
    {
        public Sprite Sprite;
        public Sprite SpriteWhenSelected;

        [Space]
        public Type type;

        public string typeID { get => System.Convert.ToString((byte)type >> 6, toBase: 2); }

        public enum Type {
            // shootingAttack (starting the 8-bit number with 00)
            defaultBullet = 0,
            multyShotBullet = 1,
            explodingBullet = 2,

            // mines (starting the 8-bit number with 01)
            defaultMine = 64,
            strongerMine = 65,

            // placables (starting the 8-bit number with 10)
            defaultDefenceBuilding = 128,
            strongDefenceBuilding = 129,
        }
    }
}