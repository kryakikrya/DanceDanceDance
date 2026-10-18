using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Custom/TV Scanlines")]
public sealed class TVScanlinesVolume : VolumeComponent, IPostProcessComponent
{
    [Tooltip("Затемнение полос: 0 отключает эффект, 1 делает полосы чёрными.")]
    public ClampedFloatParameter Intensity = new ClampedFloatParameter(0.35f, 0f, 1f);

    [Tooltip("Толщина каждой горизонтальной полосы в пикселях экрана.")]
    public ClampedIntParameter LineThickness = new ClampedIntParameter(1, 1, 12);

    [Tooltip("Пустое расстояние между полосами в пикселях экрана.")]
    public ClampedIntParameter LineSpacing = new ClampedIntParameter(2, 0, 32);

    public bool IsActive()
    {
        return active && Intensity.value > 0f;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
