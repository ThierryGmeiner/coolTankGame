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
        private CameraMovement camera;

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
            controler.TankAttack.Enable();
            controler.Camera.Enable();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            controler.TankDrive.Turbo.started += (InputAction.CallbackContext c) => tank.Movement.EnableTurbo();
            controler.TankDrive.Turbo.canceled += (InputAction.CallbackContext c) => tank.Movement.DisableTurbo();
            controler.TankDrive.Jump.started += (InputAction.CallbackContext c) => tank.Movement.Jump();
            controler.TankAttack.ShootAttack.started 
                += (InputAction.CallbackContext c) => { if (!ClickOnTank()) tank.Attack.Shoot(tank.Head.transform.rotation); };
            controler.TankDrive.SetPath.started += (InputAction.CallbackContext c) => SetNewPath();
            camera = Camera.main.GetComponent<CameraMovement>();
            controler.Camera.LockCamera.started += (InputAction.CallbackContext c) => { if (ClickOnTank()) camera.LockCamToPlayer(); };
        }

        private void Update() {
            tank.Movement.RotateHead(GetMousePosition());
            camera.Move(controler.Camera.Move.ReadValue<Vector2>());
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

        private bool ClickOnTank()
            => tank.Movement.grid.GetNodeFromPosition(GetMousePosition()) == tank.Movement.grid.GetNodeFromPosition(tank.transform.position);

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