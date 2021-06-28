Shader "Custom/UnlitTexture" 
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
  }
  SubShader
  {
    Tags { "RenderType" = "Opaque" }
    Pass
    {
      CGPROGRAM
        #include "_shaderCommon.txt"
        #pragma vertex genericVertexShader_textured
        #pragma fragment fragmentShader
        #pragma target 2.0

        sampler2D _MainTex;

        fixed4 fragmentShader(VertexToFragment_textured i) : SV_Target
        {
          return tex2D(_MainTex, i.texcoord);
        }
      ENDCG
    }
  }
}
