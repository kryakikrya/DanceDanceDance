using UnityEngine;

public class CMS
{
    private ResourceLoader _loader;

    public CMS(ResourceLoader loader)
    {
        _loader = loader;
    }

    public T GetEntity<T>(string id, string additionalFolder = "", bool isNullValid = false) where T : Object
    {
        additionalFolder = additionalFolder == "" ? "" : additionalFolder + "/";
        
        T loaded = _loader.Load<T>(additionalFolder + id);

        if (loaded != null)
        {
            Debug.Log($"Entity was found ({additionalFolder + id})");
            return loaded;
        }
        else
        {
            if (isNullValid)
            {
                return null;
            }
            
            Debug.LogWarning($"Entity not found ({additionalFolder + id})");
            return null;
        }
    }
}