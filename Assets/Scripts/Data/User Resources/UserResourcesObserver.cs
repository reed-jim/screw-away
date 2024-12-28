using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/UserResourcesObserver")]
public class UserResourcesObserver : ScriptableObject
{
    [SerializeField] private float coinValue;

    public float CoinValue
    {
        get => coinValue;
        set
        {
            coinValue = value;

            Save();
        }
    }

    public void Save()
    {
        DataUtility.SaveAsync(GameConstants.CURRENT_LEVEL, coinValue);
    }

    public void Load()
    {
        coinValue = DataUtility.Load(GameConstants.CURRENT_LEVEL, coinValue);
    }
}
