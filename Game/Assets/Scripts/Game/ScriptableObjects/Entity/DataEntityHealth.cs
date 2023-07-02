using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntityHealth), menuName = nameof(DataEntityHealth))]
    public class DataEntityHealth : ScriptableObject
    {
        [Range(1, 1000)]
        public int HitPoints = 100;
    }
}