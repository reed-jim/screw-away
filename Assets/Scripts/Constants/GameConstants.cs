using UnityEngine;

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


    public static Color SAFERIO_RED = new Color(255f / 255, 80f / 255, 80f / 255, 1);
    public static Color SAFERIO_GREEN = new Color(90f / 255, 255f / 255, 90f / 255, 1);
    public static Color SAFERIO_ORANGE = new Color(255f / 255, 160f / 255, 0f / 255, 1);
    public static Color SAFERIO_PURPLE = new Color(200f / 255, 0f / 255, 255f / 255, 1);
    public static Color SAFERIO_BLUE = new Color(90f / 255, 90f / 255, 255f / 255, 1);
    #endregion

    #region OBJECT POOLING
    public static string TAG_SOUND = "Tag Sound";
    public static string SCREW_BOX = "Screw Box";
    public static string SCREW_PORT_SLOT = "Screw Port Slot";
    public static string FAKE_SCREW = "Fake Screw";
    #endregion

    #region SOUND
    public static string LOOSEN_SCREW_SOUND = "Loosen Screw Sound";
    public static string LOOSEN_SCREW_FAIL_SOUND = "Loosen Screw Fail Sound";
    public static string SCREW_BOX_DONE_SOUND = "Screw Box Done Sound";
    public static string WIN_SOUND = "Win Sound";
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
    public static string WEEKLY_TASKS = "Weekly Tasks";
    #endregion
}
