// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Water"
{
	Properties
	{
		_WaveSpeed("Wave Speed", Float) = 0
		_WaveTile("Wave Tile", Float) = 1
		_WaveStretch("Wave Stretch", Vector) = (0.023,0.01,0,0)
		_WaveUp("Wave Up", Vector) = (0,1,0,0)
		_WaveHeight("Wave Height", Float) = 1
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_EdgeDistance("Edge Distance", Float) = 0
		_EdgePower("Edge Power", Range( 0 , 1)) = 0
		_NormalMap("Normal Map", 2D) = "white" {}
		_NormalSpeed("Normal Speed", Float) = 1
		_NormalTile("Normal Tile", Float) = 1
		_NormalStrenght("Normal Strenght", Range( 0 , 1)) = 1
		_Foam(" Foam", 2D) = "white" {}
		_EdgeFoamTile("Edge Foam Tile", Float) = 0
		_FoamTile("Foam Tile", Float) = 0
		_FoamMask("Foam Mask", Float) = 2
		_RefractAmount("Refract Amount", Float) = 0
		_Depth("Depth", Float) = -4
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float3 _WaveUp;
		uniform float _WaveHeight;
		uniform float _WaveSpeed;
		uniform float2 _WaveStretch;
		uniform float _WaveTile;
		uniform sampler2D _NormalMap;
		uniform float _NormalSpeed;
		uniform float _NormalTile;
		uniform float _NormalStrenght;
		uniform float4 _WaterColor;
		uniform sampler2D _Foam;
		uniform float _FoamTile;
		uniform float _FoamMask;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _RefractAmount;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth;
		uniform float _EdgeDistance;
		uniform float _EdgeFoamTile;
		uniform float _EdgePower;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 Tesselation135 = UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, 0.0,80.0,( _WaveHeight * 8.0 ));
			return Tesselation135;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float temp_output_11_0 = ( _Time.y * _WaveSpeed );
			float2 _WaveDirection = float2(-1,0);
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult14 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile15 = appendResult14;
			float4 WaveTileUV25 = ( ( WorldSpaceTile15 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner7 = ( temp_output_11_0 * _WaveDirection + WaveTileUV25.xy);
			float simplePerlin2D6 = snoise( panner7 );
			simplePerlin2D6 = simplePerlin2D6*0.5 + 0.5;
			float2 panner27 = ( temp_output_11_0 * _WaveDirection + ( WaveTileUV25 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D28 = snoise( panner27 );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float temp_output_30_0 = ( simplePerlin2D6 + simplePerlin2D28 );
			float3 WaveHeight36 = ( ( _WaveUp * _WaveHeight ) * temp_output_30_0 );
			v.vertex.xyz += WaveHeight36;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult14 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile15 = appendResult14;
			float4 temp_output_82_0 = ( WorldSpaceTile15 / 10.0 );
			float2 panner69 = ( 1.0 * _Time.y * ( float2( 1,0 ) * _NormalSpeed ) + ( temp_output_82_0 * _NormalTile ).xy);
			float2 panner70 = ( 1.0 * _Time.y * ( float2( -1,0 ) * ( _NormalSpeed * 3.0 ) ) + ( temp_output_82_0 * ( _NormalTile * 5.0 ) ).xy);
			float3 Normals79 = BlendNormals( UnpackScaleNormal( tex2D( _NormalMap, panner69 ), _NormalStrenght ) , UnpackScaleNormal( tex2D( _NormalMap, panner70 ), _NormalStrenght ) );
			o.Normal = Normals79;
			float2 panner104 = ( 1.0 * _Time.y * float2( 0.04,-0.03 ) + ( WorldSpaceTile15 * _FoamMask ).xy);
			float simplePerlin2D103 = snoise( float2( 0,0 )*panner104.x );
			simplePerlin2D103 = simplePerlin2D103*0.5 + 0.5;
			float clampResult110 = clamp( ( tex2D( _Foam, ( ( WorldSpaceTile15 / 10.0 ) * _FoamTile ).xy ).r * simplePerlin2D103 ) , 0.0 , 1.0 );
			float Foam100 = clampResult110;
			float4 temp_cast_5 = (( 0.0 + Foam100 )).xxxx;
			float temp_output_11_0 = ( _Time.y * _WaveSpeed );
			float2 _WaveDirection = float2(-1,0);
			float4 WaveTileUV25 = ( ( WorldSpaceTile15 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner7 = ( temp_output_11_0 * _WaveDirection + WaveTileUV25.xy);
			float simplePerlin2D6 = snoise( panner7 );
			simplePerlin2D6 = simplePerlin2D6*0.5 + 0.5;
			float2 panner27 = ( temp_output_11_0 * _WaveDirection + ( WaveTileUV25 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D28 = snoise( panner27 );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float temp_output_30_0 = ( simplePerlin2D6 + simplePerlin2D28 );
			float WavePattern32 = temp_output_30_0;
			float clampResult46 = clamp( WavePattern32 , 0.0 , 1.0 );
			float4 lerpResult44 = lerp( _WaterColor , temp_cast_5 , clampResult46);
			float4 Albedo47 = lerpResult44;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor118 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( (ase_grabScreenPosNorm).xy ,  0.0 ) + ( _RefractAmount * Normals79 ) ).xy);
			float4 clampResult119 = clamp( screenColor118 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Refraction120 = clampResult119;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth123 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth123 = abs( ( screenDepth123 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth ) );
			float clampResult125 = clamp( ( 1.0 - distanceDepth123 ) , 0.0 , 1.0 );
			float Depth126 = clampResult125;
			float4 lerpResult127 = lerp( Albedo47 , Refraction120 , Depth126);
			o.Albedo = lerpResult127.rgb;
			float screenDepth50 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth50 = abs( ( screenDepth50 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _EdgeDistance ) );
			float4 clampResult57 = clamp( ( ( ( 1.0 - distanceDepth50 ) + tex2D( _Foam, ( ( WorldSpaceTile15 / 10.0 ) * _EdgeFoamTile ).xy ) ) * _EdgePower ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Edge55 = clampResult57;
			o.Emission = Edge55.rgb;
			o.Smoothness = 0.9;
			o.Alpha = 1;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=18912
640;469.6;1523.2;614.2;6093.027;788.2679;2.395183;True;False
Node;AmplifyShaderEditor.CommentaryNode;16;-7421.972,-1111.503;Inherit;False;953.3571;323.0571;Comment;3;13;14;15;World Space UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;13;-7371.973,-1057.839;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;14;-7035.296,-1048.577;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-6722.815,-1051.615;Float;True;WorldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;38;-6288.01,-1129.706;Inherit;False;2317.84;673.7035;;11;33;23;24;34;36;17;19;18;21;20;25;Wave UV's and Height;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;80;-1898.412,-1959.352;Inherit;False;2969.597;1368.531;Comment;21;64;59;68;65;67;66;72;69;71;73;41;70;76;75;74;63;77;78;79;82;83;Normal Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;19;-6211.521,-837.8144;Float;True;Property;_WaveStretch;Wave Stretch;2;0;Create;True;0;0;0;False;0;False;0.023,0.01;0.015,0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;17;-6238.01,-1079.706;Inherit;True;15;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-5932.883,-810.3454;Float;True;Property;_WaveTile;Wave Tile;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-1848.412,-1909.352;Inherit;True;15;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-5953.01,-1070.475;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1560.038,-1898.537;Float;True;Property;_NormalTile;Normal Tile;11;0;Create;True;0;0;0;False;0;False;1;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-1825.954,-1638.998;Float;True;Constant;_Float0;Float 0;13;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-1024.612,-1445.877;Float;True;Property;_NormalSpeed;Normal Speed;10;0;Create;True;0;0;0;False;0;False;1;0.0025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;71;-1184.742,-1698.03;Float;True;Constant;_PanDirection;Pan Direction;10;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;72;-1000.652,-896.0208;Float;True;Constant;_PanD2;PanD2;10;0;Create;True;0;0;0;False;0;False;-1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1316.281,-1159.18;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;82;-1538.07,-1638.998;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;111;-6960.11,-2256.777;Inherit;False;2322.575;875.4021;Comment;13;96;95;98;97;99;94;105;104;103;106;109;100;110;Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-5647.262,-971.2373;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-725.6927,-1295.783;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1297.012,-1868.979;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1000.452,-1167.836;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-501.911,-928.7012;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-853.8723,-1706.448;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-6910.11,-2206.777;Inherit;True;15;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-6855.91,-1962.875;Float;True;Constant;_Float2;Float 2;14;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-6794.442,-1639.775;Float;True;Property;_FoamMask;Foam Mask;16;0;Create;True;0;0;0;False;0;False;2;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-5329.867,-1012.406;Float;True;WaveTileUV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;39;-6990.397,-119.4765;Inherit;False;2045.952;1136.596;Comment;13;10;12;31;29;26;9;11;27;7;6;28;30;32;Wave Pattern;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-6650.594,786.1208;Inherit;True;25;WaveTileUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;97;-6521.214,-2193.287;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-6928.922,679.7806;Float;True;Property;_WaveSpeed;Wave Speed;0;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;93;-6939.271,-3186.496;Inherit;False;1595.871;836.9741;Comment;8;85;87;89;88;86;90;84;92;Edge Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;59;-1809.609,-1297.713;Inherit;True;Property;_NormalMap;Normal Map;9;0;Create;True;0;0;0;False;0;False;None;359dc1316ce14664297e66292880c2f0;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleTimeNode;10;-6930.753,459.9248;Inherit;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-459.5942,-1486.57;Float;True;Property;_NormalStrenght;Normal Strenght;12;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-6510.393,-1952.532;Float;True;Property;_FoamTile;Foam Tile;15;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;70;-217.8417,-1128.689;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-6518.718,-1673.949;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;69;-537.1902,-1836.61;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;9;-6924.004,179.2918;Float;True;Constant;_WaveDirection;Wave Direction;0;0;Create;True;0;0;0;False;0;False;-1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-6520.571,458.1485;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-6398.169,763.7195;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.1,0.1,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-6940.397,-69.47654;Inherit;True;25;WaveTileUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;84;-6260.926,-3136.496;Inherit;True;Property;_Foam; Foam;13;0;Create;True;0;0;0;False;0;False;None;d1bdf56bc5d4646418a34b342a0c54a8;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;104;-6231.219,-1652.454;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.04,-0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;41;-8.400698,-1656.243;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;63;16.20713,-1395.901;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-6243.902,-2183.637;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;94;-5971.267,-2177.751;Inherit;True;Property;_TextureSample3;Texture Sample 3;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-6215.314,121.6307;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BlendNormalsNode;77;482.7805,-1484.903;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;103;-5937.14,-1659.955;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;27;-6084.485,471.8413;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;58;-6831.292,-3935.342;Inherit;False;2076.136;648.719;Comment;7;53;57;55;54;52;50;51;Edge;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-5811.464,452.7922;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;-5948.333,83.56328;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-6889.271,-2878.733;Inherit;True;15;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-5572.692,-1795.498;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-6883.306,-2642.415;Float;True;Constant;_Float1;Float 1;14;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;846.3853,-1458.215;Float;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;122;-1931.995,-3078.791;Inherit;False;1950.584;769.3625;Comment;9;120;116;114;112;115;113;117;118;119;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-5531.092,258.7822;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-1864.789,-2794.839;Float;True;Property;_RefractAmount;Refract Amount;17;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;110;-5252.435,-1811.011;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;116;-1873.168,-2539.428;Inherit;True;79;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-6530.203,-2607.922;Float;True;Property;_EdgeFoamTile;Edge Foam Tile;14;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;131;-1893.35,-3647.153;Inherit;False;1349.95;350.1799;Comment;5;124;123;130;125;126;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-6781.292,-3885.342;Float;True;Property;_EdgeDistance;Edge Distance;7;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;112;-1881.995,-3027.376;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;-6524.467,-2869.376;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-5169.249,332.6635;Float;True;WavePattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;49;-3630.728,-1580.823;Inherit;False;1628.905;944.9741;Comment;8;47;44;42;46;43;45;101;102;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-1489.999,-2762.311;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;113;-1541.791,-3028.791;Inherit;True;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-1843.35,-3597.153;Float;True;Property;_Depth;Depth;18;0;Create;True;0;0;0;False;0;False;-4;-4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;50;-6509.915,-3834.09;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-6234.527,-2899.711;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;100;-5025.442,-2070.1;Float;True;Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;138;-4480.425,-270.3922;Inherit;False;1124.319;627.6643;Comment;6;133;134;22;137;132;135;Tesselation;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;85;-5971.132,-3091.834;Inherit;True;Property;_TextureSample2;Texture Sample 2;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;45;-3548.031,-873.05;Inherit;True;32;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-3572.25,-1093.766;Inherit;True;100;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;-6149.217,-3807.031;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-1207.799,-2832.679;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DepthFade;123;-1638.35,-3591.153;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;130;-1364.691,-3575.156;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-5578.8,-3097.997;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;118;-853.5021,-2823.527;Inherit;False;Global;_GrabScreen0;Grab Screen 0;18;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-3264.913,-1308.278;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-5101.497,-714.4032;Float;True;Property;_WaveHeight;Wave Height;4;0;Create;True;0;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-5783.911,-3758.339;Float;True;Property;_EdgePower;Edge Power;8;0;Create;True;0;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;23;-5105.166,-1038.691;Float;True;Property;_WaveUp;Wave Up;3;0;Create;True;0;0;0;False;0;False;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;46;-3297.705,-878.5741;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-3580.728,-1530.823;Float;False;Property;_WaterColor;Water Color;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2352932,0.5411765,0.7019608,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-4430.425,-29.25422;Float;False;Constant;_Tessalation;Tessalation;3;0;Create;True;0;0;0;False;0;False;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-4254.442,-220.3921;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-4424.974,93.87216;Inherit;False;Constant;_Float3;Float 3;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;119;-552.8079,-2823.528;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-4814.277,-1011.798;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;44;-2932.052,-1502.962;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-5438.568,-3748.471;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;125;-1115.471,-3562.483;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-4425.974,241.8722;Inherit;False;Constant;_Float4;Float 3;20;0;Create;True;0;0;0;False;0;False;80;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;132;-3873.543,19.17144;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-4491.191,-800.6854;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-768.2004,-3555.373;Float;True;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;-206.2107,-2823.616;Float;True;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-2673.79,-1508.244;Float;True;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;57;-5199.242,-3739.017;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-3580.906,49.33998;Float;True;Tesselation;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-700.6378,-235.1619;Inherit;False;120;Refraction;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-4961.881,-3731.916;Float;True;Edge;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-4194.972,-756.4794;Float;True;WaveHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-701.09,-156.7886;Inherit;False;126;Depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-701.2648,-328.0991;Inherit;False;47;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-621.0227,464.046;Inherit;True;36;WaveHeight;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-792.6318,200.3475;Inherit;True;55;Edge;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-741.2561,-20.76204;Inherit;True;79;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;127;-139.7059,-210.3297;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;43;-3572.732,-1328.95;Float;False;Property;_TopColor;Top Color;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.286274,0.6862744,0.8235294,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;136;-13.71871,362.7365;Inherit;True;135;Tesselation;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-437.7209,189.0501;Float;True;Constant;_Smoothness;Smoothness;5;0;Create;True;0;0;0;False;0;False;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;320,-208;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;False;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;13;1
WireConnection;14;1;13;3
WireConnection;15;0;14;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;67;0;66;0
WireConnection;82;0;64;0
WireConnection;82;1;83;0
WireConnection;20;0;18;0
WireConnection;20;1;21;0
WireConnection;75;0;74;0
WireConnection;65;0;82;0
WireConnection;65;1;66;0
WireConnection;68;0;82;0
WireConnection;68;1;67;0
WireConnection;76;0;72;0
WireConnection;76;1;75;0
WireConnection;73;0;71;0
WireConnection;73;1;74;0
WireConnection;25;0;20;0
WireConnection;97;0;95;0
WireConnection;97;1;96;0
WireConnection;70;0;68;0
WireConnection;70;2;76;0
WireConnection;105;0;95;0
WireConnection;105;1;106;0
WireConnection;69;0;65;0
WireConnection;69;2;73;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;29;0;31;0
WireConnection;104;0;105;0
WireConnection;41;0;59;0
WireConnection;41;1;69;0
WireConnection;41;5;78;0
WireConnection;63;0;59;0
WireConnection;63;1;70;0
WireConnection;63;5;78;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;94;0;84;0
WireConnection;94;1;99;0
WireConnection;7;0;26;0
WireConnection;7;2;9;0
WireConnection;7;1;11;0
WireConnection;77;0;41;0
WireConnection;77;1;63;0
WireConnection;103;1;104;0
WireConnection;27;0;29;0
WireConnection;27;2;9;0
WireConnection;27;1;11;0
WireConnection;28;0;27;0
WireConnection;6;0;7;0
WireConnection;109;0;94;1
WireConnection;109;1;103;0
WireConnection;79;0;77;0
WireConnection;30;0;6;0
WireConnection;30;1;28;0
WireConnection;110;0;109;0
WireConnection;89;0;86;0
WireConnection;89;1;90;0
WireConnection;32;0;30;0
WireConnection;115;0;114;0
WireConnection;115;1;116;0
WireConnection;113;0;112;0
WireConnection;50;0;51;0
WireConnection;87;0;89;0
WireConnection;87;1;88;0
WireConnection;100;0;110;0
WireConnection;85;0;84;0
WireConnection;85;1;87;0
WireConnection;52;0;50;0
WireConnection;117;0;113;0
WireConnection;117;1;115;0
WireConnection;123;0;124;0
WireConnection;130;0;123;0
WireConnection;92;0;52;0
WireConnection;92;1;85;0
WireConnection;118;0;117;0
WireConnection;101;1;102;0
WireConnection;46;0;45;0
WireConnection;137;0;33;0
WireConnection;137;1;22;0
WireConnection;119;0;118;0
WireConnection;24;0;23;0
WireConnection;24;1;33;0
WireConnection;44;0;42;0
WireConnection;44;1;101;0
WireConnection;44;2;46;0
WireConnection;53;0;92;0
WireConnection;53;1;54;0
WireConnection;125;0;130;0
WireConnection;132;0;137;0
WireConnection;132;1;133;0
WireConnection;132;2;134;0
WireConnection;34;0;24;0
WireConnection;34;1;30;0
WireConnection;126;0;125;0
WireConnection;120;0;119;0
WireConnection;47;0;44;0
WireConnection;57;0;53;0
WireConnection;135;0;132;0
WireConnection;55;0;57;0
WireConnection;36;0;34;0
WireConnection;127;0;48;0
WireConnection;127;1;128;0
WireConnection;127;2;129;0
WireConnection;0;0;127;0
WireConnection;0;1;81;0
WireConnection;0;2;56;0
WireConnection;0;4;40;0
WireConnection;0;11;37;0
WireConnection;0;14;136;0
ASEEND*/
//CHKSM=FF4F771ADD3B60BF20EFD039B951D29F0E391153