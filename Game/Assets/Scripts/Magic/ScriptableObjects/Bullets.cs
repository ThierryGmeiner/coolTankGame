using UnityEngine;

namespace Magic.Data
{
    [CreateAssetMenu(fileName = "BulletStorage", menuName = "BulletStorage")]
    public class Bullets : ScriptableObject
    {
        [SerializeField] private GameObject[] bullet;

        public GameObject[] Bullet { get => bullet; }

    }
}