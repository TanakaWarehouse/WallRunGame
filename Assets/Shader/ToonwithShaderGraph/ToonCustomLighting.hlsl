#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED


//構造体の定義(照明の計算に必要な変数をここに入れる)
struct ToonLightingData {

    // 位置と向き
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;

    //メインライトの影はシャドウマップに保持される
    //システムがそれらを読み取るためには影座標が必要(float 4)
    float4 shadowCoord;

    // 表面
    float3 albedo;
    float3 ambientcolor;
    float3 specularcolor;
    float smoothness;
    float3 rimcolor;
    float rimamount;
    float rimthreshold;
    float4 sampleTexture;

};

// Translate a [0, 1] smoothness value to an exponent 
float GetSmoothnessPower(float rawSmoothness) {
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW  //シェーダーグラフのプレビューウィンドウをレンダリングするために必要
//ここからどんな照明にするかを書く(つまりトゥーンっぽくする処理はここに書く)
float3 CustomLightHandling(ToonLightingData d, Light light) {


    float3 normal = normalize(d.normalWS);
    float NdotL = dot(light.direction, normal);

    //影
    float shadow = light.shadowAttenuation;

    float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
    float3 toonlight = lightIntensity * light.color;
    
    float3 viewDir = normalize(d.viewDirectionWS);
    float3 halfVector = normalize(light.direction + viewDir);
    float NdotH = dot(normal, halfVector);
    
    float specularIntensity = pow(NdotH * lightIntensity, d.smoothness * d.smoothness);
    float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
    float3 specular = specularIntensitySmooth * d.specularcolor;
    
    float3 rimDot = 1 - dot(viewDir, normal);
    float rimIntensity = rimDot * pow(NdotL, d.rimthreshold);
    rimIntensity = smoothstep(d.rimamount - 0.01, d.rimamount + 0.01, rimIntensity);
    float3 rim = rimIntensity * d.rimcolor; 

    float3 color = d.sampleTexture * (d.ambientcolor + toonlight + specular + rim) * d.albedo;

    return color;
}
#endif

//照明のアルゴリズム(ShaderGraphで仮想光源用意するなら，ここはほぼ固定なはず)
//これをCustomFunctionのNameに入れる
float3 CalculateToonLighting(ToonLightingData d) {
#ifdef SHADERGRAPH_PREVIEW
    // プレビュー用 diffuse + specular
    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.normalWS, lightDir)) + pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
#else
    // メインライト取得 (Located in URP/ShaderLibrary/Lighting.hlsl)
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);
    
    float3 color = 0;

    // 自分で作ったライトの効果を加える
    color += CustomLightHandling(d, mainLight);

    return color;
#endif
}


//シェーダーグラフが呼び出せるカスタムラッパー関数
//ここの変数をShaderGraphのInputs,Outputsに入れる
void CalculateToonLighting_float(float4 SampleTexture, float3 Position, float3 Normal, float3 Albedo, float3 ViewDirection, float Smoothness, 
    float3 AmbientColor, float3 SpecularColor, float3 RimColor, float RimAmount, float RimThreshold,out float3 Color) {

        ToonLightingData d;
        d.sampleTexture = SampleTexture;
        d.positionWS = Position;
        d.normalWS = Normal;
        d.viewDirectionWS = ViewDirection;
        d.albedo = Albedo;
        d.smoothness = Smoothness;
        d.ambientcolor = AmbientColor;
        d.specularcolor = SpecularColor;
        d.rimcolor = RimColor;
        d.rimamount = RimAmount;
        d.rimthreshold = RimThreshold;

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


        Color = CalculateToonLighting(d);
}

#endif