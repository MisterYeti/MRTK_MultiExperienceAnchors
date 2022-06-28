// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_StandardOpaque"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (0.4716981,0.05562477,0.05562477,0)
		_FresnelColor("FresnelColor", Color) = (0.240566,0.3731039,0.9622642,0)
		_FresnalScale("FresnalScale", Range( 0 , 10)) = 1
		_FresnelPower("FresnelPower", Range( 0 , 10)) = 1
		[HDR]_emisColor("emisColor", Color) = (1,1,1,0)
		_emissive("emissive", Range( 0 , 1)) = 0
		_Smoothness1("Smoothness", Range( 0 , 1)) = 0
		_Metallic1("Metallic", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _FresnalScale;
		uniform float _FresnelPower;
		uniform float4 _FresnelColor;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _BaseColor;
		uniform float4 _emisColor;
		uniform float _emissive;
		uniform float _Metallic1;
		uniform float _Smoothness1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV4 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4 = ( 0.0 + _FresnalScale * pow( 1.0 - fresnelNdotV4, ( _FresnelPower * 2.0 ) ) );
			float4 Fresnel13 = ( fresnelNode4 * _FresnelColor );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 Albedo17 = ( Fresnel13 + ( tex2D( _Albedo, uv_Albedo ) * _BaseColor ) );
			o.Albedo = Albedo17.rgb;
			float4 Emission33 = ( ( Albedo17 * _emisColor ) * _emissive );
			o.Emission = Emission33.rgb;
			o.Metallic = _Metallic1;
			o.Smoothness = _Smoothness1;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
-1920;6;1920;1029;1241.423;167.279;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;11;-2207.97,321.5785;Inherit;False;1482.869;517.6389;Fresnel;8;6;7;8;10;9;5;4;13;Fresnel;1,0.4661931,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1851.901,575.4175;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2157.97,541.3332;Inherit;False;Property;_FresnelPower;FresnelPower;4;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1938.669,434.5718;Inherit;False;Property;_FresnalScale;FresnalScale;3;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1689.9,547.4175;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;4;-1424.521,371.5785;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-1406.9,606.4175;Inherit;False;Property;_FresnelColor;FresnelColor;2;0;Create;True;0;0;False;0;0.240566,0.3731039,0.9622642,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;16;-1872.674,-265.8862;Inherit;False;1128.94;560.6946;Albedo;6;3;15;14;2;1;17;Albedo;0,0.6267066,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1108.844,520.1476;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2;-1785.674,12.11384;Inherit;False;Property;_BaseColor;BaseColor;1;0;Create;True;0;0;False;0;0.4716981,0.05562477,0.05562477,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1822.674,-215.8862;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-940.3766,667.6442;Inherit;False;Fresnel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1437.221,-113.7846;Inherit;False;13;Fresnel;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1439.674,-31.88614;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1222.732,-19.19152;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-990.1701,6.894173;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1578.02,886.1104;Inherit;False;853.8401;455.2118;Emission;6;30;28;29;31;32;33;Emission;0.701412,1,0.4669811,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1528.02,936.1104;Inherit;False;17;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;32;-1539.063,1053.322;Inherit;False;Property;_emisColor;emisColor;5;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1283.974,998.8426;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1547.063,1246.322;Inherit;False;Property;_emissive;emissive;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1102.063,1059.322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-948.1793,1058.666;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-394.2202,169.6565;Inherit;False;Property;_Metallic1;Metallic;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-363.5315,-9.142029;Inherit;False;17;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-348.4973,71.99655;Inherit;False;33;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-393.2178,248.8396;Inherit;False;Property;_Smoothness1;Smoothness;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;S_StandardOpaque;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;4;2;7;0
WireConnection;4;3;9;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;13;0;5;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;14;0;15;0
WireConnection;14;1;3;0
WireConnection;17;0;14;0
WireConnection;29;0;28;0
WireConnection;29;1;32;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;33;0;31;0
WireConnection;0;0;18;0
WireConnection;0;2;35;0
WireConnection;0;3;38;0
WireConnection;0;4;37;0
ASEEND*/
//CHKSM=000D7084AE53B7873E6DD95A8E116E790C740CCB