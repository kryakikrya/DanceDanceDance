using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_6000_0_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
#endif

public sealed class TVScanlinesRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader _shader;

    private Material _material;
    private TVScanlinesRenderPass _renderPass;

    public override void Create()
    {
        if (_renderPass != null)
        {
            _renderPass.Dispose();
        }

        CoreUtils.Destroy(_material);
        _material = null;
        _renderPass = null;

        if (_shader == null)
        {
            _shader = Shader.Find("Hidden/Custom/TVScanlines");
        }

        if (_shader == null)
        {
            return;
        }

        _material = CoreUtils.CreateEngineMaterial(_shader);
        _renderPass = new TVScanlinesRenderPass(_material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_renderPass == null || renderingData.cameraData.cameraType != CameraType.Game)
        {
            return;
        }

        TVScanlinesVolume volume = VolumeManager.instance.stack?.GetComponent<TVScanlinesVolume>();
        if (volume == null || !volume.IsActive())
        {
            return;
        }

        _renderPass.Setup(volume.Intensity.value, volume.LineThickness.value, volume.LineSpacing.value);
        renderer.EnqueuePass(_renderPass);
    }

    protected override void Dispose(bool disposing)
    {
        if (_renderPass != null)
        {
            _renderPass.Dispose();
            _renderPass = null;
        }

        CoreUtils.Destroy(_material);
        _material = null;
    }

    private sealed class TVScanlinesRenderPass : ScriptableRenderPass
    {
        private static readonly int IntensityId = Shader.PropertyToID("_Intensity");
        private static readonly int LineThicknessId = Shader.PropertyToID("_LineThickness");
        private static readonly int LineSpacingId = Shader.PropertyToID("_LineSpacing");
        private const string PassName = "TV Scanlines";

        private readonly Material _material;
        private float _intensity;
        private float _lineThickness;
        private float _lineSpacing;

        private RTHandle _cameraColorTarget;
        private RTHandle _temporaryColorTarget;

        public TVScanlinesRenderPass(Material material)
        {
            _material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void Setup(float intensity, float lineThickness, float lineSpacing)
        {
            _intensity = intensity;
            _lineThickness = lineThickness;
            _lineSpacing = lineSpacing;
        }

#if UNITY_6000_0_OR_NEWER
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            TextureHandle source = resourceData.activeColorTexture;
            if (!source.IsValid())
            {
                return;
            }

            TextureDesc destinationDescription = renderGraph.GetTextureDesc(source);
            destinationDescription.name = PassName;
            destinationDescription.clearBuffer = false;
            destinationDescription.depthBufferBits = DepthBits.None;

            TextureHandle destination = renderGraph.CreateTexture(destinationDescription);

            SetMaterialProperties();
            RenderGraphUtils.BlitMaterialParameters parameters = new RenderGraphUtils.BlitMaterialParameters(source, destination, _material, 0);
            renderGraph.AddBlitPass(parameters, PassName);

            resourceData.cameraColor = destination;
        }
#endif
        public override void OnCameraCleanup(CommandBuffer commandBuffer)
        {
            _cameraColorTarget = null;
        }

        private void SetMaterialProperties()
        {
            _material.SetFloat(IntensityId, _intensity);
            _material.SetFloat(LineThicknessId, _lineThickness);
            _material.SetFloat(LineSpacingId, _lineSpacing);
        }

        public void Dispose()
        {
            _temporaryColorTarget?.Release();
            _temporaryColorTarget = null;
            _cameraColorTarget = null;
        }
    }
}
