using System;
using UnityEngine;
using UnityEngine.UI;
using Game.Entity;

namespace Game.UI
{
    public class AttackBar : BarValue
    {
        private IRangeAttack rangeAttack;

        protected override void Start() {
            base.Start();
            rangeAttack = parent.GetComponent<IRangeAttack>();
            rangeAttack.OnUpdateShotsUntilCooldown += UpdateAttackBar;
        }

        protected override void Update() {
            base.Update();
        }

        private void UpdateAttackBar() {
            targetValue = (float)rangeAttack.ShotsUntilCooldown / (float)rangeAttack.MaxShotsUntilCooldown;
        }
    }
}