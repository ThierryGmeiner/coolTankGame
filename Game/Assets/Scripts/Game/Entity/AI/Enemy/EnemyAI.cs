using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity.Tank;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float sightRadius = 10;

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

        protected bool CanSeePlayer() {
            if (Vector3.Distance(transform.position, enemyPlayer.transform.position) > sightRadius) {
                return false;
            }
            return !Physics.Linecast(transform.position, enemyPlayer.transform.position, obstacleLayer);
        }

    }
}