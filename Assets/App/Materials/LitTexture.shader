Shader "Custom/LitTexture" 
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
        #include "App.cginc"
        #pragma vertex genericVertexShader_texturedLit
        #pragma fragment fragmentShader
        #pragma target 2.0

        sampler2D _MainTex;

        fixed4 fragmentShader(VertexToFragment_texturedLit i) : SV_Target
        {
          float4 texColor = tex2D(_MainTex, i.texcoord);
          return float4(texColor.rgb * calcLight(i.normal), texColor.a);
        }
      ENDCG
    }
  }
}
