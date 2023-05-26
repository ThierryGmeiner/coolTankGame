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
        private AStar aStar;
        private AStarGrid grid;

        private void Awake() {
            controler = new PlayerControler();
            controler.TankDrive.Enable();
            grid = GameObject.Find("A*").GetComponent<AStarGrid>();
            aStar = new AStar(grid, System.Math.Max(transform.localScale.x, transform.localScale.z) / 2);
        }

        private void Start() {
            tank = GetComponent<Tank>();
            controler.TankDrive.Turbo.started += (InputAction.CallbackContext context) => tank.Movement.EnableTurbo();
            controler.TankDrive.Turbo.canceled += (InputAction.CallbackContext context) => tank.Movement.DisableTurbo();
            controler.TankDrive.Jump.started += (InputAction.CallbackContext context) => tank.Movement.Jump();
        }

        private void Update() {
            SetLookDirection(tank.TankHead.transform);
            if (Input.GetMouseButtonDown(1)) {
                SetPath(GetMousePosition());
            }
            if (Input.GetMouseButtonDown(0)) {
                Vector3 attackDirection = -(tank.ShootingSpot.position - GetMousePosition());
                tank.Attack.Shoot(new Vector3(attackDirection.x, 0, attackDirection.z));
            }
        }

        public void SetLookDirection(Transform transform) {
            Vector3 mousePos = GetMousePosition();
            transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        }

        public void SetPath(Vector3 targetPos) {
            AStarNode[] newPath = GetPath(targetPos);
            if (newPath.Length > 0) {
                tank.Movement.Path = newPath;
            }
        }

        private AStarNode[] GetPath(Vector3 targetPos) {
            AStarNode startPos = grid.GetNodeFromPosition(transform.position);
            AStarNode TargetPos = grid.GetNodeFromPosition(targetPos);
            return aStar.FindOptimizedPath(startPos, TargetPos);
        }

        private Vector3 GetMousePosition() {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        private void OnDrawGizmos() {
            if (tank.Movement.Path == null) return;
            var oldPath = aStar.FindPath(aStar.StartNode, aStar.TargetNode);
            foreach (AStarNode node in oldPath) {
                Gizmos.color = Color.cyan;
                if (Magic.Array.Contains(tank.Movement.Path, node)) Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(node.Position.x, 0.25f, node.Position.z), Vector3.one * (0.5f));
            }
        }
    }
}