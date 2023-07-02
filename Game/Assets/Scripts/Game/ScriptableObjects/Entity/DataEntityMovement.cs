using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntityMovement), menuName = nameof(DataEntityMovement))]
    public class DataEntityMovement : ScriptableObject
    {
        public float Speed = 10;
        [Range(1.1f, 5f)] 
        public float SpritnMultiplyer = 1.5f;
        public float JumpForce = 10;
    }
}