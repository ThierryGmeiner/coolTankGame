using UnityEngine;
using Magic;

namespace Game.Entity.Particle
{
    [RequireComponent(typeof(Collider))]
    public class ShowPathParticle : MonoBehaviour, IPoolable
    {
        [SerializeField] private Collider colliedr;
        [SerializeField] public ParticleSystem stayingParticle;
        [SerializeField] public ParticleSystem flyingParticle;
        private IPoolable iPoolable;

        public GameObject GameObject { get => gameObject; }
        public IPoolable IPoolable { get => iPoolable; }
        public ObjectPooling ObjectPooler { get; set; }

        private void Awake() {
            colliedr ??= GetComponent<Collider>();
            iPoolable = GetComponent<IPoolable>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == Tags.Player) {
                SetInactive();
            }
        }

        public void SetActive() {
            flyingParticle.Emit(1);
            stayingParticle.Emit(1);
            flyingParticle.Play();
            stayingParticle.Play();
            colliedr.enabled = true;
        }

        public void SetInactive() {
            flyingParticle.Stop();
            stayingParticle.Stop();
            transform.position = new Vector3(-200, -200, -200);
            colliedr.enabled = false;
        }
    }
}