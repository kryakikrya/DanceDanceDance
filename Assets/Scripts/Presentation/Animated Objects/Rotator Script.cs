using System.Collections;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    [SerializeField] private float _angle = 3f;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _stepInterval = 0.1f;
    [SerializeField] private bool _rotateLeftFirst = true;

    private Quaternion _initialRotation;
    private Coroutine _rotationCoroutine;

    private void Start()
    {
        _initialRotation = transform.localRotation;
        _rotationCoroutine = StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        float direction = _rotateLeftFirst ? -1f : 1f;
        float currentAngle = 0f;
        float stepAngle = _angle * _stepInterval / _duration;

        while (true)
        {
            yield return new WaitForSeconds(_stepInterval);

            currentAngle += direction * stepAngle;

            if (currentAngle <= -_angle || currentAngle >= _angle)
            {
                currentAngle = Mathf.Clamp(currentAngle, -_angle, _angle);
                direction *= -1f;
            }

            transform.localRotation = _initialRotation * Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    private void OnDestroy()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
        }
    }
}