using UnityEngine;
using static GameEnum;

public class ScrewBoxSlot : MonoBehaviour
{
    private bool _isFilled;

    public bool IsFilled
    {
        get => _isFilled;
        set => _isFilled = value;
    }
}
