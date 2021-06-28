Shader "Custom/UnlitColorTransparent"
{
  Properties
  {
    _Color("Color", Color) = (1,1,1,1)
  }
  SubShader
  {
    Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Pass
    {
      CGPROGRAM
        #include "_shaderCommon.txt"

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
