using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class ObjectPooling : MonoBehaviour
    {        
        [SerializeField] private string objectName = " ";
        [SerializeField] private int spawnAmount = 10;
        [SerializeField] private GameObject pooledObject;

        public LinkedList<IPoolable> InactiveObjects { get; private set; } = new LinkedList<IPoolable>();
        public LinkedList<IPoolable> ActiveObjects { get; private set; } = new LinkedList<IPoolable>();

        private void Awake() {
            IPoolable iPoolable = pooledObject?.GetComponent<IPoolable>();
            if (iPoolable == null) { return; }

            for (int i = 0; i < spawnAmount; i++) {
                InstantiatePooledObject(i);
            }
        }

        private void InstantiatePooledObject(int num) {
            if (pooledObject == null) { return; }
            GameObject obj = Instantiate(pooledObject);
            IPoolable iPoolable = obj.GetComponent<IPoolable>();

            iPoolable.SetInactive();
            iPoolable.ObjectPooler = this;
            obj.transform.parent = transform;
            obj.name = objectName + " " + System.Convert.ToString(num);
            InactiveObjects.AddFirst(iPoolable);
        }

        public GameObject RequestObject() {
            if (InactiveObjects.Count == 0) {
                InstantiatePooledObject(InactiveObjects.Count + ActiveObjects.Count);
            }
            IPoolable obj = InactiveObjects.First.Value;
            InactiveObjects.RemoveFirst();
            ActiveObjects.AddFirst(obj);
            obj.SetActive();
            return obj.GameObject;
        }

        public void DeaktivateObject(IPoolable p) {
            ActiveObjects.Remove(p);
            InactiveObjects.AddLast(p);
            p.SetInactive();
        }

        public IEnumerator DeaktivateOverTime(LinkedList<IPoolable> list, float timeInSec) {
            LinkedList<IPoolable> waitingList = new LinkedList<IPoolable>();
            while (list.Count > 0) {
                waitingList.AddFirst(list.First.Value);
                list.RemoveFirst();
            }
            foreach (IPoolable p in waitingList) {
                InactiveObjects.AddLast(p);
                p.SetInactive();
                yield return new WaitForSeconds(timeInSec);
            }
        }

        public GameObject PooledObject {
            get => pooledObject;
            set {
                pooledObject = value;
                Awake();
            }
        }
    }

    public interface IPoolable
    {
        public GameObject GameObject { get; }
        public ObjectPooling ObjectPooler { get; set; }
        public void SetActive();
        public void SetInactive();
    }
}