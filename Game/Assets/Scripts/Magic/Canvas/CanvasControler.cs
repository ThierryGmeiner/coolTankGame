using System;
using UnityEngine;

namespace Magic.Canvas
{
    public class CanvasControler : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private LoockAt loockAt = LoockAt.Parent;

        private Camera cam;
        private Action LockFacingDirection;
        
        private void Start() {
            cam = Camera.main;
            LockFacingDirection = loockAt == LoockAt.Camera ? loockToCamera : loockToParent;
        }

        private void Update() {
            LockFacingDirection();
        }

        private void loockToCamera() {
            transform.rotation = cam.transform.rotation;
        }

        private void loockToParent() {
            Quaternion rot = parent.rotation;
            transform.rotation = new Quaternion(0 - rot.x, 0 - rot.y, 0 - rot.z, 0);
        }

        private enum LoockAt { Parent, Camera }
    }
}