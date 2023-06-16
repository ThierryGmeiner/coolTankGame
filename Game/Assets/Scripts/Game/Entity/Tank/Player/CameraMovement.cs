using System;
using UnityEngine;

namespace Game.Cam
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float normalSpeed = 10;
        [SerializeField] private float sprintSpeed = 25;
        private float speed;

        private bool camIsLocked;
        private bool camIsMoving;

        // usefull references
        private Plane plane = new Plane(Vector3.up, 0);
        private Vector3 playerOffset;
        private Rigidbody rigidBody;
        private Quaternion defaultRotation;

        // move towards target when cam gets locked
        private Vector3 targetPos;
        private const float LERP_SPEED = 5;

        private void Awake() {
            speed = normalSpeed;
            defaultRotation = transform.rotation;
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start() {
            player ??= GameObject.FindGameObjectWithTag(Magic.Tags.Player);
        }

        public void Move(Vector2 direction) {
            rigidBody.velocity = new Vector3(direction.x * speed, 0, direction.y * speed);

            playerOffset = transform.position - player.transform.position;
            camIsMoving = direction != Vector2.zero;
        }

        private void Update() {
            bool camHasTarget = targetPos != noTarget;
            if (camHasTarget) {
                MoveTowardsPlayer();
            }
            else if (camIsLocked) {
                if (camIsMoving) ChangeLockingState();
                else FollowPlayer();
            }
        }

        private void FollowPlayer() {
            transform.rotation = defaultRotation;
            transform.position = player.transform.position + playerOffset;
        }

        private void MoveTowardsPlayer() {
            SetNewTarget();
            transform.position = Vector3.Lerp(transform.position, targetPos, LERP_SPEED * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f) {
                targetPos = noTarget;
            }
        }

        public void ChangeLockingState() {
            camIsLocked = !camIsLocked;
            if (camIsLocked) SetNewTarget();
        }

        public void SetNewTarget() {
            Vector3 newTargetPos = transform.position + (player.transform.position - GetScreenMiddlePosition());
            targetPos = new Vector3(newTargetPos.x, transform.position.y, newTargetPos.z);
        }

        private Vector3 GetScreenMiddlePosition() {
            float distance;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        public void EnableTurbo() => speed = sprintSpeed;

        public void DisableTurbo() => speed = normalSpeed;

        private Vector3 noTarget { get => Vector3.zero; }
    }
}