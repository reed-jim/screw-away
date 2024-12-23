using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameEnum;

public class ScrewSelectionInput : MonoBehaviour
{
    [SerializeField] private LayerMask layerMaskCheck;

    [SerializeField] private InputMode _inputMode;

    #region EVENT
    public static event Action mouseUpEvent;
    #endregion

    void Awake()
    {
        SwipeGesture.swipeGestureEvent += DisableInput;
        SwipeGesture.stopSwipeGestureEvent += EnableInput;
        BoosterUI.enableBreakObjectModeEvent += EnableBreakObjectMode;
    }

    void OnDestroy()
    {
        SwipeGesture.swipeGestureEvent -= DisableInput;
        SwipeGesture.stopSwipeGestureEvent -= EnableInput;
        BoosterUI.enableBreakObjectModeEvent -= EnableBreakObjectMode;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpEvent?.Invoke();
        }
    }

    private void Select()
    {
        if (_inputMode == InputMode.Disabled)
        {
            return;
        }

        if (IsClickedOnUI())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, layerMaskCheck);

        if (hit.collider != null)
        {
            IScrew screw = hit.collider.GetComponent<IScrew>();

            if (screw != null)
            {
                screw.Select();

                return;
            }

            IObjectPart objectPart = hit.collider.GetComponent<IObjectPart>();

            if (objectPart != null)
            {
                if (_inputMode == InputMode.BreakObject)
                {
                    objectPart.Break();

                    _inputMode = InputMode.Select;
                }
                else
                {
                    objectPart.Select();
                }
            }
        }
    }

    private void EnableInput()
    {
        _inputMode = InputMode.Select;
    }

    private void DisableInput(Vector2 swipeDirection)
    {
        _inputMode = InputMode.Disabled;
    }

    private void EnableBreakObjectMode()
    {
        _inputMode = InputMode.BreakObject;
    }

    private bool IsClickedOnUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        RaycastResult raycastResult = new RaycastResult();

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
