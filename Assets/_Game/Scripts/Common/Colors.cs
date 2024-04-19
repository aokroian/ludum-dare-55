namespace _Game.Scripts.Common
{
    public static class Colors
    {
        public const string HeroMessageColor = "00ABFF";
        public const string PrincessMessageColor = "00FF33";
        public const string KeywordMessageColor = "FFEF00";
        public const string EnemyMessageColor = "D600FF";
        public const string BardMessageColor = "D7A1A1";

        public static string WrapInColor(this string text, string colorHex)
        {
            return colorHex != null ? $"<color=#{colorHex}>{text}</color>" : text;
        }

        public static string GetSpeakerColor(string speakerId)
        {
            return speakerId switch
            {
                "hero" => HeroMessageColor,
                "princess" => PrincessMessageColor,
                "enemy" => EnemyMessageColor,
                "bard" => BardMessageColor,
                _ => null
            };
        }
    }
}