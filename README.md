# Unity FPS Controller Base

![alt text](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=for-the-badge&logo=unity)

A foundational Unity project for a First-Person Shooter (FPS) controller. This repository provides a clean and extendable starting point for your next FPS game. It is built using Unity's new Input System and a simple event-based architecture for clean, decoupled code.

This project is a great learning tool or a solid base for a more complex character controller.

# ✨ Current Features

Basic Movement: Physics-based walking and strafing using Rigidbody.

Sprinting: Hold a key to move faster.

Jumping: A simple physics-based jump with a configurable cooldown.

Mouse Look: Smooth first-person camera control for looking around.

State-Driven Movement: A simple state machine (walking, sprinting, air) to manage player behavior and speed.

Event-Based Input: Uses a custom EventBus to decouple input from player actions, making the system modular and easy to extend.

Unity's New Input System: All controls are managed through an .inputactions asset.

Universal Render Pipeline (URP): The project is set up with URP for modern, scalable graphics.

# 🚧 To-Do / Known Limitations

The primary goal of this repository is to provide a base. The most notable missing feature is:

Slope Handling: The controller does not handle slopes correctly. The player will slide down steep surfaces and may have difficulty walking up even gentle ones without losing speed. Implementing proper slope detection and adjustment is the top priority for future development.

# 🚀 Getting Started
Prerequisites

Unity Editor: This project was created with Unity 6000.0.42f1. You will need this version or a newer one to open it without issues.

Git: To clone the repository.

Installation

Clone the repository to your local machine:

git clone https://github.com/YOUR_USERNAME/YOUR_REPOSITORY_NAME.git


Open the project folder in the Unity Hub.

Unity will automatically import the project and install the required packages (like the Input System and Universal RP).

Once the project is open, navigate to the Assets/Scenes/ folder and open the GameScene.unity scene.

Press Play to test the controller.

# 📂 Project Structure

The project is organized into logical folders within the Assets directory:

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
IGNORE_WHEN_COPYING_START
content_copy
download
Use code with caution.
IGNORE_WHEN_COPYING_END
# 📦 Dependencies

This project relies on the following key Unity packages, which should be installed automatically:

Input System (com.unity.inputsystem)

Universal RP (com.unity.render-pipelines.universal)
