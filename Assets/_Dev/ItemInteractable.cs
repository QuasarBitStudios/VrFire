using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemInteractable : XRBaseInteractable
{
    public enum ItemType
    {
        FollowInteractorFreeMove,
        SimpleAction,
        FollowHandFixedAxis
    }

    [Header("Item Settings")]
    [SerializeField] private ItemType itemType;
    public string itemName;

    [Tooltip("Events triggered when the item is interacted with(only in itemType.SimpleAction")]
    public UnityAction<Transform> interactableAction;
    public UnityAction droppedAction;

    ///Private Variables
    private bool handTouching = false;
    private Transform interactorObj;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        handTouching = true;
        interactorObj = args.interactorObject.transform;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        handTouching = false;
        interactorObj = null;
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        
        if (handTouching)
            Execute();
        
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        Dropped();
        droppedAction?.Invoke();
    }

    private void Execute()
    {
        switch (itemType)
        {
            case ItemType.FollowInteractorFreeMove:
               if(interactorObj != null)
                    transform.SetParent(interactorObj);
                break;
            case ItemType.SimpleAction:
                interactableAction?.Invoke(interactorObj);
                break;
            case ItemType.FollowHandFixedAxis:
                break;
            default:
                break;
        }
    }

    private void Dropped()
    {
        switch (itemType)
        {
            case ItemType.FollowInteractorFreeMove:
                transform.SetParent(null);
                break;
            case ItemType.SimpleAction:
                break;
            case ItemType.FollowHandFixedAxis:
                break;
            default:
                break;
        }
    }
}
