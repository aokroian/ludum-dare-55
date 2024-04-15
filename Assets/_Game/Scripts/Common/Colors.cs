namespace _Game.Scripts.Common
{
    public static class Colors
    {
        public const string HeroMessageColor = "00ABFF";
        public const string PrincessMessageColor = "00FF33";
        public const string KeywordMessageColor = "FFEF00";
        public const string EnemyMessageColor = "D600FF";

        public static string WrapInColor(this string text, string colorHex)
        {
            return $"<color=#{colorHex}>{text}</color>";
        }

        public static string GetSpeakerColor(string speakerId)
        {
            switch (speakerId)
            {
                case "hero":
                    return HeroMessageColor;
                case "princess":
                    return PrincessMessageColor;
                case "enemy":
                    return EnemyMessageColor;
                default:
                    return "FFFFFF";
            }
        }
    }
}