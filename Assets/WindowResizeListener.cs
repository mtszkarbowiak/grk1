using System;
using System.Collections;
using UnityEngine;

public class WindowResizeListener : MonoBehaviour
{
    public event Action<Vector2Int> OnWindowResized;

    private Vector2Int _previous;

    private void Start()
    {
        OnWindowResized?.Invoke(new Vector2Int(Screen.width,Screen.height));
    }

    private void Update()
    {
        var next = new Vector2Int(Screen.width,Screen.height);

        if (next == _previous) return;
        
        _previous = next;
            
        StopAllCoroutines();
        StartCoroutine(WaitAndAnnounceResize());
    }

    private IEnumerator WaitAndAnnounceResize()
    {
        yield return new WaitForSeconds(1f);
        
        OnWindowResized?.Invoke(_previous);
    }
}