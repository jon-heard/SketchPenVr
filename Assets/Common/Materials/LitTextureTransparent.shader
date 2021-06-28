Shader "Custom/LitTextureTransparent" 
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
    _Opacity("Opacity", range(0.0, 1.0)) = 1.0
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
        #pragma vertex genericVertexShader_texturedLit
        #pragma fragment fragmentShader
        #pragma target 2.0

        sampler2D _MainTex;
        float _Opacity;

        fixed4 fragmentShader(VertexToFragment_texturedLit i) : SV_Target
        {
          float4 texColor = tex2D(_MainTex, i.texcoord);
          return float4(texColor.rgb * calcLight(i.normal), texColor.a * _Opacity);
        }
      ENDCG
    }
  }
}
