using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntityMovement), menuName = nameof(DataEntityMovement))]
    public class DataEntityMovement : ScriptableObject
    {
        [Header("Movement")]
        public float Speed = 6;
        [Range(1.1f, 5f)] public float SpritnMultiplyer = 1.5f;
        [Space]
        [Header("Rotation")]
        public float BodyRotationSpeed = 6;
        public float HeadRotationSpeed = 6;
    }
}