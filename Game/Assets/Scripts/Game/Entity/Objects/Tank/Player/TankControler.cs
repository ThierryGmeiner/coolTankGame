using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;
using Game.Cam;
using Game.UI;

namespace Game.InputSystem
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler controler;
        private Tank tank;
        private TankMovement movement;
        private CameraMovement cam;

        private ScrolingWheel inventory;
        private static LayerMask playerLayer;
        private static LayerMask entityLayer;
        private static Plane plane = new Plane(Vector3.up, 0);

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
            controler.TankAttack.Enable();
            controler.Camera.Enable();
            controler.Inventory.Enable();

            playerLayer = LayerMask.GetMask("Player");
            entityLayer = LayerMask.GetMask("Entity");
        }

        private void Start() {
            tank = GetComponent<Tank>();
            movement = tank.Movement;
            cam = Camera.main.GetComponent<CameraMovement>();
            inventory = gameObject.GetComponentInChildren<ScrolingWheel>(includeInactive: true);

            SetupControlsMovement();
            SetupControlsAttack();
            SetupControlsCameraMovement();
            SetupControlerInventory();
        }

        private void Update() {
            SetHeadRotationTarget();
            cam.Move(controler.Camera.Move.ReadValue<Vector2>());
        }

        private void SetHeadRotationTarget() {
            Vector3 mousePos = GetMousePosition();
            Collider[] targetedEntity = Physics.OverlapSphere(mousePos, 1f, entityLayer);

            if (targetedEntity.Length > 0) {
                movement.HeadRotationTarget = targetedEntity[0].transform.position;
                return;
            }
            movement.HeadRotationTarget = mousePos;
        }

        public static Vector3 GetMousePosition() {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        private void SetupControlsMovement() {
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
            controler.Camera.LockCamera.started += (InputAction.CallbackContext c) => {
                if (ClickOnTank()) cam.ChangeLockingState(lockCam: true);
            };
        }

        private void SetupControlerInventory() {
            controler.Inventory.OpenInventory.started += (InputAction.CallbackContext c) => {
                controler.TankDrive.Disable();
                controler.TankAttack.Disable();
                controler.Camera.Disable();
                inventory.gameObject.SetActive(true);
                cam.ChangeLockingState(lockCam: true);
            };
            controler.Inventory.OpenInventory.canceled += (InputAction.CallbackContext c) => {
                controler.TankDrive.Enable();
                controler.TankAttack.Enable();
                controler.Camera.Enable();
                inventory.gameObject.SetActive(false);
                cam.ChangeLockingState(lockCam: false);
            };
            controler.Inventory.Select.canceled += (InputAction.CallbackContext c) => {
                if (inventory.gameObject.activeSelf) inventory.SelectItem();
            };
        }

        private bool ClickOnTank() => Physics.CheckSphere(GetMousePosition(), 0.3f, playerLayer);
    }
}