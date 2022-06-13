using UnityEngine;

namespace apperture.editor
{
    public enum DeviceType
    {
        IPAD_MINI,
        IPAD_AIR,
        IPAD_PRO,
        LANDSCAPE_SAMSUNG_S8,
        PORTRAIT_SAMSUNG_S8,
        LANDSCAPE_FHD,
        PORTRAIT_FHD,
        LANDSCAPE_4K,
        PORTRAIT_4K
    }

    public static class DeviceUtility
    {
        public static Vector2 GetSize(DeviceType fromDevice)
        {
            Vector2 screenSize = Vector2.zero;
            switch (fromDevice)
            {
                case DeviceType.IPAD_MINI:
                case DeviceType.IPAD_AIR:
                    screenSize = new Vector2(2048f, 1536f);
                    break;
                case DeviceType.IPAD_PRO:
                    screenSize = new Vector2(2732f, 2048f);
                    break;
                case DeviceType.LANDSCAPE_SAMSUNG_S8:
                    screenSize = new Vector2(2960f, 1440f);
                    break;
                case DeviceType.PORTRAIT_SAMSUNG_S8:
                    screenSize = new Vector2(1440f, 2960f);
                    break;
                case DeviceType.LANDSCAPE_FHD:
                    screenSize = new Vector2(1920f, 1080f);
                    break;
                case DeviceType.PORTRAIT_FHD:
                    screenSize = new Vector2(1080f, 1920f);
                    break;
                case DeviceType.LANDSCAPE_4K:
                    screenSize = new Vector2(3840f, 2160f);
                    break;
                case DeviceType.PORTRAIT_4K:
                    screenSize = new Vector2(2160f, 3840f);
                    break;
                default:
                    break;
            }
            return screenSize;
        }
    }
}
