using System;
using UnityEngine;

public class BasicObjectPart : MonoBehaviour, IObjectPart
{
    public static event Action<int> selectObjectPartEvent;
    public static event Action<int> deselectObjectPartEvent;

    private bool _isSelecting;

    void Awake()
    {
        ScrewSelectionInput.mouseUpEvent += Deselect;
    }

    void OnDestroy()
    {
        ScrewSelectionInput.mouseUpEvent -= Deselect;
    }

    public void Select()
    {
        selectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

        _isSelecting = true;
    }

    private void Deselect()
    {
        if (_isSelecting)
        {
            deselectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

            _isSelecting = false;
        }
    }
}
