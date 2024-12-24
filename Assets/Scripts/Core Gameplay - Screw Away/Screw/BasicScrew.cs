using System;
using System.Threading.Tasks;
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
            // transform.Rotate(transform.forward * 300f * Time.deltaTime);
            transform.RotateAround(transform.forward, 0.3f);
        }
    }

    public override async void Loose(int screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
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

                _tweens.Add(Tween.Position(fakeScrew.transform, transform.position + 0.3f * transform.forward, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.5f)
                .OnComplete(() =>
                {
                    fakeScrew.IsRotating = false;

                    fakeScrew.gameObject.SetActive(false);

                    GetComponent<MeshRenderer>().enabled = true;
                }));

                return;
            }

            joint.breakForce = 0;

            breakJointEvent?.Invoke(joint.gameObject.GetInstanceID());

            _isRotating = true;

            await Task.Delay(100);

            _tweens.Add(Tween.Position(transform, transform.position + 3f * transform.forward, duration: 0.3f).OnComplete(() =>
            {
                _isRotating = false;

                _tweens.Add(Tween.Delay(duration: 0.1f)
                .OnComplete(() =>
                {
                    transform.SetParent(screwBoxSlot.transform);

                    // transform.position = ConvertPositionToAnotherCameraSpace();
                    // transform.localScale *= 12f / 14;
                    gameObject.layer = LayerMask.NameToLayer("UI");

                    float duration = 0.3f;

                    _tweens.Add(Tween.LocalRotation(transform, Quaternion.Euler(new Vector3(0, 180, 0)), duration: duration));
                    _tweens.Add(Tween.LocalPosition(transform, new Vector3(0, 0, -0.3f), duration: duration));
                    _tweens.Add(Tween.Scale(transform, scaleOnScrewBox * Vector3.one, duration: duration)
                    .OnComplete(() =>
                    {
                        screwBoxSlot.CompleteFill();

                        _isDone = true;

                        screwLoosenedEvent?.Invoke();
                    }));
                }));
            }));

            SoundManager.Instance.PlaySoundLoosenScrew();

            screwBoxSlot.Fill(this);
        }
    }

    private Vector3 ConvertPositionToAnotherCameraSpace()
    {
        Vector3 camera1Position = new Vector3(0, 6.5f, -10);
        Quaternion camera1Rotation = Quaternion.Euler(30, 0, 0);

        Vector3 camera2Position = new Vector3(0, 0, -10);
        Quaternion camera2Rotation = Quaternion.Euler(0, 0, 0);

        Vector3 worldPos = gameObject.transform.position;

        Vector3 localPosInCamera1 = camera1Position - worldPos;

        Vector3 localPosInCamera2 = camera2Position + localPosInCamera1;

        Vector3 compensatedPosition = transform.position;

        compensatedPosition.y = transform.position.y * Mathf.Sin(30 * Mathf.Deg2Rad) + (camera1Position.y - camera2Position.y);

        return compensatedPosition;
    }

    // private void OnDrawGizmos()
    // {
    //     // Define the start and end points of the capsule
    //     Vector3 start = transform.position;
    //     Vector3 end = transform.position + 10 * transform.forward;

    //     // Draw the capsule shape as a wireframe using Gizmos
    //     Gizmos.color = Color.green;

    //     // Draw capsule ends as spheres
    //     Gizmos.DrawWireSphere(start, 0.5f);   // Start point of the capsule
    //     Gizmos.DrawWireSphere(end, 0.5f);     // End point of the capsule

    //     // Draw a line connecting the start and end
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(start, end);  // Connect the two spheres with a line

    //     // Optionally, draw the capsule's direction as an arrow (helpful for visualizing direction)
    //     Gizmos.DrawRay(start, 10 * transform.forward);
    // }

    public override int CountBlockingObjects()
    {
        Vector3 start = transform.position + 0.3f * transform.forward;

        Collider[] hits = Physics.OverlapCapsule(start, start + 10 * transform.forward, 0.2f);

        int number = 0;

        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == transform.parent)
                {
                    continue;
                }

                if (hits[i].GetComponent<IObjectPart>() != null)
                {
                    if (Vector3.Distance(hits[i].ClosestPoint(transform.position), transform.position) < 2)
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
