Shader "Custom/LitColorTextureTransparent"
{
  Properties
  {
      _MainTex("Albedo (RGB)", 2D) = "white" {}
      _Color ("Color", Color) = (1,1,1,1)
  }
  SubShader
  {
    Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
    LOD 200
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    CGPROGRAM
      #pragma surface surf Standard
      #pragma target 3.0
      fixed4 _Color;
      sampler2D _MainTex;
      struct Input
      {
          float2 uv_MainTex;
      };
      void surf (Input IN, inout SurfaceOutputStandard o)
      {
          fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
          o.Albedo = c.rgb;
          o.Alpha = c.a;
      }
    ENDCG
  }
  FallBack "Diffuse"
}
