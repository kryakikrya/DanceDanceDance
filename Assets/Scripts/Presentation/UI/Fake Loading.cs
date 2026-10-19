using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private float _delay = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(PlayLoading());
    }

    public IEnumerator PlayLoading()
    {
        _image.gameObject.SetActive(true);
        _image.fillAmount = 1f;
        _image.fillOrigin = Random.Range(0, 4);

        yield return new WaitForSeconds(_delay);

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            _image.fillAmount = 1f - elapsed / _duration;
            yield return null;
        }

        _image.fillAmount = 0f;
        _image.gameObject.SetActive(false);
    }
}