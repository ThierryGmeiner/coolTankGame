using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Entity;
using Game.Entity.Tank;
using Game.AI;
using Magic;

namespace Tests
{
    public static class TestHelper
    {
        public static GameObject CreateTank() {
            if (GameObject.Find("A*") == null) CreateASTarGrid();
            GameObject tank = new GameObject();
            tank.name = "TestTank";
            tank.tag = Tags.Entity;
            tank.layer = 6;
            tank.AddComponent<BoxCollider>();
            Tank tankClass = tank.AddComponent<Tank>();
            tankClass.Data = ScriptableObject.CreateInstance<TankData>();
            tank.AddComponent<TankHealth>(); 
            tank.AddComponent<TankAttack>();
            return tank;
        }

        public static T CreateTank<T>() => CreateTank().GetComponent<T>();

        public static GameObject CreateBullet() {
            GameObject bullet = new GameObject();
            bullet.AddComponent<Rigidbody>();
            bullet.AddComponent<BoxCollider>();
            bullet.AddComponent<DefaultBullet>();
            bullet.AddComponent<PlannedTimer>();
            return bullet;
        }

        public static T CreateBullet<T>() => CreateBullet().GetComponent<T>();

        public static GameObject CreateGround() {
            GameObject ground = new GameObject();
            ground.transform.localScale = new Vector3(20, 0.1f, 20);
            ground.name = "TestGround";
            ground.tag = Tags.Enviorment;
            ground.layer = 3;
            ground.AddComponent<BoxCollider>();
            return ground;
        }

        public static T CreateGround<T>() => CreateGround().GetComponent<T>();

        public static GameObject CreateObstacle(Vector3 size) {
            GameObject obstacle = new GameObject(); Debug.Log("create");

            obstacle.transform.localScale = size;
            obstacle.name = "Obstacle";
            obstacle.tag = Tags.Enviorment;
            obstacle.layer = 7;
            obstacle.AddComponent<BoxCollider>();
            return obstacle;
        }

        public static AStarGrid CreateASTarGrid() {
            GameObject obj = new GameObject();
            obj.name = "A*";
            obj.tag = Tags.Untagged;
            obj.layer = 0;
            return obj.AddComponent<AStarGrid>();
        }
        
        public static AStarNode[,] CreateAStarNodeArray(int x, int y) {
            AStarNode[,] grid = new AStarNode[x, y];
            for (int i = 0; i < grid.GetLength(0); i++) {
                for (int j = 0; j < grid.GetLength(1); j++) {
                    // pos + 0.5 because that's the default radius of the nodes
                    grid[i, j] = new AStarNode(true, new Vector2(i + 0.5f, j + 0.5f), new Vector2Int(i, j));
                }
            }
            return grid;
        }

        public static void DestroyObjects(GameObject gameObject) {
            gameObject.name = "Destroyed";
            gameObject.tag = Tags.Untagged;
            gameObject.layer = 0;
            gameObject.transform.position = GetEmtySpace(gameObject.transform.localScale);
            GameObject.Destroy(gameObject);
        }

        public static void DestroyObjects(GameObject obj1, GameObject obj2) {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3) {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4) {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
            DestroyObjects(obj4);
        }
        public static void DestroyObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4, GameObject obj5) {
            DestroyObjects(obj1);
            DestroyObjects(obj2);
            DestroyObjects(obj3);
            DestroyObjects(obj4);
            DestroyObjects(obj5);
        }
        public static void DestroyObjects(GameObject[] gameObjects) {
            foreach (GameObject gameObject in gameObjects)
                DestroyObjects(gameObject);
        }

        public static Vector3 GetEmtySpace(Vector3 localScale) {
            Vector3 spawnPos;
            float radius = Magic.MathM.Max(localScale.x, localScale.y, localScale.z) * 2;
            do {
                spawnPos = new Vector3(Random.Range(10000, 1000000), Random.Range(10000, 1000000), Random.Range(10000, 1000000));
            } while (Physics.CheckSphere(spawnPos, radius));
            return spawnPos;
        }

        public static void LoadEmptyScene() {
            SceneManager.LoadScene("Test_EmptyScene", LoadSceneMode.Single);
        }
    }
}