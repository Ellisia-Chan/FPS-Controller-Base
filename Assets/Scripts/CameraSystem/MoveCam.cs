using UnityEngine;

namespace CameraSystem {
    public class MoveCam : MonoBehaviour {
        [SerializeField] private Transform cameraPos;

        private void Update() {
            transform.position = cameraPos.position;
        }
    }
}