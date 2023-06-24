using System;
using UnityEngine;
using UnityEngine.UI;
using Game.Entity;

namespace Game.UI
{
    public class UiBar : MonoBehaviour
    {
        [SerializeField] private LoockAt loockAt = LoockAt.Parent;
        [SerializeField] protected Image bar;
        [SerializeField] protected float reductionSpeed = 1.5f;

        protected GameObject parent;
        protected Canvas canvas;
        protected Camera cam;
        protected Action LockFacingDirection;

        protected const float FULL_BAR = 1;
        protected float targetValue = 1;

        protected virtual void Start() {
            parent = transform.root.gameObject;
            canvas = GetComponent<Canvas>();
            cam = Camera.main;
            LockFacingDirection = loockAt == LoockAt.Camera ? loockToCamera : loockToParent;
        }

        protected void loockToCamera() {
            transform.rotation = cam.transform.rotation;
        }

        protected void loockToParent() {
            Quaternion rot = parent.transform.rotation;
            transform.rotation = new Quaternion(0 - rot.x, 0 - rot.y, 0 - rot.z, 0);
        }
        protected void ControleCanvasVisibility() {
            if (targetValue < FULL_BAR) EnableCanves();
            else if (canvas.enabled) Invoke(nameof(DisableCanves), 1);
        }

        protected void EnableCanves() => canvas.enabled = true;
        protected void DisableCanves() => canvas.enabled = false;

        private enum LoockAt { Parent, Camera }
    }
}