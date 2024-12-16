using System;
using System.Collections;
using UnityEngine;
using static GameEnum;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform content;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private Vector2Variable canvasSize;

    [Header("CUSTOMIZE")]
    [SerializeField] private ScreenRoute route;
    [SerializeField] private bool isRouteActiveFromStart;

    #region PRIVATE FIELD
    private Vector3 _initialContainerPosititon;
    private ISaferioUIAnimation _transitionAnimation;
    #endregion

    #region ACTION
    public static event Action<float> moveSwipingScreenEvent;
    #endregion  

    private void Awake()
    {
        SwitchRouteButton.switchRouteEvent += OnRouteSwitched;
        BottomBarItem.switchRouteEvent += OnRouteSwitched;
        SwipingScreen.hideOutsideScreenEvent += HideOutsideScreen;

        RegisterMoreEvent();

        _initialContainerPosititon = container.localPosition;
        _transitionAnimation = GetComponent<ISaferioUIAnimation>();

        if (isRouteActiveFromStart)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        MoreActionInAwake();
    }

    private void OnDestroy()
    {
        SwitchRouteButton.switchRouteEvent -= OnRouteSwitched;
        BottomBarItem.switchRouteEvent -= OnRouteSwitched;
        SwipingScreen.hideOutsideScreenEvent -= HideOutsideScreen;

        UnregisterMoreEvent();
    }

    protected virtual void RegisterMoreEvent()
    {

    }

    protected virtual void UnregisterMoreEvent()
    {

    }

    protected virtual void MoreActionInAwake()
    {

    }

    private void OnRouteSwitched(ScreenRoute route)
    {
        if (route == this.route)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    protected void Show()
    {
        moveSwipingScreenEvent?.Invoke(_initialContainerPosititon.x);

        // _transitionAnimation.Show();
    }

    protected void Hide()
    {
        // _transitionAnimation.Hide();
    }

    private void HideOutsideScreen(float swipingScreenPositionX)
    {
        if (Mathf.Abs(swipingScreenPositionX + container.localPosition.x) > canvasSize.Value.x)
        {
            content.gameObject.SetActive(false);
        }
        else
        {
            content.gameObject.SetActive(true);
        }
    }
}
