using PrimeTween;
using UnityEngine;

public class BasicScrew : BaseScrew
{
    [SerializeField] private HingeJoint joint;

    private bool _isRotating;

    public HingeJoint Joint
    {
        set => joint = value;
    }

    private void Update()
    {
        if (_isRotating)
        {
            transform.RotateAround(transform.forward, 0.5f);
        }
    }

    public override void Loose(string screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (screwId == this.screwId)
        {
            joint.breakForce = 0;

            _isRotating = true;

            Tween.Position(transform, transform.position + 3f * transform.forward, duration: 0.5f).OnComplete(() =>
            {
                _isRotating = false;

                Tween.Rotation(transform, Quaternion.Euler(new Vector3(-30, 180, 0)), duration: 0.5f);
                Tween.Position(transform, screwBoxSlot.transform.position + new Vector3(0, 0, -0.3f), duration: 0.5f);
                Tween.Scale(transform, 0.6f * _initialScale, duration: 0.5f)
                .OnComplete(() =>
                {
                    screwBoxSlot.Fill();
                });

                transform.SetParent(screwBoxSlot.transform);
            });
        }
    }
}
