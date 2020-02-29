﻿
Shader "Hidden/BlendModes/UnlitTransparent/Overlay" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _BLENDMODES_OverlayTexture("Overlay Texture", 2D) = "white" {}
        _BLENDMODES_OverlayColor("Overlay Color", Color) = (1,1,1,1)
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

   ZWrite On
    Blend SrcAlpha OneMinusSrcAlpha

    

    Pass {

        

        CGPROGRAM
            #pragma vertex vert
           #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            
            #pragma multi_compile BLENDMODES_MODE_DARKEN BLENDMODES_MODE_MULTIPLY BLENDMODES_MODE_COLORBURN BLENDMODES_MODE_LINEARBURN BLENDMODES_MODE_DARKERCOLOR BLENDMODES_MODE_LIGHTEN BLENDMODES_MODE_SCREEN BLENDMODES_MODE_COLORDODGE BLENDMODES_MODE_LINEARDODGE BLENDMODES_MODE_LIGHTERCOLOR BLENDMODES_MODE_OVERLAY BLENDMODES_MODE_SOFTLIGHT BLENDMODES_MODE_HARDLIGHT BLENDMODES_MODE_VIVIDLIGHT BLENDMODES_MODE_LINEARLIGHT BLENDMODES_MODE_PINLIGHT BLENDMODES_MODE_HARDMIX BLENDMODES_MODE_DIFFERENCE BLENDMODES_MODE_EXCLUSION BLENDMODES_MODE_SUBTRACT BLENDMODES_MODE_DIVIDE BLENDMODES_MODE_HUE BLENDMODES_MODE_SATURATION BLENDMODES_MODE_COLOR BLENDMODES_MODE_LUMINOSITY

            #include "UnityCG.cginc"
            #include "../../../Shaders/BlendModesCG.cginc"

           struct appdata_t {
                float4 vertex : POSITION;
               float2 texcoord : TEXCOORD0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
           };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
               BLENDMODES_OVERLAY_TEX_COORD(2)
                UNITY_VERTEX_OUTPUT_STEREO
           };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            BLENDMODES_OVERLAY_VARIABLES

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
               UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                BLENDMODES_TRANSFORM_OVERLAY_TEX(o.texcoord, o)
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                BLENDMODES_BLEND_PIXEL_OVERLAY(col, i)
               UNITY_APPLY_FOG(i.fogCoord, col);
                
               return col;
            }
        ENDCG
    }
}

   Fallback "Unlit/Transparent"
}
