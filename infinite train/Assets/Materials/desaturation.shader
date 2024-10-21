Shader "Custom/DesaturationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DesaturationAmount ("Desaturation Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _DesaturationAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pobieramy kolor piksela z tekstury
                fixed4 color = tex2D(_MainTex, i.uv);

                // Przekształcamy kolor na odcienie szarości
                float gray = dot(color.rgb, float3(0.3, 0.59, 0.11));

                // Interpolujemy pomiędzy kolorem oryginalnym a szarym
                color.rgb = lerp(color.rgb, float3(gray, gray, gray), _DesaturationAmount);

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
