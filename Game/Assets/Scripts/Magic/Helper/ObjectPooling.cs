using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField] private string objectName;
        [SerializeField] private int spawnAmount;
        [SerializeField] private GameObject pooledObject;

        public LinkedList<IPoolable> InactiveObjects { get; set; } = new LinkedList<IPoolable>();
        public LinkedList<IPoolable> ActiveObjects { get; set; } = new LinkedList<IPoolable>();

        private void Awake() {
            if (pooledObject == null) {
                Debug.LogError($"{nameof(gameObject.name)}.{nameof(ObjectPooling)}.{nameof(pooledObject)} is null");
                return;
            }
            IPoolable iPoolable = pooledObject.GetComponent<IPoolable>();
            if (iPoolable == null) {
                Debug.LogError($"{nameof(gameObject.name)}.{nameof(ObjectPooling)}.{nameof(pooledObject)} has no {nameof(IPoolable)} atatched");
                return;
            }

            for (int i = 0; i < spawnAmount; i++) {
                InstantiatePooledObject();
            }
        }

        private void InstantiatePooledObject() {
            GameObject obj = Instantiate(pooledObject);
            IPoolable iPoolable = obj.GetComponent<IPoolable>();

            iPoolable.Container = this;
            obj.transform.parent = transform;
            obj.name = objectName;
            InactiveObjects.AddFirst(iPoolable);
            iPoolable.SetInactive();
        }

        public GameObject RequestActivatedObject() {
            if (InactiveObjects.Count == 0) {
                InstantiatePooledObject();
            }

            IPoolable obj = InactiveObjects.First.Value;
            InactiveObjects.RemoveFirst();
            ActiveObjects.AddFirst(obj);
            obj.SetActive();
            return obj.GameObject;
        }

        public void DeaktivateObject(IPoolable iPoolable) {
            ActiveObjects.Remove(iPoolable);
            InactiveObjects.AddLast(iPoolable);
            iPoolable.SetInactive();
        }
    }

    public interface IPoolable
    {
        public GameObject GameObject { get; }
        public ObjectPooling Container { get; set; }
        public void SetActive();
        public void SetInactive();
    }
}