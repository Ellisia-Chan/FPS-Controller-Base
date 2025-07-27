using UnityEngine;

namespace Interface {
    public interface IInteractable {
        /// <summary>
        /// The text prompt to display to the player when they can interact with this object.
        /// </summary>
        public string InteractionPrompt { get; }

        /// <summary>
        /// Called when the player's crosshair enters the object's collider.
        /// </summary>
        public void OnFocus();

        /// <summary>
        /// Called when the player's crosshair exits the object's collider.
        /// </summary>
        public void OnLoseFocus();

        /// <summary>
        /// Called when the player presses the interact button while focusing on this object.
        /// </summary>
        /// <param name="interactor">The GameObject that is performing the interaction.</param>
        public void OnInteract(GameObject interactor);
    } 
}

