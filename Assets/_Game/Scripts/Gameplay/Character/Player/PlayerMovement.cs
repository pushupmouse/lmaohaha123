using MEC;
using Obvious.Soap;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3Variable _inputs;
    [SerializeField] private FloatReference _speed;
    [SerializeField] private TransformVariable _playerTransform;

    private void Awake()
    {
        _playerTransform.Value = transform;
    }

    private void Start()
    {
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }

    private void MyUpdate()
    {
        transform.position += _inputs.Value * (_speed * Time.deltaTime);
    }
}
