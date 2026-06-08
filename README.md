# BattleSnake 🐍

BattleSnake is a 2D arcade game for Android developed with Unity and C#. Control a snake, avoid incoming enemies and arena borders, and shoot venom to survive through increasingly difficult levels.

This is a legacy personal game project originally created to practice game development concepts such as touch controls, 2D physics, enemy behavior, collision detection, scoring, and level progression.

## 🎮 Gameplay

The player controls a continuously moving snake using an on-screen horizontal joystick.

The objective is to eliminate every enemy in the level by shooting venom while avoiding direct contact with enemies and the arena borders.

After defeating all enemies:

- The player advances to the next level.
- More enemies are generated.
- The snake grows by one body segment.
- The difficulty gradually increases.

Complete all levels to win the game.

## ✨ Features

- Touch-friendly controls for Android
- Horizontal joystick movement
- Venom shooting mechanic
- Enemies that follow the player
- Progressive level system
- Dynamic enemy generation
- Snake body growth
- Health system
- Score tracking
- Local best-score storage
- Instructions and high-score menus
- Game Over and victory screens

## 🕹️ Controls

| Control | Action |
|---|---|
| Drag the joystick left or right | Rotate and control the snake |
| Tap the venom button | Shoot at enemies |

## 🛠️ Technologies

- Unity `2019.4.41f2`
- C#
- Unity 2D Physics
- Unity UI
- Android SDK
- PlayerPrefs for local score storage

## 📂 Project Structure

```text
Assets/
├── Images/          # Game sprites and interface images
├── Music/           # Audio assets
├── Prefabs/         # Snake, enemies and projectile prefabs
├── Resources/       # Runtime resources
├── Scenes/
│   ├── Menu.unity
│   └── GameScene.unity
└── Scripts/
    ├── Botones.cs   # Touch controls and menu interface
    ├── Enemy.cs     # Enemy movement and player tracking
    ├── Motor.cs     # Levels, scoring and game state
    ├── Snake.cs     # Player movement, health and shooting
    └── Venom.cs     # Projectile behavior and collisions
```

## 🚀 Running the Project

### Requirements

- Unity Hub
- Unity `2019.4.41f2` or a compatible version
- Android Build Support, when building the APK

### Installation

1. Clone the repository:

```bash
git clone https://github.com/Stefanyvl2210/BattleSnake.git
```

2. Open Unity Hub.
3. Select **Add project from disk**.
4. Choose the cloned project directory.
5. Open the `Assets/Scenes/Menu.unity` scene.
6. Press the **Play** button in Unity.

## 📱 Android Build

To generate an APK:

1. Open **File → Build Settings**.
2. Select **Android**.
3. Click **Switch Platform**.
4. Confirm that the menu and game scenes are included.
5. Select **Build**.
6. Choose where to save the APK.

## 📦 Download

The compiled Android version can be published and downloaded from the repository's **Releases** section:

[View BattleSnake releases](https://github.com/Stefanyvl2210/BattleSnake/releases)

> This game was developed with an older Unity and Android configuration. Compatibility with recent Android devices is not guaranteed.

## 👩‍💻 Author

Developed by [Stefany Vega](https://github.com/Stefanyvl2210).
