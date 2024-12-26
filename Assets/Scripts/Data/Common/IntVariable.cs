using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/IntVariable")]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int value;
    [SerializeField] private bool isSave;

    public int Value
    {
        get => value;
        set
        {
            this.value = value;

            if (isSave)
            {
                Save();
            }
        }
    }

    public void Save()
    {
        DataUtility.SaveAsync(GameConstants.CURRENT_LEVEL, value);
    }

    public void Load()
    {
        value = DataUtility.Load(GameConstants.CURRENT_LEVEL, value);
    }
}
