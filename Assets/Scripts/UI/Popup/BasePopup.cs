using System;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class BasePopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button closeButton;

    [Header("CUSTOMIZE")]
    [SerializeField] private ScreenRoute route;
    [SerializeField] private bool isRouteActiveFromStart;

    private ISaferioUIAnimation _transitionAnimation;

    public static event Action popupShowEvent;
    public static event Action popupHideEvent;

    private void Awake()
    {
        SwitchRouteButton.switchRouteEvent += OnRouteSwitched;
        TopBar.showPopupEvent += OnRouteSwitched;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }

        RegisterMoreEvent();

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
        TopBar.showPopupEvent -= OnRouteSwitched;

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
        gameObject.SetActive(true);

        _transitionAnimation.Show();

        popupShowEvent?.Invoke();
    }

    protected void Hide()
    {
        _transitionAnimation.Hide();

        popupHideEvent?.Invoke();
    }
}
