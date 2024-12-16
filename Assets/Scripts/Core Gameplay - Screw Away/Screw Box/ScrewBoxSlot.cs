using System;
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

    #region EVENT
    public static event Action<int> screwBoxCompleteEvent;
    #endregion

    public void Fill()
    {
        _isFilled = true;

        if (transform.GetSiblingIndex() == 2)
        {
            screwBoxCompleteEvent?.Invoke(transform.parent.gameObject.GetInstanceID());
        }
    }
}
