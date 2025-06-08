# Unity FPS Controller Base

[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=for-the-badge&logo=unity)](https://unity.com)

A foundational Unity project for a First-Person Shooter (FPS) controller. This repository provides a clean and extendable starting point for your next FPS game. It is built using Unity's new Input System and a simple event-based architecture for clean, decoupled code.

This project is a great learning tool or a solid base for a more complex character controller.

## ✨ Current Features

-   [x] **Basic Movement**: Physics-based walking and strafing using `Rigidbody`.
-   [x] **Sprinting**: Hold a key to move faster.
-   [x] **Jumping**: A simple physics-based jump with a configurable cooldown.
-   [x] **Mouse Look**: Smooth first-person camera control for looking around.
-   [x] **State-Driven Movement**: A simple state machine (`walking`, `sprinting`, `air`) to manage player behavior and speed.
-   [x] **Event-Based Input**: Uses a custom `EventBus` to decouple input from player actions, making the system modular and easy to extend.
-   [x] **Unity's New Input System**: All controls are managed through an `.inputactions` asset.
-   [x] **Universal Render Pipeline (URP)**: The project is set up with URP for modern, scalable graphics.

## 🚧 To-Do / Known Limitations

The primary goal of this repository is to provide a base. The most notable missing feature is:

-   [ ] **Slope Handling**: The controller **does not handle slopes correctly**. The player will slide down steep surfaces and may have difficulty walking up even gentle ones without losing speed. Implementing proper slope detection and adjustment is the top priority for future development.

## 🚀 Getting Started

### Prerequisites

-   **Unity Editor**: This project was created with **Unity 6000.0.42f1**. You will need this version or a newer one to open it without issues.
-   **Git**: To clone the repository.

### Installation

1.  Clone the repository to your local machine:
    ```sh
    git clone https://github.com/YOUR_USERNAME/YOUR_REPOSITORY_NAME.git
    ```
2.  Open the project folder in the Unity Hub.
3.  Unity will automatically import the project and install the required packages (like the Input System and Universal RP).
4.  Once the project is open, navigate to the `Assets/Scenes/` folder and open the **`GameScene.unity`** scene.
5.  Press **Play** to test the controller.

## 🧠 Core Concepts & Code

The project is designed to be simple and easy to understand. Here are the key scripts and concepts:

### `PlayerMovement.cs`
This is the heart of the controller. It's responsible for:
-   Applying forces to the `Rigidbody` for movement.
-   Handling the `MovementState` enum to switch between walking, sprinting, and in-air speeds.
-   Managing jump logic and ground detection via a `Raycast`.
-   Listening for input events from the `EventBus`.

### `PlayerCam.cs`
A straightforward script that controls the camera's rotation based on mouse input. It directly rotates the camera for vertical look (pitch) and an `orientation` Transform for horizontal look (yaw), which ensures the player's body rotates with the camera.

### `InputManager.cs`
This is a Singleton that serves as the bridge between **Unity's Input System** and the rest of the game.
-   It initializes the `InputSystem_Actions`.
-   It subscribes to input events (like `Jump.performed` or `Sprint.canceled`).
-   When an input occurs, it publishes a custom event to the `EventBus` (e.g., `Evt_PlayerJumpAction`).

### `EventBus.cs`
A simple static class that implements the publish-subscribe pattern. This allows different systems to communicate without having direct references to each other.

**Example Flow for Jumping:**
1.  The user presses the **Space Bar**.
2.  Unity's Input System triggers the `Jump` action.
3.  `InputManager.cs` catches this and calls `EventBus.Publish(new Evt_PlayerJumpAction());`.
4.  `PlayerMovement.cs`, which is subscribed to `Evt_PlayerJumpAction`, receives the event and executes its jump logic.

```csharp
// InputManager.cs - Publishing an event
private void OnEnable() {
    inputActions.Player.Jump.performed += e => EventBus.Publish(new Evt_PlayerJumpAction());
    //...
}

// PlayerMovement.cs - Subscribing to the event
private void OnEnable() {
    EventBus.Subscribe<Evt_PlayerJumpAction>(OnJumpPerformed);
    //...
}

private void OnJumpPerformed(Evt_PlayerJumpAction evt) {
    // Handle the jump logic here
}
```

## 📂 Project Structure

The project is organized into logical folders within the `Assets` directory:

```
├── Assets
│   ├── Materials/      # Materials for the player, ground, and physics.
│   ├── Scenes/         # Contains the main demo scene (GameScene).
│   ├── Scripts/        # All C# source code.
│   │   ├── CameraSystem/   # Scripts for camera control.
│   │   ├── EventSystem/    # The EventBus and custom event definitions.
│   │   ├── InputSystem/    # InputManager and generated Input Actions.
│   │   └── PlayerSystem/   # The core player controller logic.
│   ├── Settings/       # URP Render Pipeline assets and Volume Profiles.
│   └── Textures/       # Textures used for the ground.
├── Packages/           # Unity package manifest.
└── ProjectSettings/    # Unity project configuration files.
```

## 📦 Dependencies

This project relies on the following key Unity packages, which should be installed automatically:
-   **Input System** (`com.unity.inputsystem`)
-   **Universal RP** (`com.unity.render-pipelines.universal`)
