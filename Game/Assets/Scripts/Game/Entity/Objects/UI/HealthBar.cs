using UnityEngine;
using Game.Entity;

namespace Game.UI
{
    public class HealthBar : BarValue
    {
        protected override void Start() {
            base.Start();
            IDamagable damagable = parent.GetComponent<IDamagable>();
            IRepairable repairable = parent.GetComponent<IRepairable>();
            damagable.OnDamaged += UpdateHealthBar;
            repairable.OnRepaired += UpdateHealthBar;
        }

        protected override void Update() {
            base.Update();
        }

        private void UpdateHealthBar(int maxHP, int hp, int damage) => UpdateHealthBar(maxHP, hp, damage, Vector3.zero);

        private void UpdateHealthBar(int maxHP, int hp, int damage, Vector3 direction) {
            targetValue = (float)hp / (float)maxHP;
        }
    }
}