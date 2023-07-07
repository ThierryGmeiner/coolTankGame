using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataInteractable), menuName = nameof(DataInteractable))]
    public class DataInteractable : ScriptableObject
    {
        [SerializeField] public DataEntity Entity;
        [SerializeField] public int healing = 20;
        [SerializeField] public int hitPoints = 3;
    }
}