	// Copyright 2017 Ramiro Oliva (Kronnect) - All Rights Reserved.
		
	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D _BlurTex;
	uniform float4    _MainTex_TexelSize;
	uniform float4    _MainTex_ST;

	uniform float _Radius;
	uniform int   _Samples;
	uniform float _Threshold;
	uniform float _Intensity;
	uniform float _LumaProtect;
	uniform float _BlurSpread;
	uniform float2 _Direction;

    struct appdata {
    	float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
    };
    
	struct v2f {
	    float4 pos : SV_POSITION;
	    float2 uv: TEXCOORD0;
	};

	struct v2fCross {
	    float4 pos : SV_POSITION;
	    float2 uv: TEXCOORD0;
	    float2 uv1: TEXCOORD1;
	    float2 uv2: TEXCOORD2;
	    float2 uv3: TEXCOORD3;
	    float2 uv4: TEXCOORD4;
	};


	v2f vert(appdata v) {
    	v2f o;
    	o.pos = UnityObjectToClipPos(v.vertex);
   		o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
    	return o;
	}
		

	float2 getRandom(float k) {
		if (k==0) return float2(0.07779833, 0.2529951);
		else if (k==1) return float2(-0.1778869f, -0.05900348);
		else if (k==2) return float2(0.8558092, 0.2799575);
		else if (k==3) return float2(-0.03023551, 0.8480632);
		else if (k==4) return float2(0.4166129, 0.8863604f);
		else if (k==5) return float2(0.3985788, -0.03791248);
		else if (k==6) return float2(-0.44102, 0.2654153);
		else if (k==7) return float2(-0.4586931, 0.7403293);
		else if (k==8) return float2(0.1117442, -0.5198008);
		else if (k==9) return float2(-0.8176585, 0.1296148);
		else if (k==10) return float2(-0.7903557, -0.2716176);
		else if (k==11) return float2(-0.4248519, -0.4493517);
		else if (k==12) return float2(0.8380554, -0.3609802);
		else if (k==13) return float2(0.4613214, 0.409142);
		else if (k==14) return float2(0.553355, -0.7046115);
		else return float2(-0.1786912f, -0.8461482);
	}

	float getSaturation(float3 rgb) {
//		float ma = max(rgb.r, max(rgb.g, rgb.b)); // ignoring green channel produces similar results
//		float mi = min(rgb.r, min(rgb.g, rgb.b));
		float ma = max(rgb.r, rgb.b);
		float mi = min(rgb.r, rgb.b);
		return ma - mi;
	}

	float getLuma(float3 rgb) {
		const float3 lum = float3(0.299, 0.587, 0.114);
		return dot(rgb, lum);
	}

	v2fCross vertBlurH(appdata v) {
    	v2fCross o;
    	o.pos = UnityObjectToClipPos(v.vertex);
    	o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
		float2 inc = half2(_MainTex_TexelSize.x * 1.3846153846, 0);	
		#if UNITY_SINGLE_PASS_STEREO
		inc.x *= 2.0;
		#endif
    	o.uv1 = UnityStereoScreenSpaceUVAdjust(v.texcoord - inc, _MainTex_ST);	
    	o.uv2 = UnityStereoScreenSpaceUVAdjust(v.texcoord + inc, _MainTex_ST);	
		float2 inc2 = half2(_MainTex_TexelSize.x * 3.2307692308, 0);	
		#if UNITY_SINGLE_PASS_STEREO
		inc2.x *= 2.0;
		#endif
		o.uv3 = UnityStereoScreenSpaceUVAdjust(v.texcoord - inc2, _MainTex_ST);
    	o.uv4 = UnityStereoScreenSpaceUVAdjust(v.texcoord + inc2, _MainTex_ST);	
		return o;
	}	
	
	v2fCross vertBlurV(appdata v) {
    	v2fCross o;
    	o.pos = UnityObjectToClipPos(v.vertex);
    	o.uv  = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
		float2 inc = half2(0, _MainTex_TexelSize.y * 1.3846153846);	
    	o.uv1 = UnityStereoScreenSpaceUVAdjust(v.texcoord - inc, _MainTex_ST);	
    	o.uv2 = UnityStereoScreenSpaceUVAdjust(v.texcoord + inc, _MainTex_ST);	
    	float2 inc2 = half2(0, _MainTex_TexelSize.y * 3.2307692308);	
    	o.uv3 = UnityStereoScreenSpaceUVAdjust(v.texcoord - inc2, _MainTex_ST);	
    	o.uv4 = UnityStereoScreenSpaceUVAdjust(v.texcoord + inc2, _MainTex_ST);	
    	return o;
	}
	

	float4 frag (v2f i) : SV_Target {

   		float4 pix          = tex2D(_MainTex, i.uv);
   		float  satM         = getSaturation(pix.rgb);
   		float2 grd          = float2(ddx(satM), ddy(satM));
   		float4 occlusion    = 0.0;
		float2 amp          = _Radius.xx * _MainTex_TexelSize.xy;
		float rindex        = floor(frac(sin(dot(i.uv, float2(12.9898, 78.233))) * 43758.5453) * (16.0 - _Samples));

		for(int k=0; k<_Samples; k++) {
	        float2 offset   = getRandom(rindex++);
	        #if LBAO_DIRECTIONAL
	  	    offset.xy       = abs(offset.xy) * _Direction;
	  	    #endif
			float4 occUV    = float4(i.uv + offset * amp, 0, 0);
  	    	float3 occRGB   = tex2Dlod(_MainTex, occUV).rgb;
        	float  occSat   = getSaturation(occRGB);
			float  occ      = saturate( (occSat - satM - _Threshold) / _Threshold );
      		occlusion.rgb  += occRGB;
       		occlusion.a    += occ;
	    }
    	occlusion    = occlusion / float(_Samples);
    	occlusion.a *= _Intensity;

    	#if LBAO_BLUR_ON
    	    pix = occlusion;
    	#else
    		float lumaM = getLuma(pix.rgb);
    		#if LBAO_DEBUG_ON
    		pix.rgb  = 1.0.xxx;
    		#endif
	   		float lp = saturate( (_LumaProtect - lumaM) / 0.1 );
    		pix.rgb *= lerp(1.0.xxx, occlusion.rgb, occlusion.a * lp);
		#endif
		return pix;
	}

	float4 fragBlur (v2fCross i): SV_Target {
		return tex2D(_MainTex, i.uv) * 0.2270270270 + (tex2D(_MainTex, i.uv1) + tex2D(_MainTex, i.uv2)) * 0.3162162162 + (tex2D(_MainTex, i.uv3) + tex2D(_MainTex, i.uv4)) * 0.0702702703;
	}	

	float4 fragCompose(v2f i): SV_Target {
		float4 pix       = tex2D(_MainTex, i.uv);
		float  lumaM     = getLuma(pix.rgb);
		float  lp        = saturate( (_LumaProtect - lumaM) / 0.1 );
		#if LBAO_DEBUG_ON
		pix.rgb = 1.0.xxx;
		#endif
		float4 occlusion = tex2D(_BlurTex, i.uv);
   		pix.rgb *= lerp(1.0.xxx, occlusion.rgb, occlusion.a * lp);
		return pix;
	}

