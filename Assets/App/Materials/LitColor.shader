Shader "Custom/LitColor"
{
  Properties
  {
    _Color("Color", Color) = (1,1,1,1)
  }
    SubShader
  {
    Tags { "RenderType" = "Opaque" }
    Pass
    {
      CGPROGRAM
        #include "App.cginc"
        #pragma vertex genericVertexShader_lit
        #pragma fragment fragmentShader
        #pragma target 2.0

        float4 _Color;

        fixed4 fragmentShader(VertexToFragment_lit i) : SV_Target
        {
          return float4(_Color.rgb * calcLight(i.normal), _Color.a);
        }
      ENDCG
    }
  }
}
