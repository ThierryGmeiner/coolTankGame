using System;
using UnityEngine;

namespace Game.Cam
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float speed = 10;

        private bool camIsMoving;
        private bool camIsLocked = false;
        private Plane plane = new Plane(Vector3.up, 0);
        private Vector3 playerOffset;

        // move towards target when cam gets locked
        private Vector3 targetPos;
        private const float LERP_SPEED = 5;

        private void Awake() {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void Start() {
            player ??= GameObject.FindGameObjectWithTag(Magic.Tags.Player);
        }

        public Rigidbody RigidBody { get; private set; }

        private void Update() {
            if (targetPos != Vector3.zero) {
                MoveTowardsPlayer();
                return;
            }
            // TODO make that the camera gets a child of the player
            // in this way the camera has the same velocity when the player moves
            if (camIsLocked) {
                if (CamIsTooFarAwayFromPlayer()) { camIsLocked = false; }
                else FollowPlayer();
            }
        }

        public void Move(Vector2 direction) {
            Vector3 newDirection = new Vector3(direction.x, 0, direction.y);
            RigidBody.AddForce(newDirection * speed, ForceMode.Force);

            playerOffset = transform.position - player.transform.position;
            camIsMoving = newDirection != Vector3.zero;
        }

        private void FollowPlayer() {
            if (!camIsMoving) {
                transform.position = player.transform.position + playerOffset;
            }
        }

        private void MoveTowardsPlayer() {
            transform.position = Vector3.Lerp(transform.position, targetPos, LERP_SPEED * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.1f) {
                targetPos = Vector3.zero;
            }
        }

        public void ChangeLockingState() {
            camIsLocked = !camIsLocked;
            if (camIsLocked) SetNewTarget();
        }

        private void SetNewTarget() {
            Vector3 newTargetPos = transform.position + (player.transform.position - GetScreenMiddlePosition());
            targetPos = new Vector3(newTargetPos.x, transform.position.y, newTargetPos.z);
        }

        private bool CamIsTooFarAwayFromPlayer() {
            float distance = Vector3.Distance(GetScreenMiddlePosition(), player.transform.position);
            return distance > 7;
        }

        private Vector3 GetScreenMiddlePosition() {
            float distance;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }
    }
}