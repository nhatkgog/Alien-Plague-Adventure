Shader "Unlit/SpriteOutlineShader"
{
  Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineSize ("Outline Size", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.texcoord;
                float alpha = tex2D(_MainTex, uv).a;

                // Check 8 directions for alpha
                float outline = 0;
                float2 offset = float2(_OutlineSize / _ScreenParams.x, _OutlineSize / _ScreenParams.y);

                outline += tex2D(_MainTex, uv + float2(-offset.x, 0)).a;
                outline += tex2D(_MainTex, uv + float2(offset.x, 0)).a;
                outline += tex2D(_MainTex, uv + float2(0, -offset.y)).a;
                outline += tex2D(_MainTex, uv + float2(0, offset.y)).a;

                outline += tex2D(_MainTex, uv + float2(-offset.x, -offset.y)).a;
                outline += tex2D(_MainTex, uv + float2(-offset.x, offset.y)).a;
                outline += tex2D(_MainTex, uv + float2(offset.x, -offset.y)).a;
                outline += tex2D(_MainTex, uv + float2(offset.x, offset.y)).a;

                fixed4 col = tex2D(_MainTex, uv);
                if (col.a < 0.1 && outline > 0)
                    return _OutlineColor;

                return col;
            }
            ENDCG
        }
    }
}
