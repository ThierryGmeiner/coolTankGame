using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;
using Game.AI;

namespace Game.InputSystem
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler controler;
        private Tank tank;
        private Plane plane = new Plane(Vector3.up, 0);

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            controler.TankDrive.Turbo.started += (InputAction.CallbackContext context) => tank.Movement.EnableTurbo();
            controler.TankDrive.Turbo.canceled += (InputAction.CallbackContext context) => tank.Movement.DisableTurbo();
            controler.TankDrive.Jump.started += (InputAction.CallbackContext context) => tank.Movement.Jump();
        }

        private void Update() {
            SetLookDirection(tank.TankHead.transform);
            if (Input.GetMouseButtonDown(1)) SetNewPath();
            if (Input.GetMouseButtonDown(0)) Shoot();
        }

        private void SetNewPath() {
            Vector3 startPos = transform.position, targetPos = GetMousePosition();
            Thread pathFindingThread = new Thread(() => tank.Movement.SetPath(startPos, targetPos));
            pathFindingThread.Start();
            //tank.Movement.SetPath(startPos, targetPos);
        }

        private void Shoot() {
            Vector3 attackDirection = -(tank.ShootingSpot.position - GetMousePosition());
            tank.Attack.Shoot(new Vector3(attackDirection.x, 0, attackDirection.z));
        }

        public void SetLookDirection(Transform transform) {
            Vector3 mousePos = GetMousePosition();
            transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        }

        private Vector3 GetMousePosition() {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

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