using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/IntVariable")]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int value;
    [SerializeField] private bool isSave;
    [SerializeField] private string saveKey;

    public int Value
    {
        get => value;
        set
        {
            this.value = value;

            if (isSave)
            {
                Save(saveKey);
            }
        }
    }

    public void Save(string key)
    {
        DataUtility.SaveAsync(key, value);
    }

    public void Load()
    {
        value = DataUtility.Load(saveKey, 1);
    }
}
