using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class BasicScrew : BaseScrew
{
    private float _distanceBlocked;

    #region EVENT
    public static event Action disableInputEvent;
    #endregion

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
            if (!_isInteractable)
            {
                return;
            }
            else
            {
                _isInteractable = false;
            }

            if (_isDone)
            {
                return;
            }

            CountBlockingObjects();

            // BLOCKED
            if (_numberBlockingObjects > 0)
            {
                FakeScrew fakeScrew = ObjectPoolingEverything.GetFromPool<FakeScrew>(GameConstants.FAKE_SCREW);

                fakeScrew.CloneFromScrew(this);

                fakeScrew.transform.SetParent(transform);

                GetComponent<MeshRenderer>().enabled = false;

                fakeScrew.IsRotating = true;

                SoundManager.Instance.PlaySoundLoosenScrewFail();

                _tweens.Add(Tween.LocalPosition(fakeScrew.transform, 0.6f * _distanceBlocked * (fakeScrew.transform.localRotation * Vector3.forward),
                    cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.5f)
                .OnComplete(() =>
                {
                    fakeScrew.IsRotating = false;

                    ObjectPoolingEverything.ReturnToPool(GameConstants.FAKE_SCREW, fakeScrew.gameObject);
                    // fakeScrew.gameObject.SetActive(false);

                    GetComponent<MeshRenderer>().enabled = true;

                    _isInteractable = true;
                }));

                return;
            }

            joint.breakForce = 0;

            InvokeBreakJointEvent();

            screwBoxSlot.Fill(this);

            _isRotating = true;

            // FOR ROTATING
            // await Task.Delay(100);

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
                        _isInScrewPort = screwBoxSlot.IsScrewPort;

                        InvokeScrewLoosenedEvent();
                    }));
                }));
            }));

            SoundManager.Instance.PlaySoundLoosenScrew();
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

    public override int CountBlockingObjects()
    {
        Vector3 start = transform.position + transform.forward;

        Collider[] hits = Physics.OverlapCapsule(start, start + 10 * transform.forward, 0.2f);

        int number = 0;

        float minBlockedDistance = float.MaxValue;

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
                    float distance = Vector3.Distance(hits[i].ClosestPoint(transform.position), transform.position);

                    if (distance < minBlockedDistance)
                    {
                        minBlockedDistance = distance;
                    }

                    number++;

                    // if (distance < 2)
                    // {
                    //     number++;
                    // }
                }
            }
        }

        _numberBlockingObjects = number;
        _distanceBlocked = minBlockedDistance;

        return number;
    }
}
