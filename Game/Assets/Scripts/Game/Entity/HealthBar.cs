using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Entity
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image hpBar;
        [SerializeField] private LoockAt loockAt = LoockAt.Parent;
        [SerializeField] private float reductionSpeed = 1.5f;

        private GameObject parent;
        private float targetValue = 1;
        private Camera cam;
        private Action LockFacingDirection;

        private void Start() {
            parent = transform.root.gameObject;
            LockFacingDirection = loockAt == LoockAt.Camera ? loockToCamera : loockToParent;
            cam = Camera.main;
            IDamagable damagable = parent.GetComponent<IDamagable>();
            IRepairable repairable = parent.GetComponent<IRepairable>();
            damagable.OnDamaged += UpdateHealthBar;
            repairable.OnRepaired += UpdateHealthBar;
        }

        private void Update() {
            LockFacingDirection();
            hpBar.fillAmount = Mathf.MoveTowards(hpBar.fillAmount, targetValue, reductionSpeed * Time.deltaTime);
        }

        private void UpdateHealthBar(int maxHP, int hp, int damage) {
            targetValue = (float)hp / (float)maxHP;
        }

        private void loockToCamera() {
            transform.rotation = cam.transform.rotation;
        }

        private void loockToParent() {
            Quaternion rot = parent.transform.rotation;
            transform.rotation = new Quaternion(0 - rot.x, 0 - rot.y, 0 - rot.z, 0);
        }

        private enum LoockAt { Parent, Camera }
    }
}