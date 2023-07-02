using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;
using Game.Cam;

namespace Game.InputSystem
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler controler;
        private Plane plane = new Plane(Vector3.up, 0);
        private Tank tank;
        private TankMovement movement;
        private CameraMovement cam;
        private LayerMask playerLayer;
        private LayerMask entityLayer;

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
            controler.TankAttack.Enable();
            controler.Camera.Enable();
            playerLayer = LayerMask.GetMask("Player");
            entityLayer = LayerMask.GetMask("Entity");
        }

        private void Start() {
            tank = GetComponent<Tank>();
            movement = tank.Movement;
            cam = Camera.main.GetComponent<CameraMovement>();
            
            SetupControlsMovement();
            SetupControlsAttack();
            SetupControlsCameraMovement();
        }

        private void Update() {
            SetHeadRotationTarget();
            cam.Move(controler.Camera.Move.ReadValue<Vector2>());
        }

        private void SetHeadRotationTarget() {
            Vector3 mousePos = GetMousePosition();
            Collider[] targetedEntity = Physics.OverlapSphere(mousePos, 1, entityLayer);

            if (targetedEntity.Length > 0) {
                movement.HeadRotationTarget = targetedEntity[0].transform.position;
                return;
            }
            movement.HeadRotationTarget = mousePos;
        }

        private Vector3 GetMousePosition() {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        private void SetupControlsMovement() {
            controler.TankDrive.Jump.started += (InputAction.CallbackContext c) => movement.Jump();
            controler.TankDrive.SetPath.started += (InputAction.CallbackContext c) => {
                if (Input.GetKey(KeyCode.LeftShift)) movement.AddPath(GetMousePosition());
                else movement.SetPath(transform.position, GetMousePosition());
            };
        }

        private void SetupControlsAttack() {
            controler.TankAttack.ShootAttack.started += (InputAction.CallbackContext c) => { 
                if (!ClickOnTank()) tank.Attack.Shoot(Magic.MathM.ConvertToVector3(tank.Head.transform.rotation.eulerAngles.y)); 
            };
        }

        private void SetupControlsCameraMovement() {
            controler.Camera.MoveFaster.started += (InputAction.CallbackContext c) => cam.EnableTurbo();
            controler.Camera.MoveFaster.canceled += (InputAction.CallbackContext c) => cam.DisableTurbo();
            controler.Camera.FindPlayer.started += (InputAction.CallbackContext c) => cam.ChangeLockingState();
            controler.Camera.LockCamera.started += (InputAction.CallbackContext c) => {
                if (ClickOnTank()) cam.ChangeLockingState();
            };
        }

        private bool ClickOnTank() => Physics.CheckSphere(GetMousePosition(), 0.3f, playerLayer);
    }
}