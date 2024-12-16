using System;
using PrimeTween;
using UnityEngine;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] private string screwId;

    public string ScrewId
    {
        get => screwId;
        set => screwId = value;
    }

    #region EVENT
    public static event Action<string> looseScrewEvent;

    public void Loose()
    {
        looseScrewEvent?.Invoke(screwId);

        gameObject.SetActive(false);
    }
    #endregion
}
