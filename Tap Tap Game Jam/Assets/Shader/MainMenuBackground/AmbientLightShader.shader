Shader "Custom/AmbientLightShader"
{
    Properties
    {
        // 这些属性主要用于在材质检视面板中显示，实际的值由 C# 脚本在运行时传入
        _Speed ("Speed", Range(0.01, 10.0)) = 1.0
        _PatternScale("Pattern Scale", Range(0.1, 20.0)) = 5.0
        _CurlScale("Edge Blur", Range(0.01, 10.0)) = 1.0
        _Brightness("Brightness", Range(0.0, 4.0)) = 1.0
        _Darkness("Darkness", Range(0.0, 2.0)) = 0.0
        _NoiseFactor("Noise Factor", Range(0.0, 0.4)) = 0.05
        
        _Color0 ("Color 0", Color) = (1,0,0,1)
        _Color1 ("Color 1", Color) = (1,1,0,1)
        _Color2 ("Color 2", Color) = (0,1,0,1)
        _Color3 ("Color 3", Color) = (0,1,1,1)
        _Color4 ("Color 4", Color) = (0,0,1,1)
        _Color5 ("Color 5", Color) = (1,0,1,1)
    }
    SubShader
    {
        // 不需要剔除、深度测试或写入深度
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
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

            // 从 C# 脚本传入的变量
            float _Speed;
            float _PatternScale;
            float _CurlScale;
            float _Brightness;
            float _Darkness;
            float _NoiseFactor;
            fixed4 _Color0, _Color1, _Color2, _Color3, _Color4, _Color5;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            // --- Ported GLSL Noise Functions to HLSL ---
            float mod289(float x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
            float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
            float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
            float4 mod289(float4 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; } // Added for float4

            float permute(float v) { return mod289(((v * 34.0) + 1.0) * v); }
            float3 permute(float3 v) { return mod289(((v * 34.0) + 1.0) * v); }
            float4 permute(float4 v) { return mod289(((v * 34.0) + 1.0) * v); }

            float taylorInvSqrt(in float r) { return 1.79284291400159 - 0.85373472095314 * r; }
            
            float snoise(in float3 v)
            {
                const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
                const float4 D = float4(0.0, 0.5, 1.0, 2.0);

                float3 i = floor(v + dot(v, C.yyy));
                float3 x0 = v - i + dot(i, C.xxx);

                float3 g = step(x0.yzx, x0.xyz);
                float3 l = 1.0 - g;
                float3 i1 = min(g.xyz, l.zxy);
                float3 i2 = max(g.xyz, l.zxy);
                
                float3 x1 = x0 - i1 + C.xxx;
                float3 x2 = x0 - i2 + C.yyy;
                float3 x3 = x0 - D.yyy;

                i = mod289(i);
                float4 p = permute(permute(permute(
                            i.z + float4(0.0, i1.z, i2.z, 1.0))
                        + i.y + float4(0.0, i1.y, i2.y, 1.0))
                        + i.x + float4(0.0, i1.x, i2.x, 1.0));

                float n_ = 0.142857142857;
                float3 ns = n_ * D.wyz - D.xzx;

                float4 j = p - 49.0 * floor(p * ns.z * ns.z);

                float4 x_ = floor(j * ns.z);
                float4 y_ = floor(j - 7.0 * x_);

                float4 x = x_ * ns.x + ns.yyyy;
                float4 y = y_ * ns.x + ns.yyyy;
                float4 h = 1.0 - abs(x) - abs(y);

                float4 b0 = float4(x.xy, y.xy);
                float4 b1 = float4(x.zw, y.zw);

                float4 s0 = floor(b0) * 2.0 + 1.0;
                float4 s1 = floor(b1) * 2.0 + 1.0;
                float4 sh = -step(h, float4(0.0, 0.0, 0.0, 0.0));

                float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
                float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

                float3 p0 = float3(a0.xy, h.x);
                float3 p1 = float3(a0.zw, h.y);
                float3 p2 = float3(a1.xy, h.z);
                float3 p3 = float3(a1.zw, h.w);

                float4 norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
                p0 *= norm.x;
                p1 *= norm.y;
                p2 *= norm.z;
                p3 *= norm.w;

                float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
                m = m * m;
                return 42.0 * dot(m * m, float4(dot(p0, x0), dot(p1, x1), dot(p2, x2), dot(p3, x3)));
            }

            float3 snoise3(float3 x)
            {
                float s = snoise(x);
                float s1 = snoise(float3(x.y - 19.1, x.z + 33.4, x.x + 47.2));
                float s2 = snoise(float3(x.z + 74.2, x.x - 124.5, x.y + 99.4));
                return float3(s, s1, s2);
            }

            float3 curl(float3 p)
            {
                const float e = 0.1;
                float3 dx = float3(e, 0.0, 0.0);
                float3 dy = float3(0.0, e, 0.0);
                float3 dz = float3(0.0, 0.0, e);

                float3 p_x0 = snoise3(p - dx);
                float3 p_x1 = snoise3(p + dx);
                float3 p_y0 = snoise3(p - dy);
                float3 p_y1 = snoise3(p + dy);
                float3 p_z0 = snoise3(p - dz);
                float3 p_z1 = snoise3(p + dz);

                float x = p_y1.z - p_y0.z - p_z1.y + p_z0.y;
                float y = p_z1.x - p_z0.x - p_x1.z + p_x0.z;
                float z = p_x1.y - p_x0.y - p_y1.x + p_y0.x;

                const float divisor = 1.0 / (2.0 * e);
                return normalize(float3(x, y, z) * divisor);
            }
            
            // 简单的伪随机数生成器，用于添加噪点
            float random (float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 st = i.uv;

                // 计算卷曲噪声，并加入时间作为动画变量
                float3 d3 = curl(float3(st * _PatternScale, _Time.y * _Speed * 0.1)) * _CurlScale + 0.5;

                // 使用噪声的不同通道来控制不同颜色的alpha值
                float4 color_0 = float4(_Color0.rgb, d3.r * _Brightness);
                float4 color_1 = float4(_Color1.rgb, d3.g * _Brightness);
                float4 color_2 = float4(_Color2.rgb, d3.b * _Brightness);
                float4 color_3 = float4(_Color3.rgb, d3.r * _Brightness);
                float4 color_4 = float4(_Color4.rgb, d3.g * _Brightness);
                float4 color_5 = float4(_Color5.rgb, d3.b * _Brightness);

                // 从暗色开始，逐层混合颜色
                float3 finalColor = float3(_Darkness, _Darkness, _Darkness);
                finalColor = lerp(finalColor, color_0.rgb, saturate(color_0.a));
                finalColor = lerp(finalColor, color_1.rgb, saturate(color_1.a));
                finalColor = lerp(finalColor, color_2.rgb, saturate(color_2.a));
                finalColor = lerp(finalColor, color_3.rgb, saturate(color_3.a));
                finalColor = lerp(finalColor, color_4.rgb, saturate(color_4.a));
                finalColor = lerp(finalColor, color_5.rgb, saturate(color_5.a));

                // 添加最后的屏幕颗粒感/噪点
                float noise = (random(i.uv) - 0.5) * _NoiseFactor;
                finalColor += finalColor * noise;

                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}