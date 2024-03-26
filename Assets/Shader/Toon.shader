Shader "Custom/Toon"
{
    //UnityのInspectorでいじれる部分
    Properties
    {
        _Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}

        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)

        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
        _Glossiness("Glossiness", Float) = 32
        
        [HDR]
        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0,1)) = 0.1

    }

    SubShader
    {
        Tags 
        { 

            "RenderType"="Opaque"

            
			"LightMode" = "UniversalForward"

			//DirectionalLightのみの影響を受けるように
			"PassFlags" = "OnlyDirectional"


        }
        LOD 100

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap
            

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                //頂点座標
				float4 vertex : POSITION;

				//テクスチャ座標				
				float4 uv : TEXCOORD0;

				//法線
				float3 normal : NORMAL;
            };

            struct v2f
            {
                //テクスチャ座標
                float2 uv : TEXCOORD0;

                //クリップスペース位置
                float4 pos : SV_POSITION;

                float3 worldNormal : NORMAL;

                //鏡面反射のための「見ている角度」の情報
                float3 viewDir : TEXCOORD1;

                //影のデータをTEXCOORD2に格納
                SHADOW_COORDS(2)

                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            //反射光
            float4 _AmbientColor; 
            
            //鏡面反射
            float _Glossiness;
            float4 _SpecularColor;

            //リムライト
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;



            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                //現在の頂点からカメラに向かう方向
                o.viewDir = WorldSpaceViewDir(v.vertex);
                TRANSFER_SHADOW(o);

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            { 

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                
                float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

                //影
                fixed shadow = SHADOW_ATTENUATION(i);
                
                //NdotLが0より大きかったら1,そうじゃなきゃ0
                //float lightIntensity = NdotL > 0 ? 1 : 0;

                float lightIntensity = smoothstep(0, 0.01, NdotL * shadow); //光の境界をsmoothstep関数で滑らかに

                //ここで光の色を含める(_LightColor0はDirectionalLightの色)
                float4 light = lightIntensity * _LightColor0;


                //鏡面反射の強度はブリンフォン法で，表面の法線と半分のベクトルの内積で定義されている
                float3 viewDir = normalize(i.viewDir);
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

                //smoothstep関数で滑らかに
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;	
                
                //リムライト
                float4 rimDot = 1 - dot(viewDir, normal);

                //float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot); //オブジェクト全体にリムライト
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold); //光の当たっている部分だけリムライト
                
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);

                float4 rim = rimIntensity * _RimColor;

                

                return col * (_AmbientColor + light + specular + rim) * _Color;
            }
            ENDCG
        }

        //影の投影(別のシェーダーからパスを取得して，このシェーダーに挿入可能)
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

    }
    
    
}

