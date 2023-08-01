using System.Collections.Generic;
using UnityEngine;
using Game.AI;
using Game.Entity.Interactable;
using Magic;

namespace Game.Data
{
    public class SceneData : MonoBehaviour {
        [SerializeField] private GameObject entityCotainer;
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject bulletContainer;
        [SerializeField] private GameObject pathParticleContainer;
        [SerializeField] private GameObject interactableContainer;
        [SerializeField] private GameObject repairBoxContainer;

        public GameObject[] AllObjects { get; private set; }
        public GameObject Player { get; private set; }
        public EnemyAI[] Enemys { get; private set; }
        public RepaiBox[] RepairBoxes { get; private set; }
        public AStarGrid AStarGrid { get; private set; }

        public GameObject EntityContainer => entityCotainer;
        public GameObject EnemyContainer => enemyContainer;
        public GameObject BulletCotainer => bulletContainer;
        public GameObject PathParticleContainer => pathParticleContainer;
        public GameObject InteractableContainer => interactableContainer;
        public GameObject RepairBoxContainer => repairBoxContainer;

        public void Awake() {
            gameObject.name = "SceneData";
            FindAllObjects();
            FindPlayer();
            FindEnemys();
            FindRepairBoxes();
            FindAStarGrid();
        }

        public void FindAllObjects() {
            GameObject[] objectsInSceene = FindObjectsOfType<GameObject>();
            List<GameObject> activeObjects = new();
            for (int i = 0; i < objectsInSceene.Length; i++) {
                if (objectsInSceene[i].activeInHierarchy) {
                    activeObjects.Add(objectsInSceene[i]);
                }
            } AllObjects = activeObjects.ToArray();
        }

        public void FindPlayer() => Player = GameObject.FindGameObjectWithTag(Tags.Player);
        public void FindEnemys() => Enemys = SceneHelper.GetObjectsFromRoot<EnemyAI>(entityCotainer);
        public void FindRepairBoxes() => RepairBoxes = SceneHelper.GetObjectsFromRoot<RepaiBox>(RepairBoxContainer);
        public void FindAStarGrid() => AStarGrid = 
            GameObject.Find("AStarGrid")?.GetComponent<AStarGrid>() ?? Instantiate(new GameObject()).AddComponent<AStarGrid>();
    }
}