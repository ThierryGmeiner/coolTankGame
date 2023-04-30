using Game.Entity.Tank;
using Magic.Data;
using UnityEngine;

namespace Tests
{
    public static class TestHelper
    {
        public static GameObject CreateTank()
        {
            GameObject tank = new GameObject();
            tank.name = "TestTank";
            tank.tag = "Tank";
            tank.AddComponent<Rigidbody>();
            tank.AddComponent<BoxCollider>();
            tank.AddComponent<Tank>().Data = ScriptableObject.CreateInstance<TankData>();
            return tank;
        }

        public static T CreateTank<T>() => CreateTank().GetComponent<T>();

        public static void DestroyObjects(GameObject gameObject)
        {
            gameObject.name = "Destroyed";
            gameObject.tag = "Untagged";
            gameObject.transform.position = GetEmtySpace(gameObject.transform.localScale);
            GameObject.Destroy(gameObject);
        }
 
        public static void DestroyObjects(GameObject obj1, GameObject obj2)
        {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3)
        {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4)
        {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
            DestroyObjects(obj4);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4, GameObject obj5)
        {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
            DestroyObjects(obj4);
            DestroyObjects(obj5);
        }
        public static void DestroyObjects(GameObject[] gameObjects)
        {
            foreach (GameObject gameObject in gameObjects) 
                DestroyObjects(gameObject);
        }

        private static Vector3 GetEmtySpace(Vector3 localScale) 
        {
            Vector3 spawnPos;
            float radius = Magic.MathM.Max(localScale.x, localScale.y, localScale.z) * 2;
            do {
                spawnPos = new Vector3(Random.Range(10000, 1000000), Random.Range(10000, 1000000), Random.Range(10000, 1000000));
            } while (Physics.CheckSphere(spawnPos, radius));
            return spawnPos;
        }
    }
}