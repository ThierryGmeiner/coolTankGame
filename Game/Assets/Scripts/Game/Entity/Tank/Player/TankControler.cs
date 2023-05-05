using UnityEngine;
using UnityEngine.InputSystem;
using Game.Entity.Tank;

namespace Game.Input
{
    [RequireComponent(typeof(Tank))]
    public class TankControler : MonoBehaviour
    {
        private PlayerControler controler;
        private Tank tank;

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

        private void FixedUpdate() {
            tank.Movement.Move(controler.TankDrive.Move.ReadValue<Vector2>());
        }
    }
}