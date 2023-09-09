using SFML.Graphics;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace Critters;

public static class Configuration
{

    public static uint SidePanelWidth = 300;

    public static uint Height = 800;

    public static uint Width = 1600;

    public static Color Background => new Color(233, 233, 233);

    public static bool AllowCameraMovement => true;

    public static Key PanLeft => Key.Left;

    public static Key PanRight => Key.Right;

    public static Key PanUp => Key.Up;

    public static Key PanDown => Key.Down;

    public static Key ZoomIn => Key.Add;

    public static Key ZoomOut => Key.Subtract;

    public static Key RotateRight => Key.Num1;

    public static Key RotateLeft => Key.Num2;

    public static FloatRect SinglePlayer => new FloatRect(0, 0, 1, 1);

    public static FloatRect TwoPlayerLeft => new FloatRect(0, 0, 0.5f, 1);

    public static FloatRect TwoPlayerRight => new FloatRect(0.5f, 0, 0.5f, 1);

    public static FloatRect FourPlayerTopLeft => new FloatRect(0, 0, 0.5f, 0.5f);

    public static FloatRect FourPlayerTopRight => new FloatRect(0.5f, 0, 0.5f, 0.5f);

    public static FloatRect FourPlayerBottomLeft => new FloatRect(0, 0.5f, 0.5f, 0.5f);

    public static FloatRect FourPlayerBottomRight => new FloatRect(0.5f, 0.5f, 0.5f, 0.5f);

    public static bool DrawToFile => false;

    public static float CameraMovementSpeed => 1200f;

    public static float CameraZoomSpeed => 0.05f;

    // Degrees per second
    public static float CameraRotationSpeed => 45f;

    // Pause Key
    public static Keyboard.Key PauseKey => Keyboard.Key.P;

    // Quit Key
    public static Keyboard.Key QuitKey => Keyboard.Key.Q;
    // WARNING: Towns may overlap as there is no logic for their placement.
    public static bool UseRandomTowns => false;

    // NOTE: TownCount only applies when using random towns.
    public static int TownCount => 20;

}