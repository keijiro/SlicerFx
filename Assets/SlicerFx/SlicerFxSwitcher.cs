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

[RequireComponent(typeof(SlicerFx))]
public class SlicerFxSwitcher : MonoBehaviour
{
    public Gradient albedoFront;
    public Gradient albedoBack;
    public Gradient emission;
    public AnimationCurve width = AnimationCurve.Linear(0, 1, 1, 0.2f);
    public float switchSpeed = 5;

    SlicerFx fx;
    float parameter;
    float target;

    public bool state {
        get { return target > 0.0f; }
        set { target = state ? 1.0f : 0.0f; }
    }

    void Awake()
    {
        fx = GetComponent<SlicerFx>();
    }

    void Update()
    {
        if (parameter < target)
        {
            parameter = Mathf.Min(1.0f, parameter + switchSpeed * Time.deltaTime);
            fx.enabled = true;
        }
        else if (parameter > target)
        {
            parameter = Mathf.Max(0.0f, parameter - switchSpeed * Time.deltaTime);
            if (parameter == 0.0f) fx.enabled = false;
        }

        if (parameter > 0.0f)
        {
            fx.albedoFront = albedoFront.Evaluate(parameter);
            fx.albedoBack = albedoBack.Evaluate(parameter);
            fx.emission = emission.Evaluate(parameter);
            fx.width = width.Evaluate(parameter);
        }
    }

    public void Toggle()
    {
        target = target > 0.0f ? 0.0f : 1.0f;
    }
}
