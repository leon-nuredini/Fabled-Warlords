Shader "Hidden/LBAO" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius ("Sample Radius", Float) = 12.0
		_Samples ("Sample Count", Int) = 8.0
		_Threshold ("Threshold", Float) = 0.2
		_Intensity ("Intensity", Float) = 0.5
		_LumaProtect ("Luma Protect", Float) = 0.1
		_BlurSpread ("Blur Spread", Float) = 1.0
		_Direction ("Direction", Vector) = (-1.0,1.0,0)
	}

Subshader {	

  Pass { // 0
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode Off }
	  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma target 3.0
      #pragma multi_compile __ LBAO_DEBUG_ON
      #pragma multi_compile __ LBAO_BLUR_ON
      #pragma multi_compile __ LBAO_DIRECTIONAL
      #include "LBAO.cginc"
      ENDCG
  }

   Pass { // 1
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode Off }
	  
      CGPROGRAM
      #pragma vertex vertBlurH
      #pragma fragment fragBlur
      #pragma fragmentoption ARB_precision_hint_fastest
      #include "LBAO.cginc"
      ENDCG
  }    
  
      
  Pass { // 2
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode Off }
	  
      CGPROGRAM
      #pragma vertex vertBlurV
      #pragma fragment fragBlur
      #pragma fragmentoption ARB_precision_hint_fastest
      #include "LBAO.cginc"
      ENDCG
  }    

  Pass { // 3
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode Off }
	  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragCompose
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma multi_compile __ LBAO_DEBUG_ON
      #include "LBAO.cginc"
      ENDCG
  }  

}
FallBack Off
}
