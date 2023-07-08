Shader "Unlit/AlphaToCoverage"
{
	// The _BaseMap variable is visible in the Material's Inspector, as a field
	 // called Base Map.
	Properties
	{
		_BaseMap("Base Map", 2D) = "white"{}
		_AlphaCutoff("Cutoff", Range(0.15,0.85)) = 0.4
	}

		SubShader
		{
			Tags {"RenderQueue" = "AlphaTest" "RenderType" = "TransparentCutout" "RenderPipeline" = "UniversalRenderPipeline" }
			Cull Off
			Pass
			{
				AlphaToMask On
				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

				struct Attributes
				{
					float4 positionOS   : POSITION;
					// The uv variable contains the UV coordinate on the texture for the
					// given vertex.
					float2 uv           : TEXCOORD0;
					half3 normal		: NORMAL;
				};

				struct Varyings
				{
					float4 positionHCS  : SV_POSITION;
					// The uv variable contains the UV coordinate on the texture for the
					// given vertex.
					float2 uv           : TEXCOORD0;
					half3 worldNormal   : NORMAL;
				};

				// This macro declares _BaseMap as a Texture2D object.
				TEXTURE2D(_BaseMap);
				// This macro declares the sampler for the _BaseMap texture.
				SAMPLER(sampler_BaseMap);

				CBUFFER_START(UnityPerMaterial)
					// The following line declares the _BaseMap_ST variable, so that you
					// can use the _BaseMap variable in the fragment shader. The _ST 
					// suffix is necessary for the tiling and offset function to work.
					float4 _BaseMap_ST;
					float _AlphaCutoff;

				CBUFFER_END

				float CalcMipLevel(float2 texture_coord)
				{
					float2 dx = ddx(texture_coord);
					float2 dy = ddy(texture_coord);
					float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));

					return max(0.0, 0.5 * log2(delta_max_sqr));
				}

				Varyings vert(Attributes IN)
				{
					Varyings OUT;
					OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
					// The TRANSFORM_TEX macro performs the tiling and offset
					// transformation.
					OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
					// Pega a normal em World Space
					OUT.worldNormal = TransformObjectToWorldNormal(IN.normal);
					return OUT;
				}

				half4 frag(Varyings i) : SV_Target
				{
					// The SAMPLE_TEXTURE2D marco samples the texture with the given
					// sampler.
					half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
					// rescale alpha by mip level (if not using preserved coverage mip maps)
					//col.a *= 1 + max(0, CalcMipLevel(i.uv * _MainTex_TexelSize.zw)) * _MipScale;
					// rescale alpha by partial derivative
					col.a = (col.a - _AlphaCutoff) / max(fwidth(col.a), 0.0001) + 0.5;
					///clip(col.a - 0.5);

					//half3 worldNormal = normalize(i.worldNormal * facing);

					//fixed ndotl = saturate(dot(worldNormal, normalize(_WorldSpaceLightPos0.xyz)));
					//fixed3 lighting = ndotl * _LightColor0;
					//lighting += ShadeSH9(half4(worldNormal, 1.0));

					//col.rgb *= lighting;

					return col;
				}
				ENDHLSL
			}
		}
}
