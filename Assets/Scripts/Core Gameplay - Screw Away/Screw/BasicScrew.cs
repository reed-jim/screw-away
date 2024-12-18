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
    public static event Action screwLoosenedEvent;

    private void Update()
    {
        if (_isRotating)
        {
            transform.RotateAround(transform.forward, 0.3f);
        }
    }

    public override void Loose(int screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (screwId == this.screwId)
        {
            if (_isDone)
            {
                return;
            }

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

            Tween.Position(transform, transform.position + 3f * transform.forward, duration: 0.3f).OnComplete(() =>
            {
                _isRotating = false;

                Tween.Delay(duration: 0.1f)
                .OnComplete(() =>
                {
                    transform.SetParent(screwBoxSlot.transform);
                    gameObject.layer = LayerMask.NameToLayer("UI");

                    Tween.Rotation(transform, Quaternion.Euler(new Vector3(0, 180, 0)), duration: 0.3f);
                    Tween.Position(transform, screwBoxSlot.transform.position + new Vector3(0, 0, -0.3f), duration: 0.3f);
                    Tween.Scale(transform, scaleOnScrewBox, duration: 0.3f)
                    .OnComplete(() =>
                    {
                        screwBoxSlot.CompleteFill();

                        _isDone = true;

                        screwLoosenedEvent?.Invoke();
                    });
                });
            });

            SoundManager.Instance.PlaySoundLoosenScrew();

            screwBoxSlot.Fill(this);
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
