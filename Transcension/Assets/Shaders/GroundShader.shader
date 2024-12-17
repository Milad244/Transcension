Shader "Unlit/GroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TileFactor ("Tiling Factor", Vector) = (1, 1, 0, 0) // Controls texture repeat
        _TileOffset ("Tile Offset", Vector) = (0, 0, 0, 0)  // Select a specific tile
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TileFactor; // Tiling control
            float4 _TileOffset; // Offset control

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Scale UVs by the tiling factor and offset to select a tile
                o.uv = v.uv * _TileFactor.xy + _TileOffset.xy;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
