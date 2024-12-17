using System;
using UnityEngine;
using static GameEnum;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/Screw Away/LevelDifficultyConfiguration")]
public class LevelDifficultyConfiguration : ScriptableObject
{
    [SerializeField] private LevelPhase[] levelPhases;

    public LevelPhase[] LevelPhases
    {
        get => levelPhases;
    }
}

[Serializable]
public class LevelPhase
{
    [SerializeField] private float startProgress;
    [SerializeField] private float endProgress;
    [SerializeField] private LevelDifficulty levelDifficulty;

    public float StartProgress
    {
        get => startProgress;
    }

    public float EndProgress
    {
        get => endProgress;
    }

    public LevelDifficulty LevelDifficulty
    {
        get => levelDifficulty;
    }
}
