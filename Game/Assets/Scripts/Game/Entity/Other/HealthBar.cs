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
        private Canvas canvas;
        private Camera cam;
        private Action LockFacingDirection;
        private float targetValue = 1;

        private void Start() {
            parent = transform.root.gameObject;
            canvas = GetComponent<Canvas>();
            cam = Camera.main;
            LockFacingDirection = loockAt == LoockAt.Camera ? loockToCamera : loockToParent;
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
                hpBar.fillAmount = Mathf.MoveTowards(hpBar.fillAmount, targetValue, reductionSpeed * Time.deltaTime);
            }
        }

        private void ControleCanvasVisibility() {
            if (targetValue < 1) EnableCanves();
            else if (canvas.enabled) Invoke(nameof(DisableCanves), 1);
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

        private void EnableCanves() => canvas.enabled = true;
        private void DisableCanves() => canvas.enabled = false;

        private enum LoockAt { Parent, Camera }
    }
}