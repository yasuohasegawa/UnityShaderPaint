// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/Drawing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BrushSize("BrushSize",Float) = 20
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
        //Cull Off ZWrite On AlphaTest Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _PreviousTexture2;
			float4 _MainTex_ST;
			float4 _mouse;
			float4 _selectedColor;
			float _BrushSize;
			int _DrawFlg;
			int _blushType;

			int init = 0;

			float n1(float n) {
			 	return frac(cos(n*85.62+0.123456789)*941.53);   
			}

			float n1(float2 n) {
			 	return n1(n1(n.x)*1436.6+n1(n.y)*346.2);   
			}

			float2 n2(float n) {
			 	return float2(n1(n),n1(n*2.79-400.0));   
			}

			float3 n3(float n) {
			 	return float3(n1(n),n1(n*2.79-400.0),n1(600.0-n*3.32));   
			}

			float3 n3(float2 n){
			 	return n3(n1(n.x)*0.74+n1(n.y)*0.91);   
			}

			float p1(float2 n) {
			 	float2 F = floor(n);
			    float2 S = frac(n);
			    return lerp(lerp(n1(F),n1(F+float2(1,0)),S.x),
			               lerp(n1(F+float2(0,1)),n1(F+float2(1,1)),S.x),S.y);
			}

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed2 resolution = _ScreenParams;
				fixed2 fragCord = i.uv*_ScreenParams;
					
			    float distance = length( fragCord - _mouse.xy*resolution ) / _BrushSize;
			    if(_blushType == 1) {
			    	distance = distance+p1(fragCord*0.3)*0.5;
			    } else if(_blushType == 2) {
			    	distance = distance*p1(fragCord/0.2)*0.5+0.9;
			    }

			    if(_Time.w>=0.1){
			    	init = 1;
			    }

			    if(_DrawFlg == 1){
					if (distance < 1.0) {
						col = _selectedColor;
				    } else {
				    	if(init == 1){
				    		col = tex2D(_PreviousTexture2, i.uv);
				    	} else {
				    		discard;
				    	}
				    }
			    } else {
			    	col = fixed4(0.0,0.0,0.0,1.0);
			    }

				return col;
			}
			ENDCG
		}

        GrabPass{"_PreviousTexture2"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


            sampler2D _PreviousTexture2;
            float4 _PreviousTexture2_ST;
            float4 _mouse;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _PreviousTexture2);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_PreviousTexture2, i.uv);
                col.rgb = 1.0-col.rgb;
                return col;
            }
            ENDCG
        }

	}
}