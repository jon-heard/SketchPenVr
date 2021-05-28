#ifndef SKETCHPAD_CG_INCLUDED
#define SKETCHPAD_CG_INCLUDED

#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
#include "UnityCG.cginc"

//#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
//
//inline float3 UnityObjectToWorldNormal(in float3 norm)
//{
//#ifdef UNITY_ASSUME_UNIFORM_SCALING
//  return UnityObjectToWorldDir(norm);
//#else
//  return normalize(mul(norm, (float3x3)unity_WorldToObject));
//#endif
//}

struct AppToVertex
{
  float4 vertex : POSITION;
  UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VertexToFragment
{
  float4 vertex : SV_POSITION;
  UNITY_VERTEX_OUTPUT_STEREO
};
VertexToFragment genericVertexShader(AppToVertex v)
{
  VertexToFragment o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_OUTPUT(VertexToFragment, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);
  return o;
}

struct AppToVertex_lit
{
  float4 vertex : POSITION;
  float3 normal : NORMAL;
  UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VertexToFragment_lit
{
  float4 vertex : SV_POSITION;
  float3 normal : TEXCOORD1;
  UNITY_VERTEX_OUTPUT_STEREO
};
VertexToFragment_lit genericVertexShader_lit(AppToVertex_lit v)
{
  VertexToFragment_lit o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_OUTPUT(VertexToFragment_lit, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.normal = UnityObjectToWorldNormal(v.normal);
  return o;
}

struct AppToVertex_textured 
{
  float4 vertex : POSITION;
  float2 texcoord : TEXCOORD0;
  UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VertexToFragment_textured
{
  float4 vertex : SV_POSITION;
  float2 texcoord : TEXCOORD0;
  UNITY_VERTEX_OUTPUT_STEREO
};
VertexToFragment_textured genericVertexShader_textured(AppToVertex_textured v)
{
  VertexToFragment_textured o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_OUTPUT(VertexToFragment_textured, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.texcoord = v.texcoord;
  return o;
}
float4 _MainTex_ST;
VertexToFragment_textured genericVertexShader_textured_tileoffset(AppToVertex_textured v)
{
  VertexToFragment_textured o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_OUTPUT(VertexToFragment_textured, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
  return o;
}

struct AppToVertex_texturedLit
{
  float4 vertex : POSITION;
  float2 texcoord : TEXCOORD0;
  float3 normal : NORMAL;
  UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VertexToFragment_texturedLit
{
  float4 vertex : SV_POSITION;
  float2 texcoord : TEXCOORD0;
  float3 normal : TEXCOORD1;
  UNITY_VERTEX_OUTPUT_STEREO
};
VertexToFragment_texturedLit genericVertexShader_texturedLit(AppToVertex_texturedLit v)
{
  VertexToFragment_texturedLit o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_INITIALIZE_OUTPUT(VertexToFragment_texturedLit, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.texcoord = v.texcoord;
  o.normal = UnityObjectToWorldNormal(v.normal);
  return o;
}

inline float calcLight(float3 normal)
{
  return min(1.0, max(0.0, dot(normal, normalize(float3(1, 4, -2)))) + 0.5);
}

inline float4 alphaOver(float4 baseColor, float4 overColor)
{
  if (overColor.a == 0)
  {
    return baseColor;
  }
  else if (overColor.a == 1)
  {
    return overColor;
  }
  else
  {
    float aAlphaComponent = baseColor.a * (1.0 - overColor.a);
    return float4(
      overColor.rgb * overColor.a + baseColor.rgb * aAlphaComponent, overColor.a + aAlphaComponent);
  }
}

inline float4 calcShadowColor(float4 baseColor, float2 texcoord, sampler2D shadowTex, float4 shadowState)
{
  float reverseBaseColor = ((baseColor.r + baseColor.g + baseColor.b) / 3.0 < 0.5) ? 1.0 : 0.0;
  float2 shadowUv = (texcoord - shadowState.xy) / shadowState.z * float2(1.0, 0.5625) + float2(0.5, 0.5);
  return float4(
    reverseBaseColor, reverseBaseColor, reverseBaseColor,
    max(0.0, tex2D(shadowTex, shadowUv).r * shadowState.w));
}

#endif // UNITY_CG_INCLUDED
