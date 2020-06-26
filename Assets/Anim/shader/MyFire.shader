Shader "Unlit/MyFire"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed",Range(-10,10))=1
		
		_Top("Top",Range(0,1))=0
		_Bottom("Bottom",Range(0,1))=0
		_Height("Height",Range(0,1))=1
		_Edge("Edge",Range(0,1))=0.5
		_AlphaControl("AlphaControl",Range(0,20))=20
		_MoreGradient("MoreGradient",Range(0,1))=0.5
		
		_MainColor("MainColor",Color)=(1,1,1,1)
    }
    SubShader
    {
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
		Zwrite Off Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Top,_Bottom,_Height,_Speed,_Edge,_AlphaControl,_MoreGradient;
			float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float gradientBlend = lerp(2, 0, saturate(saturate(i.uv.y*_Height-_Top)+saturate((1-i.uv.y)*_Height-_Bottom)));
                fixed4 color = tex2D(_MainTex,fixed2( i.uv.x,i.uv.y+_Speed*_Time.x));
				color.a = saturate(( gradientBlend * lerp(_MoreGradient,1,color.r)-_Edge)*_AlphaControl);
				color.rgb=_MainColor.rgb;
                return color;
            }
            ENDCG
        }
    }
}
