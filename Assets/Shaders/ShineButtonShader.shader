Shader "Unlit/ShineButtonShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _LineColor ("Line Color", Color) = (1, 1, 1, 1)
        _LineWidth ("Line Width", Float) = 0.05
        _LineSpeed ("Line Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _LineColor;
            float _LineWidth;
            float _LineSpeed;

            // Structs
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex function
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment function
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the base texture
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                
                // Multiply the base texture with the _BaseColor property
                baseColor *= _BaseColor;

                // Calculate looping time for the shine effect
                float time = frac(_Time.y * _LineSpeed);

                // Determine the position of the shine line
                float linePos = frac(i.uv.x + time);

                // Create the thin shine line effect
                float lineEffect = smoothstep(1.0 - _LineWidth, 1.0, linePos);

                // Apply the shine effect on top of the base texture
                fixed4 shineColor = _LineColor * lineEffect;

                // Combine the base texture with the shine effect
                return baseColor + shineColor * baseColor.a;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}
