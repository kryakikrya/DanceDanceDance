using UnityEngine;

public class ResourceLoader
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>($"CMS/{path}");
    }
}