using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JC.Utilities.Extensions
{
    /// <summary>
    /// Represents the System.Drawing.Color extensions.
    /// </summary>
    public class ColorExtensions
    {
        /// <summary>
        /// The color list from system definition.
        /// </summary>
        public static List<Color> Colors = new List<Color>()
        {
            Color.Red,
            Color.IndianRed,
            Color.OrangeRed,
            Color.Blue,
            Color.Purple,
            Color.Pink,
            Color.Lime,
            Color.Orange,
            Color.Yellow,
            Color.YellowGreen,
            Color.DarkRed,
            Color.AliceBlue,
            Color.DarkBlue,
            Color.LightYellow,
            Color.LightBlue,
            Color.LightPink,
            Color.MediumBlue
        };
        private static Random rd = new Random();
        /// <summary>
        /// Gets a random System.Drawing.Color without specified except colors.
        /// </summary>
        /// <param name="exceptColors">The except color to not get.</param>
        /// <returns>The random color.</returns>
        public static Color GetRandomColor(params Color[] exceptColors)
        {
            var listColor = Colors.ToList(); //copy
            if (exceptColors != null && exceptColors.Any())
            {
                listColor.RemoveAll(th => exceptColors.Contains(th));
            }
            if (listColor.Any())
            {
                int rdIndex = rd.Next(1, listColor.Count) - 1;
                return listColor[rdIndex];
            }
            return Color.Black;
        }
        /// <summary>
        /// Converts the Hexadecimal string color to <see cref="Color"/>. The hexadecimal is like #FFFFFF or FFFFFF.
        /// </summary>
        /// <param name="colorHex">The hexadecimal color string to convert.</param>
        /// <returns>The color result.</returns>
        public static Color ConvertFromHexString(string colorHex)
        {
            try
            {
                colorHex = colorHex?.TrimStart('#');
                if (colorHex?.Length != 6)
                {
                    throw new ArgumentException(colorHex);
                }
                var strR = colorHex.Substring(0, 2);
                var strG = colorHex.Substring(2, 2);
                var strB = colorHex.Substring(4, 2);
                var r = int.Parse(strR, System.Globalization.NumberStyles.HexNumber);
                var g = int.Parse(strG, System.Globalization.NumberStyles.HexNumber);
                var b = int.Parse(strB, System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                throw new InvalidOperationException($"Cannot convert '{colorHex}' string to color.");
            }
        }
        /// <summary>
        /// Converts the rgb string color to <see cref="Color"/>. The int string color is like "255,255,255" by ',' separator character.
        /// </summary>
        /// <param name="colorRGB">The rgb color string.</param>
        /// <returns>The color result.</returns>
        public static Color ConvertFromRGBString(string colorRGB)
        {
            try
            {
                var arrRGB = colorRGB.Split(',');
                if (arrRGB.Length != 3)
                {
                    throw new ArgumentException(colorRGB);
                }
                int r = Convert.ToInt32(arrRGB[0]);
                int g = Convert.ToInt32(arrRGB[1]);
                int b = Convert.ToInt32(arrRGB[2]);
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                throw new InvalidOperationException($"Cannot convert '{colorRGB}' string to color.");
            }
        }
        /// <summary>
        /// Converts the argb to <see cref="Color"/>.
        /// </summary>
        /// <param name="argb">The argb.</param>
        /// <returns>The color result.</returns>
        public static Color ConvertFromArgb(int argb)
        {
            return Color.FromArgb(argb);
        }
        /// <summary>
        /// Converts the argb color to Hexadecimal string. The return value is like '#FFFFFF'.
        /// </summary>
        /// <param name="argb">The argb.</param>
        /// <returns>The color result.</returns>
        public static string ConvertToHexFromArgb(int argb)
        {
            var color = Color.FromArgb(argb);
            return ConvertToHex(color);
        }
        /// <summary>
        /// Converts the hexadecimal color to argb. The hexadecimal color is '#FFFFFF' or 'FFFFFF'.
        /// </summary>
        /// <param name="colorHex">The hexadecimal color.</param>
        /// <param name="emptyValueAsBlack">Wheter return as black color when it's empty value or null.</param>
        /// <returns>The argb result.</returns>
        public static int ConvertToArgbFromHex(string colorHex, bool emptyValueAsBlack = false)
        {
            if (emptyValueAsBlack && string.IsNullOrEmpty(colorHex))
            {
                return Color.Black.ToArgb();
            }
            var color = ConvertFromHexString(colorHex);
            return color.ToArgb();
        }
        /// <summary>
        /// Try to convert the specified color name to system color.
        /// </summary>
        /// <param name="colorName">The specified color name.</param>
        /// <param name="color">The ouput system color.</param>
        /// <returns>True when success; false when failure.</returns>
        public static bool TryConvertFromName(string colorName, out Color color)
        {
            try
            {
                color = Color.FromName(colorName);
                return true;
            }
            catch
            {
                color = Color.Black;
                return false;
            }
        }
        /// <summary>
        /// Converts the system color to hexadecimal string. The hexadecimal string is as '#FFFFFF';
        /// </summary>
        /// <param name="color">The system color.</param>
        /// <returns>The hexadecimal string.</returns>
        public static string ConvertToHex(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
