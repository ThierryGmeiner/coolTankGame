using UnityEngine;
using Magic;

namespace Game.Entity.Particle
{
    [RequireComponent(typeof(Collider))]
    public class ParticleShowPath : MonoBehaviour, IPoolable
    {
        [SerializeField] private Collider colliedr;
        private IPoolable iPoolable;

        public GameObject GameObject { get => gameObject; }
        public IPoolable IPoolable { get => iPoolable; }
        public ObjectPooling Container { get; set; }

        private void Awake() {
            colliedr ??= GetComponent<Collider>();
            iPoolable = GetComponent<IPoolable>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == Tags.Player) {
                Debug.Log("tag");
            }
        }

        public void SetActive() {
            colliedr.enabled = true;
        }

        public void SetInactive() {
            transform.position = new Vector3(-200, -200, -200);
            colliedr.enabled = false;
        }
    }
}