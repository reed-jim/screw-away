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
    }

    public enum GameFaction
    {
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        All
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
}
