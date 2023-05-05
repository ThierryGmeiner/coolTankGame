using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;

namespace Game.Input
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler inputAction;
        private Tank tank;

        private void Awake() {
            inputAction = new PlayerControler();
            inputAction.TankDrive.Enable();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            inputAction.TankDrive.Turbo.started += (InputAction.CallbackContext context) => tank.Movement.EnableTurbo();
            inputAction.TankDrive.Turbo.canceled += (InputAction.CallbackContext context) => tank.Movement.DisableTurbo();
        }

        private void FixedUpdate() {
            tank.Movement.Move(inputAction.TankDrive.Move.ReadValue<Vector2>());
        }
    }
}