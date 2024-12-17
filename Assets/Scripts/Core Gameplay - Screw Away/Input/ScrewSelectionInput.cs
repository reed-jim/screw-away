using System;
using UnityEngine;

public class ScrewSelectionInput : MonoBehaviour
{
    [SerializeField] private LayerMask layerMaskCheck;

    #region EVENT
    public static event Action mouseUpEvent;
    #endregion

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
                objectPart.Select();
            }
        }
    }
}
