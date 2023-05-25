using UnityEngine;
using System.Collections.Generic;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = "BulletStorage", menuName = "BulletStorage")]
    public class BulletStorage : ScriptableObject
    {
        [SerializeField] private GameObject[] bulletObjects;
        private List<Bullet> bulletSorts;
        private Bullet currentBullet;

        private void Awake() {
            foreach (GameObject bullet in bulletObjects) {
                bulletSorts.Add(bullet.GetComponent<Bullet>());
            }
            currentBullet = bulletSorts[0];
        }

        public Bullet CurrentBullet { get => currentBullet; }

        public void SetCurrentBullet(int index) => currentBullet = bulletSorts[index];
    }
}