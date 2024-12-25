using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class BasicObjectPart : MonoBehaviour, IObjectPart
{
    [SerializeField] private Transform levelCenter;
    [SerializeField] private Rigidbody partRigidbody;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 throwForceMultiplier = new Vector3(0.003f, 0.001f, 0.003f);

    private bool _isSelecting;
    private bool _isFree;
    private bool _isImmuneSwipeForce;
    [SerializeField] private int _totalJoint;

    public static event Action<int> selectObjectPartEvent;
    public static event Action<int> deselectObjectPartEvent;
    public static event Action<BaseScrew> loosenScrewOnObjectBrokenEvent;

    void Awake()
    {
        ScrewSelectionInput.mouseUpEvent += Deselect;
        BasicScrew.breakJointEvent += OnJointBreakCallback;
        SwipeGesture.swipeGestureEvent += OnSwipe;

        partRigidbody = GetComponent<Rigidbody>();

        _totalJoint = GetComponents<HingeJoint>().Length;

        throwForceMultiplier = new Vector3(66f, 9f, 66f);
    }

    void OnDestroy()
    {
        ScrewSelectionInput.mouseUpEvent -= Deselect;
        BasicScrew.breakJointEvent -= OnJointBreakCallback;
        SwipeGesture.swipeGestureEvent -= OnSwipe;
    }

    public void Select()
    {
        selectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

        _isSelecting = true;
    }

    private void Deselect()
    {
        if (_isSelecting)
        {
            deselectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

            _isSelecting = false;
        }
    }

    private async void OnJointBreakCallback(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            await Task.Delay(500);

            _totalJoint--;

            if (_totalJoint == 0)
            {
                _isFree = true;

                Throw();
            }
        }
    }

    private async void OnSwipe(Vector2 direction)
    {
        return;

        if (_totalJoint <= 1 && !_isImmuneSwipeForce)
        {
            // partRigidbody.AddForce(-30f * direction);

            if (partRigidbody.linearVelocity.magnitude > 5)
            {
                return;
            }

            // OUT OF CENTER
            if (_totalJoint == 0)
            {
                Vector3 forceDirection = direction;

                forceDirection.y = 0;

                partRigidbody.AddForce(-20f * forceDirection);
            }
            else
            {
                partRigidbody.AddForce(20f * (Vector3.zero - transform.localPosition));
            }

            _isImmuneSwipeForce = true;

            // await Task.Delay(1000);

            _isImmuneSwipeForce = false;
        }
    }

    public void Break()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseScrew screw = transform.GetChild(i).GetComponent<BaseScrew>();

            if (screw.IsValidToLoose())
            {
                loosenScrewOnObjectBrokenEvent?.Invoke(screw);
            }
            else
            {
                screw.ForceUnscrew();
            }
        }

        AudioSource breakObjectSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.BREAK_OBJECT_SOUND);

        breakObjectSound.Play();

        Throw(forceBoost: 2);
    }

    private void Throw(float forceBoost = 1)
    {
        Vector3 direction;

        if (levelCenter != null)
        {
            direction = transform.position - levelCenter.position;
        }
        else
        {
            direction = transform.position - transform.parent.position;
        }

        direction = direction.normalized;

        if (Mathf.Abs(direction.x) < 0.01f)
        {
            direction.x = 1f;
        }
        if (Mathf.Abs(direction.y) < 0.01f)
        {
            direction.y = 1f;
        }
        if (Mathf.Abs(direction.z) < 0.01f)
        {
            direction.z = 1f;
        }

        partRigidbody.AddForce(TransformUtil.ComponentWiseMultiply(forceBoost * throwForceMultiplier, direction), ForceMode.Impulse);
    }
}
