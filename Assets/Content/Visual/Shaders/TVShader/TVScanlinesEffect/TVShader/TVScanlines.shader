Shader "Hidden/Custom/TVScanlines"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            Name "TVScanlines"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _Intensity;
            float _LineThickness;
            float _LineSpacing;

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord);

                float pixelY = floor(input.texcoord.y * _BlitTexture_TexelSize.w);
                float period = max(_LineThickness + _LineSpacing, 1.0);
                float positionInPeriod = fmod(pixelY, period);
                float lineMask = step(positionInPeriod + 0.5, _LineThickness);

                color.rgb *= 1.0 - lineMask * _Intensity;
                return color;
            }
            ENDHLSL
        }
    }

    Fallback Off
}
