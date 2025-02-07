using Obvious.Soap;
using UnityEngine;

public class EventPickup : Pickup
{
    [SerializeField] private ScriptableEventNoParam _onPickedUpEvent;

    protected override void OnTriggerEnter(Collider other)
    {
        _onPickedUpEvent.Raise();
        
        base.OnTriggerEnter(other);
    }
}
