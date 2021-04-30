Shader "Pixelate"
    {
        Properties
        {
            [NoScaleOffset]_MainTexture("MainTexture", 2D) = "white" {}
            _PixelateAmount("Pixelate Amount", Range(0, 1)) = 0.5
            [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
            _StencilComp ("Stencil Comparison", Float) = 8
            _Stencil ("Stencil ID", Float) = 0
            _StencilOp ("Stencil Operation", Float) = 0
            _StencilWriteMask ("Stencil Write Mask", Float) = 255
            _StencilReadMask ("Stencil Read Mask", Float) = 255
            _ColorMask ("Color Mask", Float) = 15
        }
        SubShader
        {
            Tags
            {
                "RenderPipeline"="UniversalPipeline"
                "RenderType"="Transparent"
                "UniversalMaterialType" = "Lit"
                "Queue"="Transparent"
            }
            Stencil
            {  
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]
            Pass
            {
                Name "Sprite Lit"
                Tags
                {
                    "LightMode" = "Universal2D"
                }
    
                // Render State
                Cull Off
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma exclude_renderers d3d11_9x
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
                #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
                #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
                #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_COLOR
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_COLOR
                #define VARYINGS_NEED_SCREENPOSITION
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SPRITELIT
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 color : COLOR;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float4 texCoord0;
                    float4 color;
                    float4 screenPosition;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float4 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyzw =  input.texCoord0;
                    output.interp1.xyzw =  input.color;
                    output.interp2.xyzw =  input.screenPosition;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.interp0.xyzw;
                    output.color = input.interp1.xyzw;
                    output.screenPosition = input.interp2.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _MainTexture_TexelSize;
                float _PixelateAmount;
                CBUFFER_END
                
                // Object and Global properties
                SAMPLER(SamplerState_Linear_Repeat);
                TEXTURE2D(_MainTexture);
                SAMPLER(sampler_MainTexture);
    
                // Graph Functions
                
                void Unity_Comparison_Equal_float(float A, float B, out float Out)
                {
                    Out = A == B ? 1 : 0;
                }
                
                void Unity_OneMinus_float(float In, out float Out)
                {
                    Out = 1 - In;
                }
                
                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }
                
                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }
                
                void Unity_Floor_float4(float4 In, out float4 Out)
                {
                    Out = floor(In);
                }
                
                void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                    float4 SpriteMask;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    UnityTexture2D _Property_fb5779a1640d4481a977757192aeba31_Out_0 = UnityBuildTexture2DStructNoScale(_MainTexture);
                    float _Property_c85e652d147046cea90e318ea00d3db6_Out_0 = _PixelateAmount;
                    float _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2;
                    Unity_Comparison_Equal_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, 0, _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2);
                    float4 _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0 = IN.uv0;
                    float _Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0 = 200;
                    float _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1;
                    Unity_OneMinus_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1);
                    float _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2;
                    Unity_Multiply_float(_Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1, _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2);
                    float4 _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2;
                    Unity_Multiply_float((_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2);
                    float4 _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1;
                    Unity_Floor_float4(_Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2, _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1);
                    float4 _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2;
                    Unity_Divide_float4(_Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1, (_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2);
                    float4 _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3;
                    Unity_Branch_float4(_Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2, _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2, _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3);
                    float4 _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fb5779a1640d4481a977757192aeba31_Out_0.tex, _Property_fb5779a1640d4481a977757192aeba31_Out_0.samplerstate, (_Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3.xy));
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_R_4 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.r;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_G_5 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.g;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_B_6 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.b;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_A_7 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.a;
                    surface.BaseColor = (_SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.xyz);
                    surface.Alpha = 1;
                    surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS.xyz;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "Sprite Normal"
                Tags
                {
                    "LightMode" = "NormalsRendering"
                }
    
                // Render State
                Cull Off
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma exclude_renderers d3d11_9x
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SPRITENORMAL
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 normalWS;
                    float4 tangentWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.normalWS;
                    output.interp1.xyzw =  input.tangentWS;
                    output.interp2.xyzw =  input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.normalWS = input.interp0.xyz;
                    output.tangentWS = input.interp1.xyzw;
                    output.texCoord0 = input.interp2.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _MainTexture_TexelSize;
                float _PixelateAmount;
                CBUFFER_END
                
                // Object and Global properties
                SAMPLER(SamplerState_Linear_Repeat);
                TEXTURE2D(_MainTexture);
                SAMPLER(sampler_MainTexture);
    
                // Graph Functions
                
                void Unity_Comparison_Equal_float(float A, float B, out float Out)
                {
                    Out = A == B ? 1 : 0;
                }
                
                void Unity_OneMinus_float(float In, out float Out)
                {
                    Out = 1 - In;
                }
                
                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }
                
                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }
                
                void Unity_Floor_float4(float4 In, out float4 Out)
                {
                    Out = floor(In);
                }
                
                void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                    float3 NormalTS;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    UnityTexture2D _Property_fb5779a1640d4481a977757192aeba31_Out_0 = UnityBuildTexture2DStructNoScale(_MainTexture);
                    float _Property_c85e652d147046cea90e318ea00d3db6_Out_0 = _PixelateAmount;
                    float _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2;
                    Unity_Comparison_Equal_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, 0, _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2);
                    float4 _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0 = IN.uv0;
                    float _Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0 = 200;
                    float _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1;
                    Unity_OneMinus_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1);
                    float _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2;
                    Unity_Multiply_float(_Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1, _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2);
                    float4 _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2;
                    Unity_Multiply_float((_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2);
                    float4 _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1;
                    Unity_Floor_float4(_Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2, _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1);
                    float4 _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2;
                    Unity_Divide_float4(_Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1, (_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2);
                    float4 _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3;
                    Unity_Branch_float4(_Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2, _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2, _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3);
                    float4 _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fb5779a1640d4481a977757192aeba31_Out_0.tex, _Property_fb5779a1640d4481a977757192aeba31_Out_0.samplerstate, (_Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3.xy));
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_R_4 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.r;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_G_5 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.g;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_B_6 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.b;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_A_7 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.a;
                    surface.BaseColor = (_SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.xyz);
                    surface.Alpha = 1;
                    surface.NormalTS = IN.TangentSpaceNormal;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS.xyz;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "Sprite Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }
    
                // Render State
                Cull Off
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma exclude_renderers d3d11_9x
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_COLOR
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_COLOR
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SPRITEFORWARD
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 color : COLOR;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float4 texCoord0;
                    float4 color;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float4 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyzw =  input.texCoord0;
                    output.interp1.xyzw =  input.color;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.interp0.xyzw;
                    output.color = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _MainTexture_TexelSize;
                float _PixelateAmount;
                CBUFFER_END
                
                // Object and Global properties
                SAMPLER(SamplerState_Linear_Repeat);
                TEXTURE2D(_MainTexture);
                SAMPLER(sampler_MainTexture);
    
                // Graph Functions
                
                void Unity_Comparison_Equal_float(float A, float B, out float Out)
                {
                    Out = A == B ? 1 : 0;
                }
                
                void Unity_OneMinus_float(float In, out float Out)
                {
                    Out = 1 - In;
                }
                
                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }
                
                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }
                
                void Unity_Floor_float4(float4 In, out float4 Out)
                {
                    Out = floor(In);
                }
                
                void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                    float3 NormalTS;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    UnityTexture2D _Property_fb5779a1640d4481a977757192aeba31_Out_0 = UnityBuildTexture2DStructNoScale(_MainTexture);
                    float _Property_c85e652d147046cea90e318ea00d3db6_Out_0 = _PixelateAmount;
                    float _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2;
                    Unity_Comparison_Equal_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, 0, _Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2);
                    float4 _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0 = IN.uv0;
                    float _Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0 = 200;
                    float _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1;
                    Unity_OneMinus_float(_Property_c85e652d147046cea90e318ea00d3db6_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1);
                    float _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2;
                    Unity_Multiply_float(_Float_ec3267f25e1041f28dc01ccf2b41ebd7_Out_0, _OneMinus_e9c0f85fed0d4f6da13f6c572d6cab0a_Out_1, _Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2);
                    float4 _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2;
                    Unity_Multiply_float((_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2);
                    float4 _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1;
                    Unity_Floor_float4(_Multiply_1abe13de94324d13b5f0d58e647ebb05_Out_2, _Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1);
                    float4 _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2;
                    Unity_Divide_float4(_Floor_c5bfbe4f987d4153a41ceb6cffa9b2d1_Out_1, (_Multiply_82cc2d5e9aff4f469ea7c709dd147f12_Out_2.xxxx), _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2);
                    float4 _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3;
                    Unity_Branch_float4(_Comparison_0a72efe57f2a41538a29edd7957d703d_Out_2, _UV_3e3d108dbfc54e569ef083f17f04282c_Out_0, _Divide_e2570cb3be184e989cf5e8961c1ea5b2_Out_2, _Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3);
                    float4 _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fb5779a1640d4481a977757192aeba31_Out_0.tex, _Property_fb5779a1640d4481a977757192aeba31_Out_0.samplerstate, (_Branch_ce7a2b9457854ce18fe6a9cec384e900_Out_3.xy));
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_R_4 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.r;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_G_5 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.g;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_B_6 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.b;
                    float _SampleTexture2D_258de05df5684ff78db19c80703f052b_A_7 = _SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.a;
                    surface.BaseColor = (_SampleTexture2D_258de05df5684ff78db19c80703f052b_RGBA_0.xyz);
                    surface.Alpha = 1;
                    surface.NormalTS = IN.TangentSpaceNormal;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS.xyz;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"
    
                ENDHLSL
            }
        }
        FallBack "Hidden/Shader Graph/FallbackError"
    }