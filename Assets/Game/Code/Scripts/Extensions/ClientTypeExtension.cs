using UnityEngine;

namespace Extensions
{
    public static class ClientTypeExtension
    {
        public static float GetTimer(this ClientType @type)
        {
            return @type switch
            {
                ClientType.CHILL => 600,
                ClientType.NORMAL => 300,
                ClientType.RUSH => 60,
                ClientType.KAREN => 10,
                _ => 300
            };
        }

        public static float GetDelayBetweenItems(this ClientType @type)
        {
            return @type switch
            {
                ClientType.CHILL => 5,
                ClientType.NORMAL => 3,
                ClientType.RUSH => 1,
                ClientType.KAREN => 0.5f,
                _ => 3
            };
        }

        public static Color GetColor(this ClientType @type)
        {
            return @type switch
            {
                ClientType.CHILL => Color.green,
                ClientType.NORMAL => Color.blue,
                ClientType.RUSH => Color.red,
                ClientType.KAREN => Color.magenta,
                _ => Color.blue
            };
        }
    }
}