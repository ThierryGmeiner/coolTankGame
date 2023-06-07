using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class TankAI : EnemyAI
    {

        private void Update() {
            Debug.Log(CanSeePlayer());
        }


    }
}