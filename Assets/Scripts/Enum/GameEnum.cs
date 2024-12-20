using UnityEngine;

public class GameEnum : MonoBehaviour
{
    public enum ScreenRoute
    {
        Waiting,
        Lobby,
        LobbyRoom,
        Screen1,
        Screen2,
        Screen3,
        Screen4,
        Screen5,
        Setting,
        Win,
        Debug
    }

    public enum GameFaction
    {
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        All,
        None
    }

    public enum CharacterAnimationState
    {
        Idle = 0,
        Walking = 1
    }

    public enum LevelDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public enum InputMode
    {
        Select,
        BreakObject,
        Disabled
    }
}
