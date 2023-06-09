using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class TankAI : EnemyAI
    {

        private void Update() {

        }

        private void HandleMovement() {

            if (Vector3.Distance(transform.position, enemyPlayer.transform.position) < 4) {
                tank.Movement.Path = null;
                return;
            }

            if (CanSeePlayer()) {
                if (tank.Movement.Path == null || tank.Movement.Path.Nodes.Length == 0) {
                    tank.Movement.SetPath(transform.position, enemyPlayer.transform.position);
                }
            }
        }
    }
}