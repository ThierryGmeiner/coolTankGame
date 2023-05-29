using UnityEngine;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = "BulletStorage", menuName = "BulletStorage")]
    public class BulletStorage : ScriptableObject
    {
        [SerializeField] 
        private GameObject[] bullets;
        private GameObject current;

        public void ManualAwake() {
            current = bullets[0];
        }
        
        public GameObject Current { get => current; set => current = value; }
        public GameObject[] Bullets { get => bullets; }
    }
}