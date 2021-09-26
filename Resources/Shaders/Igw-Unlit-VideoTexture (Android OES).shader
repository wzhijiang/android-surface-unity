Shader "Igw/Unlit/VideoTexture (texture+color support) - Android OES ONLY"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "black" {}

        [KeywordEnum(None, Top_Bottom, Left_Right)] Stereo("Stereo Mode", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "IgnoreProjector" = "False" "Queue" = "Geometry" }
        LOD 100
        Lighting Off
        Cull Off

        Pass
        {
            GLSLPROGRAM

            #pragma only_renderers gles gles3
            #extension GL_OES_EGL_image_external : require
            #extension GL_OES_EGL_image_external_essl3 : enable
            precision mediump float;

            #ifdef VERTEX

            #include "UnityCG.glslinc"
            #define SHADERLAB_GLSL

            varying vec2 texVal;
            uniform vec4 _MainTex_ST;

            /// @fix: explicit TRANSFORM_TEX(); Unity's preprocessor chokes when attempting to use the TRANSFORM_TEX() macro in UnityCG.glslinc
            ///     (as of Unity 4.5.0f6; issue dates back to 2011 or earlier: http://forum.unity3d.com/threads/glsl-transform_tex-and-tiling.93756/)
            vec2 transformTex(vec4 texCoord, vec4 texST)
            {
                return (texCoord.xy * texST.xy + texST.zw);
            }

            void main()
            {
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                texVal = transformTex(gl_MultiTexCoord0, _MainTex_ST);
                texVal.y = 1.0 - texVal.y;
            }
            #endif  

            #ifdef FRAGMENT

            varying vec2 texVal;

            uniform samplerExternalOES _MainTex;

            vec3 gammaToLinear(vec3 v)
            {
                const float gamma = 2.2;
                return pow(v, vec3(gamma));
            }

            void main()
            {
	    #if defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
                vec4 col = texture2D(_MainTex, texVal.xy);
		
		// If color space is Linear, the line below is required.
                col.xyz = gammaToLinear(col.xyz);
		
                gl_FragColor = col;
	    #else
                gl_FragColor = vec4(1.0, 1.0, 0.0, 1.0);
	    #endif
                }
            #endif

            ENDGLSL
        }
    }

    FallBack "Diffuse", 1
}