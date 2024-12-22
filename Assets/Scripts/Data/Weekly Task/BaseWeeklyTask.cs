using Unity.Mathematics;
using UnityEngine;

public abstract class BaseWeeklyTask : MonoBehaviour
{
    private float _requirementValue;
    private float _currentValue;

    public float RequirementValue
    {
        get => _requirementValue;
    }

    public float CurrentValue
    {
        get => _currentValue;
    }

    public float Progress
    {
        get => _currentValue / _requirementValue;
    }

    public abstract void Init();

    public void SetRequirement(float value)
    {
        _requirementValue = value;
    }

    public void MakeProgress(float addedValue)
    {
        _currentValue += addedValue;
    }

    public void Save()
    {
        DataUtility.Save(GameConstants.SAVE_FILE_NAME, "", this);
    }
}
