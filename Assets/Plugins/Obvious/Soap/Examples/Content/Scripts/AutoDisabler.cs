using UnityEngine;

namespace Obvious.Soap.Example
{


    public class AutoDisabler : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
      
        public void OnEnable()
        {
            Invoke(nameof(Destroy),_duration);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}