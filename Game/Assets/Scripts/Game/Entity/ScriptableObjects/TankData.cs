using UnityEngine;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = "ScriptableObject Tank", menuName = "ScriptableObject Tank")]
    public class TankData : ScriptableObject
    {
        [Header("Tank")]
        public string Name = "DefaultTank";

        [Space]
        [Header("Movement")]
        public float Speed = 10;
        public float TurboMultiplier = 1.6f;
        public float JumpForce = 10;

        [Space]
        [Header("Health")]
        public int Health = 100;

        [Space]
        [Header("Armor")]
        public int ArmorProcent = 5;

        [Space]
        [Header("Attack")]
        public BulletStorage BulletStorage;
    }
}