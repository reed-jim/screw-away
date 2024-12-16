using UnityEngine;

public class HingeJointBinder : MonoBehaviour
{
    [SerializeField] private HingeJoint joint;
    [SerializeField] private Transform target;
    [SerializeField] private Transform screw;

    private void Awake()
    {
        BindHingeJointAnchor();
    }

    private void BindHingeJointAnchor()
    {
        Vector3 bindPosition = screw.position;

        bindPosition.x /= target.localScale.x;
        bindPosition.y /= target.localScale.y;
        bindPosition.z /= target.localScale.z;

        joint.anchor = bindPosition;
    }
}
