using System;
using System.Threading.Tasks;
using UnityEngine;

public class BasicObjectPart : MonoBehaviour, IObjectPart
{
    public static event Action<int> selectObjectPartEvent;
    public static event Action<int> deselectObjectPartEvent;

    [SerializeField] private Rigidbody partRigidbody;

    private bool _isSelecting;
    private bool _isFree;

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

    private void OnSwipe(Vector2 direction)
    {
        if (_isFree)
        {
            partRigidbody.AddForce(-20f * direction);
        }
    }
}
