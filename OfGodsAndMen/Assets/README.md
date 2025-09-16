# Of Gods and Men Core Scripts Package

This package contains the essential scripts for implementing the core systems of "Of Gods and Men: The End of an Era".

## Package Contents

```
OfGodsAndMen/
├── Scripts/
│   ├── GameManager.cs
│   ├── PlayerController.cs
│   ├── SceneManager.cs
│   ├── TestSceneController.cs
│   └── MainMenuController.cs
└── package.json
```

## Installation

1. Copy the `OfGodsAndMen` folder into your Unity project's `Packages` folder
2. Unity will automatically import the scripts
3. Create scenes named "MainMenu", "GameScene", "Settings", and "Credits"
4. Add the required components to GameObjects in each scene as described in the implementation README

## Required Unity Packages

This package requires the following Unity packages to be installed:
- TextMeshPro (com.unity.textmeshpro)
- AI Navigation (com.unity.ai.navigation)
- Addressables (com.unity.addressables)
- Scriptable Build Pipeline (com.unity.scriptablebuildpipeline)
- Cinemachine (com.unity.cinemachine)
- Post Processing (com.unity.post-processing)
- Animation Rigging (com.unity.animation.rigging)

## Usage

After installation, you can:
1. Attach the GameManager script to a GameObject in each scene
2. Attach the SceneManager script to a GameObject in each scene
3. Attach the PlayerController script to your player GameObject (requires Rigidbody)
4. Use the TestSceneController to verify script interactions
5. Use the MainMenuController for main menu navigation

These scripts provide a foundation for implementing the core game systems described in the design documents and should work without recoding when imported into a Unity project.