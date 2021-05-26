Shader "Custom/UnlitColor"
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
        #pragma vertex genericVertexShader
        #pragma fragment fragmentShader
        #pragma target 2.0

        float4 _Color;

        fixed4 fragmentShader(VertexToFragment i) : SV_Target
        {
          return _Color;
        }
      ENDCG
    }
  }
}
