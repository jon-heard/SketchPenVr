Shader "Custom/UnlitTextureWithBlobShadow" 
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
    _ShadowTex("Shadow", 2D) = "white" {}
    _ShadowState("Shadow State", Vector) = (0.5, 0.5, .01, 1.0)
  }
  SubShader
  {
    Tags { "RenderType" = "Opaque" }
    Pass
    {
      CGPROGRAM
        #include "App.cginc"
        #pragma vertex genericVertexShader_textured
        #pragma fragment fragmentShader
        #pragma target 2.0

        sampler2D _MainTex;
        sampler2D _ShadowTex;
        float4 _ShadowState;

        fixed4 fragmentShader(VertexToFragment_textured i) : SV_Target
        {
          float4 mainColor = tex2D(_MainTex, i.texcoord);
          float4 shadowColor = calcShadowColor(mainColor, i.texcoord, _ShadowTex, _ShadowState);
          return alphaOver(mainColor, shadowColor);
        }
      ENDCG
    }
  }
}
