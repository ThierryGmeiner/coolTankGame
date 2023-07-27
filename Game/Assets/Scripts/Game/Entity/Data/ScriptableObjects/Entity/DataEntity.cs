using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntity), menuName = nameof(DataEntity))]
    public class DataEntity : ScriptableObject
    {
        public string Name = "DefaultTank";
    }
}