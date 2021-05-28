Shader "Custom/Invisible" 
{
  Properties
  {
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

        fixed4 fragmentShader(VertexToFragment i) : SV_Target
        {
          clip(-1);
          return 0;
        }
      ENDCG
    }
  }
}
