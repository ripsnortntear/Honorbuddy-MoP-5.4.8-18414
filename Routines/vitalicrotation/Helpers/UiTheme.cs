using System;
using System.Drawing;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    public class ThemePalette
    {
        public Color Back;
        public Color Panel;
        public Color Fore;
        public Color Accent;
        public Color AccentText;
        public Color Border;
    }

    public enum UIColorStyleEnum
    {
        Dark = 0,
        Light = 1,
        Crimson = 2,
        Emerald = 3
    }

    public static class UiTheme
    {
        /// <summary>
        /// Resolves the theme palette from the settings value
        /// </summary>
        public static ThemePalette Resolve(int colorStyleValue)
        {
            var style = (UIColorStyleEnum)colorStyleValue;
            return Resolve(style);
        }

        /// <summary>
        /// Resolves the theme palette from the enum
        /// </summary>
        public static ThemePalette Resolve(UIColorStyleEnum style)
        {
            switch (style)
            {
                case UIColorStyleEnum.Light:
                    return new ThemePalette
                    {
                        Back = Color.FromArgb(245, 246, 248),
                        Panel = Color.FromArgb(255, 255, 255),
                        Fore = Color.FromArgb(30, 32, 35),
                        Accent = Color.FromArgb(0, 122, 204),
                        AccentText = Color.White,
                        Border = Color.FromArgb(224, 224, 224)
                    };
                case UIColorStyleEnum.Crimson:
                    return new ThemePalette
                    {
                        Back = Color.FromArgb(18, 18, 20),
                        Panel = Color.FromArgb(28, 28, 32),
                        Fore = Color.FromArgb(232, 232, 236),
                        Accent = Color.FromArgb(220, 53, 69),
                        AccentText = Color.White,
                        Border = Color.FromArgb(60, 60, 66)
                    };
                case UIColorStyleEnum.Emerald:
                    return new ThemePalette
                    {
                        Back = Color.FromArgb(19, 22, 22),
                        Panel = Color.FromArgb(28, 36, 34),
                        Fore = Color.FromArgb(222, 235, 229),
                        Accent = Color.FromArgb(16, 185, 129),
                        AccentText = Color.Black,
                        Border = Color.FromArgb(46, 64, 60)
                    };
                case UIColorStyleEnum.Dark:
                default:
                    return new ThemePalette
                    {
                        Back = Color.FromArgb(20, 21, 25),
                        Panel = Color.FromArgb(30, 32, 36),
                        Fore = Color.FromArgb(230, 233, 239),
                        Accent = Color.FromArgb(98, 0, 238),
                        AccentText = Color.White,
                        Border = Color.FromArgb(55, 58, 64)
                    };
            }
        }
    }
}