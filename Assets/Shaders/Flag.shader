Shader "Custom/Flag" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {

        Tags { "RenderType" = "Opaque" }
        
        CGPROGRAM

        #pragma surface surf Lambert vertex:vert
        //#pragma surface surf Standard fullforwardshadows

        struct Input {
            float2 uv_MainTex;
        };

        // Access the shaderlab properties
        sampler2D _MainTex;

        float _FlagNoiseValues[36];

        // Vertex modifier function
        void vert (inout appdata_full v) {
            v.vertex.z += _FlagNoiseValues[floor(v.vertex.y + 0.5) * 6 + floor(v.vertex.x + 0.5)];
        }

        // Surface shader function
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
}

//thanks to tanoshimi from unity answers!!
