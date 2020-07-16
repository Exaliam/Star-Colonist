Shader "Custom/HexCellData"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

        sampler2D _HexCellData;
		float4 _HexCellData_TexelSize;
		
		float4 GetCellData(appdata_full v, int index)
		{
			float2 uv;
			uv.x = (v.texcoord2[index] + 0.5) * _HexCellData_TexelSixe.x;
			float row = floor(uv.x);
			uv.x -= row;
			uv.y = (row + 0.5) * _HexCellData_TexelSize.y;
			float4 data = tex2Dlod(_HexCellData, float4(uv, 0, 0));
			data.w *= 255;
			return data;
		}

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
