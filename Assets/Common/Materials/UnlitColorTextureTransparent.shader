Shader "Custom/UnlitColorTextureTransparent"
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
    _Color("Tint", Color) = (1,1,1,1)
  }
  SubShader
  {
    Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Pass {
      CGPROGRAM
        #include "_shaderCommon.txt"

        #pragma vertex genericVertexShader_textured_tileoffset
        #pragma fragment fragmentShader
        #pragma target 2.0

        sampler2D _MainTex;
        float4 _Color;

        fixed4 fragmentShader(VertexToFragment_textured i) : SV_Target
        {
          return tex2D(_MainTex, i.texcoord) * _Color;
        }
      ENDCG
    }
  }
}
