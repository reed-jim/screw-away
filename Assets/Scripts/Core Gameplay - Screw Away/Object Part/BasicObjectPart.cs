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

    private bool _isSelecting;
    private bool _isFree;
    private bool _isImmuneSwipeForce;

    void Awake()
    {
        ScrewSelectionInput.mouseUpEvent += Deselect;
        BasicScrew.breakJointEvent += OnJointBreakCallback;
        SwipeGesture.swipeGestureEvent += OnSwipe;

        partRigidbody = GetComponent<Rigidbody>();
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
            await Task.Delay(200);

            if (GetComponent<HingeJoint>() == null)
            {
                // partRigidbody.AddForce(150f * Vector3.forward);

                _isFree = true;
            }
        }
    }

    private async void OnSwipe(Vector2 direction)
    {
        if (_isFree && !_isImmuneSwipeForce)
        {
            partRigidbody.AddForce(-10f * direction);

            _isImmuneSwipeForce = true;

            await Task.Delay(200);

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
