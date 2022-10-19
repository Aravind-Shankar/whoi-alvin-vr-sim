Shader "AlvinVR/RotateRender"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ViewportRect ("Viewport rect: X,Y,W,H", Vector) = (0.0, 0.0, 1.0, 1.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _ViewportRect;

            bool isOutsideViewport(float2 uv)
            {
                return (
                    uv.x < _ViewportRect.x || uv.y < _ViewportRect.y ||
                    uv.x > (_ViewportRect.x+_ViewportRect.z) ||
                    uv.y > (_ViewportRect.y+_ViewportRect.w)
                );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 outputUV;
                outputUV.x = 1.0 - i.uv.y;
                outputUV.y = i.uv.x;

                fixed4 col = tex2D(_MainTex, outputUV);
                if (isOutsideViewport(outputUV))
                    col = fixed4(0.0, 0.0, 0.0, 1.0);
                return col;
            }
            ENDCG
        }
    }
}
