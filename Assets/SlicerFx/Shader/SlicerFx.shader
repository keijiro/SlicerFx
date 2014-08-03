//
// Slicer FX
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
Shader "Hidden/SlicerFX"
{
    Properties
    {
        _SlicerAlbedo1  ("Albedo (front)", Color)  = (0.1, 0.1, 0.1, 0)
        _SlicerAlbedo2  ("Albedo (back)",  Color)  = (0.1, 0.1, 0.1, 0)
        _SlicerEmission ("Emission",       Color)  = (0, 0, 0, 0)
        _SlicerParams   ("Parameters",     Vector) = (1, 20, 20, 10)
        _SlicerVector   ("Vector",         Vector) = (0, 0, 1, 0)
    }

    CGINCLUDE

    //
    // Slicer clipping function
    //
    // params: [Speed, Interval, Threshold, 0]
    //

    void slicer_clip(float3 pos, float3 vec, float4 params)
    {
#ifdef SLICER_DIRECTIONAL
        float w = dot(pos, vec);
#else
        float w = length(pos - vec);
#endif
        // Moving wave.
        w -= _Time.y * params.x;

        // Divide with the interval.
        w = mod(w, params.y) / params.y;

        // Clip with the threshold.
        clip(params.z - w);
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        //
        // First pass: draw front faces with Albedo1
        //

        Cull Back

        CGPROGRAM

        #pragma surface surf Lambert addshadow
        #pragma multi_compile SLICER_DIRECTIONAL SLICER_SPHERICAL

        struct Input
        {
            float3 worldPos;
        };

        half3 _SlicerAlbedo1;
        half3 _SlicerEmission;
        float4 _SlicerParams;
        float3 _SlicerVector;

        void surf(Input IN, inout SurfaceOutput o)
        {
            slicer_clip(IN.worldPos, _SlicerVector, _SlicerParams);
            o.Albedo = _SlicerAlbedo1;
            o.Emission = _SlicerEmission;
        }

        ENDCG

        //
        // Second pass: draw back faces with Albedo2
        //

        Cull Front

        CGPROGRAM

        #pragma surface surf ReversedLambert addshadow
        #pragma multi_compile SLICER_DIRECTIONAL SLICER_SPHERICAL

        // Lambert lighting function with reversed normal.
        half4 LightingReversedLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            half d = dot(-s.Normal, lightDir) * atten * 2;
            return half4(s.Albedo * _LightColor0.rgb * d, s.Alpha);
        }

        struct Input
        {
            float3 worldPos;
        };

        half3 _SlicerAlbedo2;
        half3 _SlicerEmission;
        float4 _SlicerParams;
        float3 _SlicerVector;

        void surf(Input IN, inout SurfaceOutput o)
        {
            slicer_clip(IN.worldPos, _SlicerVector, _SlicerParams);
            o.Albedo = _SlicerAlbedo2;
            o.Emission = _SlicerEmission;
        }

        ENDCG
    } 
    Fallback "Diffuse"
}
