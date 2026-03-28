namespace Homework2;

public static class CarInputParser
{
    public static bool TryParseCarType(string? input, out CarType type)
    {
        type = default;
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        string trimmed = input.Trim();
        if (Enum.TryParse(trimmed, true, out type))
        {
            return true;
        }

        string key = new string(trimmed.Where(c => !char.IsWhiteSpace(c) && c != '-').ToArray())
            .ToLowerInvariant();

        (string Key, CarType Type)[] map =
        {
            ("tesla", CarType.Tesla),
            ("mercedeseclass", CarType.MercedesEClass),
            ("mercedes", CarType.MercedesEClass),
            ("ladavesta", CarType.LadaVesta),
            ("lada", CarType.LadaVesta),
            ("porschetaycan", CarType.PorscheTaycan),
            ("porsche", CarType.PorscheTaycan),
            ("fordfocus", CarType.FordFocus),
            ("ford", CarType.FordFocus),
            ("mazdamx5", CarType.MazdaMx5),
            ("mazda", CarType.MazdaMx5)
        };

        foreach ((string k, CarType t) in map)
        {
            if (string.Equals(key, k, StringComparison.Ordinal))
            {
                type = t;
                return true;
            }
        }

        return false;
    }
}
