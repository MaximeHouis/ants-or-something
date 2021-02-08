using System;
using Color = UnityEngine.Color;

public enum AntClass
{
    Worker,
    Soldier,
    Breeder,
    Scout
}

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
}