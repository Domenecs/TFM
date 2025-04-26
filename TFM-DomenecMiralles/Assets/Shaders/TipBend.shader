Shader "Custom/BendingRod"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PullStrength ("Pull Strength", Range(0, 1)) = 0.0
        _MaxBend ("Max Bend Amount", Range(-0.1, 0.1)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _PullStrength;
            float _MaxBend;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;

                  float height = v.vertex.y;

                // Adjust this value based on where your rod tip begins
                float tipZone = saturate((height - 2) * 5); // soft falloff
                float bend = tipZone * tipZone * _PullStrength * _MaxBend;

                // Bend downward
                v.vertex.x += bend * 0.5;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}