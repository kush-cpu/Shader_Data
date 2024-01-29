// Shader Graph version for Unity URP

Shader "Custom/TransparentFresnel" {
    Properties {
        _Color ("Color", Color) = (.5,.5,.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 5.0
    }
    SubShader {
        Tags {"Queue"="Overlay" "RenderType"="Transparent"}
        LOD 100

        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB

        Pass {
            Name "FORWARD"
            Tags {"LightMode"="ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            uniform float _Glossiness;
            uniform float _Metallic;
            uniform float _FresnelPower;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 _Color;
            sampler2D _MainTex;

            fixed4 frag(v2f i) : COLOR {
                fixed4 c = tex2D(_MainTex, i.pos.xy / i.pos.w);
                fixed3 normal = normalize(i.normal);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.pos));

                // Fresnel effect
                float fresnel = pow(1.0 - dot(normal, viewDir), _FresnelPower);

                // Combine color with Fresnel effect
                fixed4 finalColor = c * _Color;
                finalColor.rgb *= fresnel;

                // Apply alpha
                finalColor.a *= c.a;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
