using MEC;
using Obvious.Soap;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private Vector3Variable _inputs;

    private void Start()
    {
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }

    private void MyUpdate()
    {
        _inputs.Value = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
}
