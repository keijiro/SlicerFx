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
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SlicerFx : MonoBehaviour
{
    // Slicer mode (directional or spherical)
    public enum SlicerMode { Directional, Spherical }
    [SerializeField] SlicerMode _mode = SlicerMode.Directional;
    public SlicerMode mode { get { return _mode; } set { _mode = value; } }

    // Slicer direction (used only in the directional mode)
    [SerializeField] Vector3 _direction = Vector3.forward;
    public Vector3 direction { get { return _direction; } set { _direction = value; } }

    // Slicer origin (used only in the spherical mode)
    [SerializeField] Vector3 _origin = Vector3.zero;
    public Vector3 origin { get { return _origin; } set { _origin = value; } }

    // Albedo color (front face)
    [SerializeField] Color _albedoFront = new Color(0.6f, 0.6f, 0.6f, 1);
    public Color albedoFront { get { return _albedoFront; } set { _albedoFront = value; } }

    // Albedo color (back face)
    [SerializeField] Color _albedoBack = new Color(0.4f, 0.4f, 0.4f, 1);
    public Color albedoBack { get { return _albedoBack; } set { _albedoBack = value; } }

    // Emission color
    [SerializeField] Color _emission = new Color(0.2f, 0.2f, 0.2f, 1);
    public Color emission { get { return _emission; } set { _emission = value; } }

    // Interval between stripes
    [SerializeField] float _interval = 0.2f;
    public float interval { get { return _interval; } set { _interval = value; } }

    // Width of stripes
    [SerializeField] float _width = 0.2f;
    public float width { get { return _width; } set { _width = value; } }

    // Scroll speed
    [SerializeField] float _speed = 1.0f;
    public float speed { get { return _speed; } set { _speed = value; } }

    // Reference to the shader.
    [SerializeField] Shader shader;

    // Private shader variables
    int albedo1ID;
    int albedo2ID;
    int emissionID;
    int paramsID;
    int vectorID;

    void Awake()
    {
        albedo1ID  = Shader.PropertyToID("_SlicerAlbedo1");
        albedo2ID  = Shader.PropertyToID("_SlicerAlbedo2");
        emissionID = Shader.PropertyToID("_SlicerEmission");
        paramsID   = Shader.PropertyToID("_SlicerParams");
        vectorID   = Shader.PropertyToID("_SlicerVector");
    }

    void OnEnable()
    {
        camera.SetReplacementShader(shader, null);
        Update();
    }

    void OnDisable()
    {
        camera.ResetReplacementShader();
    }

    void Update()
    {
        Shader.SetGlobalColor(albedo1ID, _albedoFront);
        Shader.SetGlobalColor(albedo2ID, _albedoBack);
        Shader.SetGlobalColor(emissionID, _emission);

        var param = new Vector4(_speed, _interval, _width, 0);
        Shader.SetGlobalVector(paramsID, param);

        if (_mode == SlicerMode.Directional)
        {
            Shader.DisableKeyword("SLICER_SPHERICAL");
            Shader.SetGlobalVector(vectorID, _direction.normalized);
        }
        else
        {
            Shader.EnableKeyword("SLICER_SPHERICAL");
            Shader.SetGlobalVector(vectorID, _origin);
        }
    }
}
