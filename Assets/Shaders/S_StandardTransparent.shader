// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_StandardTransparent"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Contrast("Contrast", Float) = 0
		_BaseColor("BaseColor", Color) = (0.4716981,0.05562477,0.05562477,0)
		_FresnelColor("FresnelColor", Color) = (0.240566,0.3731039,0.9622642,0)
		_FresnalScale("FresnalScale", Range( 0 , 10)) = 1
		_FresnelPower("FresnelPower", Range( 0 , 10)) = 1
		[HDR]_emisColor("emisColor", Color) = (1,1,1,0)
		_emissive("emissive", Range( 0 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Alpha("Alpha", 2D) = "white" {}
		[Toggle]_AlphaChanel("AlphaChanel", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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

		uniform float _Contrast;
		uniform float _FresnalScale;
		uniform float _FresnelPower;
		uniform float4 _FresnelColor;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _BaseColor;
		uniform float4 _emisColor;
		uniform float _emissive;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _AlphaChanel;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV4 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4 = ( 0.0 + _FresnalScale * pow( 1.0 - fresnelNdotV4, ( _FresnelPower * 2.0 ) ) );
			float4 Fresnel13 = ( fresnelNode4 * _FresnelColor );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 Albedo17 = CalculateContrast(_Contrast,( Fresnel13 + ( tex2D( _Albedo, uv_Albedo ) * _BaseColor ) ));
			o.Albedo = Albedo17.rgb;
			float4 Emission33 = ( ( Albedo17 * _emisColor ) * _emissive );
			o.Emission = Emission33.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 tex2DNode39 = tex2D( _Alpha, uv_Alpha );
			float lerpResult43 = lerp( tex2DNode39.r , tex2DNode39.a , lerp(0.0,1.0,_AlphaChanel));
			o.Alpha = ( _Opacity * lerpResult43 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
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
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
291;73;1088;611;4181.734;1575.541;5.42453;True;True
Node;AmplifyShaderEditor.CommentaryNode;11;-2207.97,321.5785;Inherit;False;1482.869;517.6389;Fresnel;8;6;7;8;10;9;5;4;13;Fresnel;1,0.4661931,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1851.901,575.4175;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2157.97,541.3332;Inherit;False;Property;_FresnelPower;FresnelPower;5;0;Create;True;0;0;False;0;1;0.13;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1938.669,434.5718;Inherit;False;Property;_FresnalScale;FresnalScale;4;0;Create;True;0;0;False;0;1;1.18;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1689.9,547.4175;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;4;-1424.521,371.5785;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-1406.9,606.4175;Inherit;False;Property;_FresnelColor;FresnelColor;3;0;Create;True;0;0;False;0;0.240566,0.3731039,0.9622642,0;0,0.4727035,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;16;-2208.476,-334.02;Inherit;False;1452.72;618.1137;Albedo;8;17;14;3;15;1;2;41;42;Albedo;0,0.6267066,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1108.844,520.1476;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2;-2121.477,-56.02005;Inherit;False;Property;_BaseColor;BaseColor;2;0;Create;True;0;0;False;0;0.4716981,0.05562477,0.05562477,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2158.477,-284.02;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;b327b80b9678b1549aeed83b123f659c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-940.3766,667.6442;Inherit;False;Fresnel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1773.024,-181.9184;Inherit;False;13;Fresnel;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1775.477,-100.02;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1507.732,145.4946;Inherit;False;Property;_Contrast;Contrast;1;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1558.535,-87.32542;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;41;-1301.151,84.09079;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-1037.114,84.69521;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1578.02,886.1104;Inherit;False;853.8401;455.2118;Emission;6;30;28;29;31;32;33;Emission;0.701412,1,0.4669811,1;0;0
Node;AmplifyShaderEditor.ColorNode;32;-1538.063,1054.322;Inherit;False;Property;_emisColor;emisColor;6;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1528.02,936.1104;Inherit;False;17;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1283.974,998.8426;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1536.063,1233.322;Inherit;False;Property;_emissive;emissive;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;45;-565.0123,659.2962;Inherit;False;Property;_AlphaChanel;AlphaChanel;12;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1102.063,1059.322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;39;-647.9764,446.7148;Inherit;True;Property;_Alpha;Alpha;11;0;Create;True;0;0;False;0;None;b327b80b9678b1549aeed83b123f659c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-414.361,350.8722;Inherit;False;Property;_Opacity;Opacity;8;0;Create;True;0;0;False;0;1;0.28;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;43;-318.934,563.598;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-948.1793,1058.666;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-364.7175,88.58359;Inherit;False;33;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-363.5315,-9.142029;Inherit;False;17;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-416.6283,259.8807;Inherit;False;Property;_Smoothness;Smoothness;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-417.6306,180.6976;Inherit;False;Property;_Metallic;Metallic;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-122.4945,447.4945;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;S_StandardTransparent;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;41;1;14;0
WireConnection;41;0;42;0
WireConnection;17;0;41;0
WireConnection;29;0;28;0
WireConnection;29;1;32;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;43;0;39;1
WireConnection;43;1;39;4
WireConnection;43;2;45;0
WireConnection;33;0;31;0
WireConnection;40;0;36;0
WireConnection;40;1;43;0
WireConnection;0;0;18;0
WireConnection;0;2;35;0
WireConnection;0;3;38;0
WireConnection;0;4;37;0
WireConnection;0;9;40;0
ASEEND*/
//CHKSM=431AFE96A95786F11AC01943761EA6A70AFFD18E