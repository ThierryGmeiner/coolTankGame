using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class BarValue : MonoBehaviour
    {
        [SerializeField] private LoockAt loockAt = LoockAt.Parent;
        [SerializeField] protected Image bar;
        [SerializeField] protected float speed = 1.5f;
        [SerializeField] protected GameObject parent;

        protected Canvas canvas;
        protected Camera cam;
        protected Action LockFacingDirection;

        protected const float FULL_BAR = 1;
        protected float targetValue = 1;

        protected virtual void Start() {
            canvas = GetComponent<Canvas>();
            cam = Camera.main;
            LockFacingDirection = loockAt == LoockAt.Camera ? loockToCamera : loockToParent;
        }

        protected virtual void Update() {
            ControleCanvasVisibility();
            if (canvas.enabled) {
                bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, targetValue, speed * Time.deltaTime);
                LockFacingDirection();
            }
        }

        protected void loockToCamera() {
            transform.rotation = cam.transform.rotation;
        }

        protected void loockToParent() {
            transform.rotation = 
                new Quaternion(0 - parent.transform.rotation.x, 0 - parent.transform.rotation.y, 0 - parent.transform.rotation.z, 0);
        }

        protected void ControleCanvasVisibility() {
             if (targetValue < FULL_BAR) {
                CancelInvoke(nameof(DisableCanves));
                canvas.enabled = true;
            }
            else if (canvas.enabled) {
                Invoke(nameof(DisableCanves), 1);
            }
        }

        protected void DisableCanves() => canvas.enabled = false;

        private enum LoockAt { Parent, Camera }
    }
}