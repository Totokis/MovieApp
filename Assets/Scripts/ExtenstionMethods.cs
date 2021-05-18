internal static class ExtenstionMethods
{
    public static string FormatNumber(int num)
    {
        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";
        if (num >= 10000)
            return (num / 1000D).ToString("0.#") + "K";

        return num.ToString("#,0");
    }
}
