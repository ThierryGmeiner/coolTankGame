using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;
using Game.Cam;
using Game.AI;

namespace Game.InputSystem
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler controler;
        private Plane plane = new Plane(Vector3.up, 0);
        private Tank tank;
        private CameraMovement cam;

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
            controler.TankAttack.Enable();
            controler.Camera.Enable();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            cam = Camera.main.GetComponent<CameraMovement>();
            
            SetupControlsMovement();
            SetupControlsAttack();
            SetupControlsCameraMovement();
        }

        private void Update() {
            tank.Movement.HeadRotationTarget = GetMousePosition();
            cam.Move(controler.Camera.Move.ReadValue<Vector2>());
        }

        private void SetNewPath() {
            Vector3 startPos = transform.position, targetPos = GetMousePosition();
            Thread pathFindingThread = new Thread(() => tank.Movement.SetPath(startPos, targetPos));
            pathFindingThread.Start();
        }

        private Vector3 GetMousePosition() {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        private void SetupControlsMovement() {
            controler.TankDrive.Jump.started += (InputAction.CallbackContext c) => tank.Movement.Jump();
        }

        private void SetupControlsAttack() {
            controler.TankDrive.SetPath.started += (InputAction.CallbackContext c) => SetNewPath();
            controler.TankAttack.ShootAttack.started += (InputAction.CallbackContext c) => { 
                if (!ClickOnTank()) tank.Attack.Shoot(tank.Head.transform.rotation); 
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

        private bool ClickOnTank() => Physics.CheckSphere(GetMousePosition(), 0.3f, LayerMask.GetMask("Player"));

        private void OnDrawGizmos() {
            if (tank == null || tank.Movement == null) return;
            if (tank.Movement.Path == null || tank.Movement.Path.Nodes.Length == 0) return;

            foreach (AStarNode node in tank.Movement.Path.Nodes) {
                Gizmos.color = Color.cyan;
                if (Magic.Array.Contains(tank.Movement.Path.Nodes, node)) Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(node.Position.x, 0.25f, node.Position.z), Vector3.one * (0.5f));
            }
        }
    }
}