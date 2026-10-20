using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;

    private void Awake()
    {
        Destroy(gameObject, _destroyDelay);
    }
}
