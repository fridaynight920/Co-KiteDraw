Shader "Custom/WindyKite" {
    Properties {
        _WaveCount ("Wave Count", Range(1, 5)) = 3
        _WaveAmplitudes ("Wave Amplitudes", Vector) = (0.1, 0.05, 0.2, 0.0)
        _WaveFrequencies ("Wave Frequencies", Vector) = (1.0, 2.0, 0.5, 0.0)
        _WaveSpeeds ("Wave Speeds", Vector) = (1.0, 0.5, 0.8, 0.0)
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            int _WaveCount;
            float4 _WaveAmplitudes;
            float4 _WaveFrequencies;
            float4 _WaveSpeeds;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                for (int i = 0; i < _WaveCount; i++) {
                    float phase = _Time.y * _WaveSpeeds[i] + v.vertex.x * _WaveFrequencies[i];
                    o.vertex.y += sin(phase) * _WaveAmplitudes[i];
                }
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return fixed4(1.0, 1.0, 1.0, 1.0);
            }
            ENDCG
        }
    }
}
