using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComicScroller : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rendererPrefab;
    [SerializeField] private Dictionary<Sprite, Vector2> _pages;

    private List<KeyValuePair<Sprite, Vector2>> _orderedPages;
    private int _currentIndex = -1;

    private void Start()
    {
        _orderedPages = _pages.ToList();
        SpawnNextPage();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame == false)
        {
            return;
        }

        SpawnNextPage();
    }

    private void SpawnNextPage()
    {
        if (_currentIndex >= _orderedPages.Count - 1)
        {
            return;
        }

        KeyValuePair<Sprite, Vector2> page = _orderedPages[++_currentIndex];

        SpriteRenderer renderer = Instantiate(
            _rendererPrefab,
            page.Value,
            Quaternion.identity
        );

        renderer.sprite = page.Key;
    }
}