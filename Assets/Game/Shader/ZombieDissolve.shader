Shader "Custom/ZombieDissolve"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0.0
        _EdgeColor ("Edge Color", Color) = (1,0.5,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _DissolveTex;
        float _DissolveAmount;
        fixed4 _EdgeColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DissolveTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float noise = tex2D(_DissolveTex, IN.uv_DissolveTex).r;

            if (noise < _DissolveAmount)
                discard;

            float edge = smoothstep(_DissolveAmount - 0.05, _DissolveAmount, noise);
            c.rgb = lerp(_EdgeColor.rgb, c.rgb, edge);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
