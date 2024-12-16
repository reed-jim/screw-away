using UnityEngine;

public class ScrewSelectionInput : MonoBehaviour
{
    [SerializeField] private LayerMask layerMaskCheck;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
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
                screw.Loose();
            }
        }
    }
}
