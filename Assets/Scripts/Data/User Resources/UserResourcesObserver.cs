using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/UserResourcesObserver")]
public class UserResourcesObserver : ScriptableObject
{
    [SerializeField] private float coinValue;

    public float CoinValue
    {
        get => coinValue;
        set => coinValue = value;
    }
}
