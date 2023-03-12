// Base found on https://lindenreid.wordpress.com/2018/01/07/waving-grass-shader-in-unity/
// Had to change many details to make it work. This asset should help people to save their time
// for debugging and experimenting. I also added some improvements like using an emission color.
// This needs a ramp and a gradient texture to work.
Shader "Custom/GrassWind" {
	Properties {
		_RampTex("Ramp", 2D) = "white" {} // Horizontal ramps
		_Color("Color", Color) = (1, 1, 1, 1)
        _WaveSpeed("Wave Speed", float) = 7.0
        _WaveAmp("Wave Amp", float) = 1.2
        _HeightFactor("Height Factor", float) = 1.5
		_HeightCutoff("Height Cutoff", float) = 0.1
        _WindTex("Wind Texture", 2D) = "white" {} // Horizontal repeatable gray scale gradient
        _WorldSize("World Size", vector) = (40, 40, 0, 0)   // Use only x and y (y is z in 3D space)
        _WindSpeed("Wind Speed", vector) = (1.5, 1.5, 0, 0) // Use only x and y
        _YOffset("Y offset", float) = 0.0 // y offset, below this is no animation
        _MaxWidth("Max Displacement Width", Range(0, 2)) = 0.1 // width of the line around the dissolve
        _Radius("Radius", Range(0,5)) = 1 // width of the line around the dissolve
        _Brightness("Brightness", Range(0,20)) = 1.8 // Brightness factor
        _Emission("Emission", Color) = (0, 0, 0, 0) // Emission color
		// _DebugBuffer("Debug Buffer", RWStructuredBuffer<float4>) = null
	}

	SubShader {
		Pass {
            Tags {
                "DisableBatching" = "True"
            }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase // Shadows
			// #pragma target 4.5 // Needed for debugging, can be removed
            #include "UnityCG.cginc"
			
			// RWStructuredBuffer<float4> debugBuffer : register(u1); // Needed for debugging, can be removed

			// Properties
			sampler2D _RampTex;
            sampler2D _WindTex;
            float4 _WindTex_ST;
			float4 _Color;
			float4 _LightColor0; // Provided by Unity
            float4 _WorldSize;
            float _WaveSpeed;
            float _WaveAmp;
            float _HeightFactor;
			float _HeightCutoff;
            float2 _WindSpeed;

			float _MaxWidth;
			float _Radius;
			float _YOffset;

			float _Brightness;
			float4 _Emission;
			
			uniform float3 _Positions[100];
			uniform float _PositionArray;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
                // float2 sp : TEXCOORD0; // Test sample position by making it visible as color
			};

			vertexOutput vert(vertexInput input) {
				vertexOutput output;

				// Convert input to clip & world space
				output.pos = UnityObjectToClipPos(input.vertex);
				float4 normal4 = float4(input.normal, 0.0);
				output.normal = normalize(mul(normal4, unity_WorldToObject).xyz);

                // Get vertex world position
                float4 worldPos = mul(unity_ObjectToWorld, input.vertex);

                // Normalize position based on world size
                float2 samplePos = worldPos.xz/_WorldSize.xy;
                // Scroll sample position based on time
                samplePos += _Time.x * _WindSpeed.xy;
				samplePos = float2(fmod(samplePos.x, 1), fmod(samplePos.y, 1)).xyxy;

                // Sample wind texture
                float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));
                
				// output.sp = samplePos; // Test sample position by making it visible as color

                // No animation below _HeightCutoff
                float heightFactor = input.vertex.y > _HeightCutoff;
				// Make animation stronger with height
				heightFactor = heightFactor * pow(abs(input.vertex.y), _HeightFactor);

                // Apply wave animation
                // output.pos.z += (sin(_WaveSpeed*windSample)*_WaveAmp * heightFactor);
				float interactionFactor;
				if (UNITY_MATRIX_P[3][3] == 1) { // Orthographic
					output.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp * heightFactor / 10;
					interactionFactor = 0.5;
				} else { // With perspective
					output.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp * heightFactor;
					interactionFactor = 4;
				}

				// Interaction radius movement for every position in array
			    for (int i = 0; i < _PositionArray; i++){
					float3 dis = distance(_Positions[i], worldPos); // Distance for radius
					float3 radius = 1 - saturate(dis / _Radius); // In world radius based on objects interaction radius
					float3 sphereDisp = worldPos - _Positions[i]; // Position comparison
					sphereDisp *= radius; // Position multiplied by radius for falloff

					// Vertex movement based on falloff and clamped
					output.pos.x += /*float2(interactionFactor, 1)*/ interactionFactor * clamp(sphereDisp.x/*z*/ /** step(_YOffset, output.pos.y)*/, -_MaxWidth, _MaxWidth);
				}

				return output;
			}

			float4 frag(vertexOutput input) : COLOR {
				// Normalize light dir
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				// Apply lighting
				float ramp = clamp(dot(input.normal, lightDir), 0.001, 1.0);
				float3 lighting = tex2D(_RampTex, float2(ramp, 0.5)).rgb;
				
                // return float4(frac(input.sp.x), 0, 0, 1); // Test sample position by making it visible as color
				
				float3 rgb = _LightColor0.rgb * _Brightness * lighting * _Color.rgb + _Emission.xyz;
				return float4(rgb, 1.0);
			}

			ENDCG
		}

	}
}
