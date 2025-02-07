using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public event Action OnPickedUp;
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
