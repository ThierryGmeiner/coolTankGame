using UnityEngine;

namespace Game.Entity.Particle
{
    [RequireComponent(typeof(Collider))]
    public class ParticleSelfDestruction : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] Collider colliedr;

        private const string NAME_PARTICLE = "particle";

        private void Awake() {
            colliedr ??= GetComponent<Collider>();
            transform.parent = container.transform;
            name = NAME_PARTICLE;
        }


    }
}