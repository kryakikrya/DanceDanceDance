using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombinationVisualization : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private int _minCombo;
    [SerializeField] private float _showDuration = 0.25f;
    [SerializeField] private float _hideDuration = 0.3f;

    private Sequence _animation;

    public void CheckCombo(int combo)
    {
        if (combo <= _minCombo)
        {
            return;
        }

        StringBuilder text = new StringBuilder();

        foreach (char digit in combo.ToString())
        {
            text.Append($"<sprite name=\"{digit}\">");
        }

        _comboText.text = text.ToString();

        _animation?.Kill();

        _spriteRenderer.transform.localScale = Vector3.zero;
        _spriteRenderer.transform.localRotation = Quaternion.identity;

        Color color = _spriteRenderer.color;
        color.a = 0;
        _spriteRenderer.color = color;

        _comboText.alpha = 0;

        _animation = DOTween.Sequence()
            .Append(_spriteRenderer.transform.DOScale(Vector3.one, _showDuration))
            .Join(_spriteRenderer.DOFade(1f, _showDuration * 0.3f))
            .Join(_comboText.DOFade(1f, _showDuration * 0.3f))
            .Join(_spriteRenderer.transform.DOLocalRotate(
                new Vector3(0, 0, 15f),
                _showDuration + _hideDuration))
            .Append(_spriteRenderer.transform.DOScale(Vector3.zero, _hideDuration))
            .Join(_spriteRenderer.DOFade(0f, _hideDuration))
            .Join(_comboText.DOFade(0f, _hideDuration));
    }

    private void OnDestroy()
    {
        _animation?.Kill();
    }
}