using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    // TODO: Customizable from Unity
    public static Color Color(this AntClass antClass)
    {
        switch (antClass)
        {
            case AntClass.Worker:
                return Utils.ColorFromInt(0xE8C547FF);
            case AntClass.Soldier:
                return Utils.ColorFromInt(0x5C80BCFF);
            case AntClass.Breeder:
                return Utils.ColorFromInt(0xDB5461FF);
            case AntClass.Scout:
                return Utils.ColorFromInt(0xCDD1C4FF);
            default:
                throw new ArgumentOutOfRangeException(nameof(antClass), antClass, null);
        }
    }

    public static T Last<T>(this List<T> list)
    {
        var size = list.Count;

        if (size == 0)
            throw new IndexOutOfRangeException("List is empty");
        return list[size - 1];
    }
}