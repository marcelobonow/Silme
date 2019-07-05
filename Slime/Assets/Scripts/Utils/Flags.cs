using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags
{
    public static bool HasFlag(Enum a, Enum b)
    {
        if(Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
            return (Convert.ToInt64(a) & Convert.ToInt64(b)) != 0;
        else
            return (Convert.ToUInt64(a) & Convert.ToUInt64(b)) != 0;
    }
}
