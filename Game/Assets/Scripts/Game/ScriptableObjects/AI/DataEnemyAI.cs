using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEnemyAI), menuName = nameof(DataEnemyAI))]
    public class DataEnemyAI : ScriptableObject
    {
        [Header("View")]
        public float viewRadius = 6;
        public float viewRadiusExtended = 18;
        [Range(0, 360)] public float viewAngle = 90;

        [Header("Movement")]
        public Transform[] wayPoints = new Transform[0];
        public float preferTargetDistanceMin = 5;
        public float preferTargetDistanceMax = 14;

        [Header("Behavior")]
        [Range(1, 99)] public int aggressiveness = 80;
        [Range(1, 50)] public int changeToDefenseMode = 30;
    }
}