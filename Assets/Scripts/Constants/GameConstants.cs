using UnityEngine;
using static GameEnum;

public static class GameConstants
{
    public static string MENU_SCENE = "Menu";
    public static string GAMEPLAY_SCENE = "Gameplay";

    #region SCREEN ROUTE
    public static string LOBBY_DETAIL_ROUTE = "Lobby Detail";
    #endregion

    #region POOLING
    public static string BOOSTER = "Booster";
    public static string VEHICLE_ENGINE_SOUND = "Vehicle Engine Sound";
    public static string HIT_OBSTACLE_SOUND = "Hit Obstacle Sound";
    public static string GET_IN_VEHICLE_SOUND = "Get In Vehicle Sound";
    public static string VEHICLE_MOVE_OUT_SOUND = "Vehicle Move Out Sound";
    #endregion

    #region COMMON TEXT
    public static string START_GAME = "Start Game";
    public static string CONNECTED = "Connected!";
    public static string DISCONNECTED = "Disconnected!";
    #endregion

    #region COLOR
    public static Color PRIMARY_BACKGROUND = new Color(30f / 255, 60f / 255, 50f / 255, 1);
    public static Color PRIMARY_TEXT = new Color(130f / 255, 255f / 255, 130f / 255, 1);
    public static Color ERROR_BACKGROUND = new Color(90f / 255, 40f / 255, 40f / 255, 1);
    public static Color ERROR_TEXT = new Color(255f / 255, 140f / 255, 140f / 255, 1);


    public static Color SAFERIO_RED = new Color(216f / 255, 67f / 255, 68f / 255, 1);
    public static Color SAFERIO_GREEN = new Color(174f / 255, 213f / 255, 86f / 255, 1);
    public static Color SAFERIO_ORANGE = new Color(214f / 255, 122f / 255, 37f / 255, 1);
    public static Color SAFERIO_PURPLE = new Color(211f / 255, 32f / 255, 254f / 255, 1);
    public static Color SAFERIO_BLUE = new Color(91f / 255, 157f / 255, 254f / 255, 1);
    public static Color SAFERIO_YELLLOW = new Color(248f / 255, 243f / 255, 59f / 255, 1);
    public static Color SAFERIO_LIGHT_BLUE = new Color(41f / 255, 185f / 255, 210f / 255, 1);
    public static Color SAFERIO_PINK = new Color(196f / 255, 110f / 255, 190f / 255, 1);
    #endregion

    #region OBJECT POOLING
    public static string TAG_SOUND = "Tag Sound";
    public static string SCREW_BOX = "Screw Box";
    public static string SCREW_PORT_SLOT = "Screw Port Slot";
    public static string FAKE_SCREW = "Fake Screw";
    public static string HAMMER = "Hammer";
    #endregion

    #region SOUND
    public static string LOOSEN_SCREW_SOUND = "Loosen Screw Sound";
    public static string LOOSEN_SCREW_FAIL_SOUND = "Loosen Screw Fail Sound";
    public static string SCREW_BOX_DONE_SOUND = "Screw Box Done Sound";
    public static string BREAK_OBJECT_SOUND = "Break Object Sound";
    public static string WIN_SOUND = "Win Sound";
    public static string LOSE_SOUND = "Lose Sound";
    #endregion

    #region ANIMATION
    public static string ANIMATION_STATE = "AnimationState";
    #endregion

    #region LANGUAGE
    public static string[] AvailableLanguages = { "English", "French", "German", "Italian", "Japanese" };
    #endregion

    #region TASK
    public static string TASK_ITEM_UI = "Task Item UI";
    public static string TASK_DESCRIPTION_PARAMETER = "task_requirement_value";
    public static string UNSCREW_TASK_TRANSLATION_NAME = "Unscrew Task Description";
    public static string COMPLETE_LEVEL_TRANSLATION_NAME = "Complete Level Task Description";
    public static string LEVEL_COMPLETED = "Level Completed";
    public static string LEVEL_PARAMETER = "level";
    #endregion

    #region IAP
    public static string REMOVE_AD_ID = "remove_ad";
    #endregion

    #region SAVE/LOAD
    public static string SAVE_FILE_NAME = "Game Local Data";
    public static string CURRENT_LEVEL = "Current Level";
    public static string WEEKLY_TASKS = "Weekly Tasks";
    public static string SCREWS_DATA = "Screws Data";
    #endregion

    #region SCREW AWAY
    public static int DEFAULT_NUMBER_SCREW_PORT = 5;
    public static GameFaction[] SCREW_FACTION = new GameFaction[8]
        {   GameFaction.Red, GameFaction.Green, GameFaction.Orange, GameFaction.Purple,
            GameFaction.Blue, GameFaction.Yellow, GameFaction.LightBlue,GameFaction.Pink };
    #endregion

    #region SWIPE
    public static int TYPICAL_FRAME_MILISECOND = 13;
    #endregion
}
