using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class BasicObjectPart : MonoBehaviour, IObjectPart
{
    public static event Action<int> selectObjectPartEvent;
    public static event Action<int> deselectObjectPartEvent;
    public static event Action<BaseScrew> loosenScrewOnObjectBrokenEvent;

    [SerializeField] private Rigidbody partRigidbody;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 throwForceMultiplier = new Vector3(133, 66, 133);

    private bool _isSelecting;
    private bool _isFree;
    private bool _isImmuneSwipeForce;
    [SerializeField] private int _totalJoint;

    void Awake()
    {
        ScrewSelectionInput.mouseUpEvent += Deselect;
        BasicScrew.breakJointEvent += OnJointBreakCallback;
        SwipeGesture.swipeGestureEvent += OnSwipe;

        partRigidbody = GetComponent<Rigidbody>();

        _totalJoint = GetComponents<HingeJoint>().Length;
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

                Vector3 direction = transform.position - transform.parent.position;

                partRigidbody.AddForce(TransformUtil.ComponentWiseMultiply(throwForceMultiplier, direction));
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

        gameObject.SetActive(false);
    }
}
