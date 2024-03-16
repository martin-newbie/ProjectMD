using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Util
{
    public static int[] GetSplitCommaInt(this string args)
    {
        return args.Split(',').Select(item => int.Parse(item)).ToArray();
    }

    public static float[] GetSplitCommaFloat(this string args)
    {
        return args.Split(',').Select(item => float.Parse(item)).ToArray();
    }
}
