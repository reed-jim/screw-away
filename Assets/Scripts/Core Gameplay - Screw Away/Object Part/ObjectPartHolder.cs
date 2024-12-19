using UnityEngine;

public class ObjectPartHolder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IObjectPart objectPart = other.GetComponent<IObjectPart>();

        if (objectPart != null)
        {
            other.gameObject.SetActive(false);
        }
    }
}
