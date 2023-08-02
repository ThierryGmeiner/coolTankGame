using System;
using UnityEngine;

namespace Game.Cam
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float normalSpeed = 7;
        [SerializeField] private float sprintSpeed = 18;
        private float speed;

        private bool camIsLocked;
        private bool camIsMoving;

        // usefull references
        private Plane plane = new Plane(Vector3.up, 0);
        private Vector3 playerOffset;
        private Rigidbody rigidBody;
        private Quaternion defaultRotation;
        private Camera cam;

        // move towards target when cam gets locked
        private Vector3 targetPos;
        private const float LERP_SPEED = 5;

        private void Awake() {
            speed = normalSpeed;
            defaultRotation = transform.rotation;
            rigidBody = GetComponent<Rigidbody>();
            cam = GetComponent<Camera>();
        }

        private void Start() {
            player ??= GameObject.FindGameObjectWithTag(Magic.Tags.Player).transform.root.gameObject;
        }

        public void Move(Vector2 direction) {
            float speedMultiplier = (speed * cam.orthographicSize) / 5;
            rigidBody.velocity = new Vector3(direction.x * speedMultiplier, 0, direction.y * speedMultiplier);

            playerOffset = transform.position - player.transform.position;
            camIsMoving = direction != Vector2.zero;
        }

        private void Update() {
            ControlZoom();
            bool camHasTarget = targetPos != noTarget;

            if (camHasTarget) {
                MoveTowardsPlayer();
            }
            else if (camIsLocked) {
                if (camIsMoving) camIsLocked = false;
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

        private void ControlZoom() {
            cam.orthographicSize += Input.mouseScrollDelta.y * -0.4f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 3, 12);
        }

        public void ChangeLockingState() {
            camIsLocked = !camIsLocked;
            if (camIsLocked) SetNewTarget();
        }

        public void ChangeLockingState(bool lockCam) {
            camIsLocked = lockCam;
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