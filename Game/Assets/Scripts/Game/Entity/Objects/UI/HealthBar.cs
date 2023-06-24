using System;
using UnityEngine;
using UnityEngine.UI;
using Game.Entity;

namespace Game.UI
{
    public class HealthBar : UiBar
    {
        protected override void Start() {
            base.Start();
            IDamagable damagable = parent.GetComponent<IDamagable>();
            IRepairable repairable = parent.GetComponent<IRepairable>();
            damagable.OnDamaged += UpdateHealthBar;
            repairable.OnRepaired += UpdateHealthBar;
        }

        private void Update() {
            // temporary function
            if (Input.GetKeyDown(KeyCode.H)) parent.GetComponent<IRepairable>()?.GetRepaired(20);

            ControleCanvasVisibility();
            // dont do the calculations when the canvas is invisible
            if (canvas.enabled) {
                LockFacingDirection();
                bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, targetValue, reductionSpeed * Time.deltaTime);
            }
        }

        private void UpdateHealthBar(int maxHP, int hp, int damage) => UpdateHealthBar(maxHP, hp, damage, Vector3.zero);

        private void UpdateHealthBar(int maxHP, int hp, int damage, Vector3 direction) {
            targetValue = (float)hp / (float)maxHP;
        }
    }
}