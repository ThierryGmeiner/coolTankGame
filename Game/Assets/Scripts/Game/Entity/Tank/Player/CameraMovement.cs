using System;
using UnityEngine;

namespace Game.Cam
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float speed = 10;

        private bool camIsLocked = false;

        private void Awake() {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void Start() {
            player ??= GameObject.FindGameObjectWithTag(Magic.Tags.Player);
        }

        public Rigidbody RigidBody { get; private set; }

        private void Update() {
            if (camIsLocked) {
                FollowPlayer();    
            }            
        }

        private void FollowPlayer() {
            Debug.Log(playerOffset);
        }

        public void LockCamToPlayer() => camIsLocked = true;
        public void UnlockCamFromPlayer() => camIsLocked = false;

        private Vector2 playerOffset {
            get {
                return transform.position - player.transform.position;
            }
        }

        public void Move(Vector2 direction) {
            RigidBody.AddForce(direction * speed, ForceMode.Force);
        }
    }
}