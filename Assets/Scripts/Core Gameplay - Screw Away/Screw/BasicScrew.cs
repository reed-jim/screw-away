using System;
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

    public bool IsRotating
    {
        get => _isRotating;
        set => _isRotating = value;
    }

    public static event Action disableInputEvent;
    public static event Action<int> breakJointEvent;

    private void Update()
    {
        if (_isRotating)
        {
            transform.RotateAround(transform.forward, 0.3f);
        }
    }

    public override void Loose(string screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (screwId == this.screwId)
        {
            CountBlockingObjects();

            if (_numberBlockingObjects > 0)
            {
                FakeScrew fakeScrew = ObjectPoolingEverything.GetFromPool<FakeScrew>(GameConstants.FAKE_SCREW);

                fakeScrew.CloneFromScrew(this);

                GetComponent<MeshRenderer>().enabled = false;

                fakeScrew.IsRotating = true;

                SoundManager.Instance.PlaySoundLoosenScrewFail();

                Tween.Position(fakeScrew.transform, transform.position + 0.3f * transform.forward, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.5f)
                .OnComplete(() =>
                {
                    fakeScrew.IsRotating = false;

                    fakeScrew.gameObject.SetActive(false);

                    GetComponent<MeshRenderer>().enabled = true;
                });

                return;
            }

            joint.breakForce = 0;

            breakJointEvent?.Invoke(joint.gameObject.GetInstanceID());

            _isRotating = true;

            Tween.Position(transform, transform.position + 3f * transform.forward, duration: 0.5f).OnComplete(() =>
            {
                _isRotating = false;

                Tween.Position(transform, transform.position - 1f * transform.forward, duration: 0.5f)
                .OnComplete(() =>
                {
                    transform.SetParent(screwBoxSlot.transform);
                    gameObject.layer = LayerMask.NameToLayer("UI");

                    Tween.Rotation(transform, Quaternion.Euler(new Vector3(0, 180, 0)), duration: 0.5f);
                    Tween.Position(transform, screwBoxSlot.transform.position + new Vector3(0, 0, -0.3f), duration: 0.5f);
                    Tween.Scale(transform, 1.5f * _initialScale, duration: 0.5f)
                    .OnComplete(() =>
                    {
                        screwBoxSlot.Fill(this);

                        _isDone = true;
                    });
                });
            });

            SoundManager.Instance.PlaySoundLoosenScrew();
        }
    }

    public override int CountBlockingObjects()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 10);

        int number = 0;

        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<IObjectPart>() != null)
                {
                    if (Vector3.Distance(hits[i].point, transform.position) < 2)
                    {
                        number++;
                    }
                }
            }
        }

        _numberBlockingObjects = number;

        return number;
    }
}
