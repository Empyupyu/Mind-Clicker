using System;
using System.Text;

public static class FloatAbbreviationExtensions
{
    public static string ToAbbreviatedString(this float value, int decimalPlaces = 1)
    {
        if (float.IsNaN(value)) return "NaN";
        if (float.IsInfinity(value)) return "∞";

        if (Math.Abs(value) < 1000f)
            return value.ToString($"F{decimalPlaces}");

        int tier = (int)Math.Floor(Math.Log10(Math.Abs(value)) / 3); // каждые 1000
        float scaled = value / (float)Math.Pow(1000, tier);

        string suffix = GetSuffix(tier);

        return scaled.ToString($"F{decimalPlaces}") + suffix;
    }

    private static string GetSuffix(int tier)
    {
        if (tier == 0) return "";

        string[] standard = { "K", "M", "B", "T" };
        if (tier <= standard.Length)
            return standard[tier - 1];

        tier -= standard.Length;

        var sb = new StringBuilder();
        while (tier > 0)
        {
            tier--;
            sb.Insert(0, (char)('A' + (tier % 26)));
            tier /= 26;
        }
        return sb.ToString();
    }
}

