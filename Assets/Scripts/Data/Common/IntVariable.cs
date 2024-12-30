using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/IntVariable")]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int value;
    [SerializeField] private bool isSave;
    [SerializeField] private string saveKey;

    public int Value
    {
        get
        {
            Debug.Log("CURRENT LEVEL GET " + saveKey + " / " + this.value);

            return value;
        }
        set
        {
            this.value = value;

            Debug.Log("CURRENT LEVEL SET " + saveKey + " / " + this.value);

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
