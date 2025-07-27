using UnityEngine;

using Interface;

namespace Objects.Interactable {
    public class Interactable_Cube : MonoBehaviour, IInteractable {
        public string InteractionPrompt => throw new System.NotImplementedException();

        public void OnFocus() {
            Debug.Log("OnFocus");
        }   

        public void OnInteract(GameObject interactor) {
            Debug.Log("OnInteract");
        }

        public void OnLoseFocus() {
            Debug.Log("OnLoseFocus");
        }
    }
}