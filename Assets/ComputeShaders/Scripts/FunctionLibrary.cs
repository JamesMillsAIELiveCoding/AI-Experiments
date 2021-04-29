using UnityEngine;
using static UnityEngine.Mathf;

public enum Function
{
    Wave,
    MultiWave,
    Ripple,
    Sphere,
    Donut
}

public static class FunctionLibrary
{
    public static Function GetNextFunction(Function _function)
    {
        if(_function == Function.Donut)
        {
            return Function.Wave;
        }
        else
        {
            return (Function)(((int)_function) + 1);
        }
    }

    public static Vector3 GetOffsetByFunction(float _u, float _v, float _t, Function _function)
    {
        switch (_function)
        {
            case Function.Wave: return Wave(_u, _v, _t);
            case Function.MultiWave: return MultiWave(_u, _v, _t);
            case Function.Ripple: return Ripple(_u, _v, _t);
            case Function.Sphere: return Sphere(_u, _v, _t);
            case Function.Donut: return Donut(_u, _v, _t);
            default: return Vector3.one * _u;
        }
    }

    public static Vector3 Wave(float _u, float _v, float _t)
    {
        Vector3 pos;
        pos.x = _u;
        pos.y = Sin(PI * (_u + _v + _t));
        pos.z = _v;

        return pos;
    }

    public static Vector3 MultiWave(float _u, float _v, float _t)
    {
        Vector3 pos;
        pos.x = _u;
        pos.z = _v;

        pos.y = Sin(PI * (_u + 0.5f * _t));
        pos.y += Sin(2f * PI * (_u + (_v + _t))) * 0.5f;
        pos.y += Sin(2f * PI * (_u + _v + .25f + _t));
        pos.y *= (1f / 2.5f);

        return pos;
    }

    public static Vector3 Ripple(float _u, float _v, float _t)
    {
        float d = Sqrt(_u * _u  + _v * _v);

        Vector3 pos;
        pos.x = _u;
        pos.z = _v;

        pos.y = Sin(PI * (4f * d - _t));
        pos.y /= (1f + 10f * d);

        return pos;
    }

    public static Vector3 Sphere(float _u, float _v, float _t)
    {
        float r = .9f + .1f * Sin(PI * (6f * _u + 4f * _v + _t));
        float s = r * Cos(.5f * PI * _v);
        Vector3 pos;

        pos.x = s * Sin(PI * _u);
        pos.y = r * Sin(PI * .5f * _v);
        pos.z = s * Cos(PI * _u);

        return pos;
    }

    public static Vector3 Donut(float _u, float _v, float _t)
    {
        float r1 = .7f + .1f * Sin(PI * (6f * _u + .5f * _t));
        float r2 = .15f + .05f * Sin(PI * (8f * _u + 4f * _v + 2f * _t));
        float s = r1 + r2 * Cos(PI * _v);
        Vector3 pos;

        pos.x = s * Sin(PI * _u);
        pos.y = r2 * Sin(PI * _v);
        pos.z = s * Cos(PI * _u);

        return pos;
    }
}
