#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif


//構造体の定義(照明の計算に必要なデータをここに入れる)
struct CustomLightingData {

    //位置と向き
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;

    //メインライトの影はシャドウマップに保持される
    //システムがそれらを読み取るためには影座標が必要(float 4)
    float4 shadowCoord;

    // 表面の属性
    float3 albedo;
    float3 smoothness;
};

// Translate a [0, 1] smoothness value to an exponent 
float GetSmoothnessPower(float rawSmoothness) {
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
float3 CustomLightHandling(CustomLightingData d, Light light){

    //ライトの色
    float3 radiance = light.color * light.shadowAttenuation;
    

    //拡散照明
    //saturate:指定した値を 0 から 1 の範囲内で返す
    //物体表面の法線ベクトルとライトの方向ベクトルの内積を取得
    float diffuse = saturate(dot(d.normalWS, light.direction));
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;

    //出力する色
    float color = d.albedo * radiance * (diffuse + specular);

    return color;
}
#endif


//照明のアルゴリズムを書く(ここが実行される部分)
float3 CalculateCustomLighting(CustomLightingData d){
    
#ifdef SHADERGRAPH_PREVIEW
    //プレビューで拡散光＋鏡面反射光を推定
    float3 lightDir = (0.5, 0.5, 0);
     float intensity = saturate(dot(d.normalWS, lightDir)) + pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
#else
    //メインライトを取得
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);

    float3 color =0;

    color += CustomLightHandling(d, mainLight);
        
    return color;

#endif
}


//シェーダーグラフが呼び出せるカスタムラッパー関数
//ここの変数をShaderGraphのInputs,Outputsに入れる
void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 Albedo, float3 ViewDirection, float Smoothness, out float3 Color){

    CustomLightingData d;
    d.positionWS = Position;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;

#ifdef SHADERGRAPH_PREVIEW
    // In preview, there's no shadows or bakedGI
    d.shadowCoord = 0;
#else
    // Calculate the main light shadow coord
    // There are two types depending on if cascades are enabled
    float4 positionCS = TransformWorldToHClip(Position);
    #if SHADOWS_SCREEN
        d.shadowCoord = ComputeScreenPos(positionCS);
    #else
        d.shadowCoord = TransformWorldToShadowCoord(Position);
    #endif
#endif

    Color = CalculateCustomLighting(d);
}

#endif