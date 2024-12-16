using UnityEngine;

public class ScrewPort : MonoBehaviour
{
    [SerializeField] private BaseScrew screw;
    [SerializeField] private HingeJoint joint;

    void Awake()
    {
        BaseScrew.looseScrewEvent += OnScrewLoosed;
    }

    void OnDestroy()
    {
        BaseScrew.looseScrewEvent -= OnScrewLoosed;
    }

    private void OnScrewLoosed(string screwId)
    {
        if (screwId == screw.ScrewId)
        {
            joint.breakForce = 0;
        }
    }
}
