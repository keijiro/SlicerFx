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
        _SlicerAlbedo   ("Albedo",     Color)  = (0.1, 0.1, 0.1, 0)
        _SlicerEmission ("Emission",   Color)  = (0, 0, 0, 0)
        _SlicerParams   ("Parameters", Vector) = (1, 20, 20, 10)
        _SlicerVector   ("Vector",     Vector) = (0, 0, 1, 0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off

        CGPROGRAM

        #pragma surface surf Lambert addshadow
        #pragma multi_compile SLICER_DIRECTIONAL SLICER_SPHERICAL

        struct Input
        {
            float3 worldPos;
        };

        float3 _SlicerAlbedo;
        float3 _SlicerEmission;
        float4 _SlicerParams; // Speed, Interval, Threshold
        float3 _SlicerVector;

        void surf(Input IN, inout SurfaceOutput o)
        {
#ifdef SLICER_DIRECTIONAL
            float w = dot(IN.worldPos, _SlicerVector);
#else
            float w = length(IN.worldPos - _SlicerVector);
#endif

            // Moving wave.
            w -= _Time.y * _SlicerParams.x;

            // Divide with the interval.
            w = mod(w, _SlicerParams.y) / _SlicerParams.y;

            // Clip with the threshold.
            clip(_SlicerParams.z - w);

            // Apply to the surface.
            o.Albedo = _SlicerAlbedo;
            o.Emission = _SlicerEmission;
        }

        ENDCG
    } 
    Fallback "Diffuse"
}
