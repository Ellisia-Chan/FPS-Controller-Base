using UnityEngine;
using InputSystem;

namespace CameraSystem {
	public class PlayerCam : MonoBehaviour {

        [SerializeField] private float mouseSensX;
        [SerializeField] private float mouseSensY;

		[SerializeField] private Transform orientation;

        private float mouseX;
        private float mouseY;

		private float xRotation;
		private float yRotation;

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            MouseInput();
            CamRotation();
        }

        private void MouseInput() {
            mouseX = InputManager.Instance.GetMouseAxisVector().x * Time.deltaTime * mouseSensX;
            mouseY = InputManager.Instance.GetMouseAxisVector().y * Time.deltaTime * mouseSensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }

        private void CamRotation() {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}