using UnityEngine;
using UnityEditor;
using Game.Entity.Tank;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float FOV = 10;
        [SerializeField] private float extended_FOV = 14;

        protected GameObject enemyPlayer;
        protected Tank tank;

        private LayerMask obstacleLayer;

        protected void Awake() {
            tank = GetComponent<Tank>();
            obstacleLayer = LayerMask.GetMask("Obstacle");
        }

        protected void Start() {
            enemyPlayer = GameObject.FindGameObjectWithTag(Magic.Tags.Player).transform.root.gameObject;
        }

        protected virtual bool CanSeePlayer() {
            if (PlayerIsOutOfSightRadius()) {
                return false;
            }
            return !Physics.Linecast(transform.position, enemyPlayer.transform.position, obstacleLayer);

        }

        private bool PlayerIsOutOfSightRadius() {
            bool playerOutsideExtendedSight = Vector3.Distance(transform.position, enemyPlayer.transform.position) > extended_FOV;
            
            if (playerOutsideExtendedSight) return true;

            bool playerOutsideSight = Vector3.Distance(transform.position, enemyPlayer.transform.position) > FOV;
            return playerOutsideSight;
        }

        private void OnDrawGizmos() {
            Handles.color = new Color(0, 1, 0, 0.2f);
            //Handles.DrawSolidDisc(transform.position, transform.up, FOV);
        }
    }
}