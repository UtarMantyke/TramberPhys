// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ErbGameArt/Potion" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Emissivepower ("Emissive power", Float ) = 10
        _Opacitypower (" Opacity power", Float ) = 1
        _Mask ("Mask", 2D) = "white" {}
        _Maskpower ("Mask power", Float ) = 1
        _Maskcolor ("Mask color", Color) = (0.9926471,0,0.7598884,1)
        _NumberofWaves ("Number of Waves", Float ) = 20
        _Wavessize ("Waves size", Range(5, 40)) = 20
        _Wavesspeed ("Waves speed", Range(-30, 30)) = 5
        _Fullness ("Fullness", Range(0, 2)) = 1
        _PotionBubbles ("PotionBubbles", 2D) = "white" {}
        _Speedofbubbles ("Speed of bubbles", Range(0, 1)) = 0.3
        _Powerofbubbles ("Power of bubbles", Range(1, 20)) = 2.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _Maskpower;
            uniform float _Emissivepower;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Opacitypower;
            uniform float4 _Maskcolor;
            uniform float _NumberofWaves;
            uniform float _Wavessize;
            uniform float _Wavesspeed;
            uniform float _Fullness;
            uniform sampler2D _PotionBubbles; uniform float4 _PotionBubbles_ST;
            uniform float _Speedofbubbles;
            uniform float _Powerofbubbles;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_4935 = _Time + _TimeEditor;
                float4 node_4094 = _Time + _TimeEditor;
                float2 node_2391 = (i.uv0+(_Speedofbubbles*node_4094.g)*float2(0,-1));
                float4 _PotionBubbles_var = tex2D(_PotionBubbles,TRANSFORM_TEX(node_2391, _PotionBubbles));
                float node_603 = (_MainTex_var.a*_Color.a*i.vertexColor.a*_Opacitypower); // A
                float3 emissive = (((_MainTex_var.rgb+((_Mask_var.rgb*_Maskpower)*_Emissivepower*_Maskcolor.rgb*saturate((sin(((_Wavesspeed*node_4935.g)+(_NumberofWaves*i.uv0.r)))+(1.0 - ((pow(i.uv0.g,_Fullness)*2.0+-1.0)*_Wavessize))))*pow(_PotionBubbles_var.rgb,_Powerofbubbles)))*_Color.rgb*i.vertexColor.rgb*_Opacitypower)*node_603);
                return fixed4(emissive,node_603);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
