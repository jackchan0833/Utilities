using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Ctrl = System.Windows.Media;
using JC.Utilities.Extensions;

namespace JC.Utilities.WPF.Extensions
{
    /// <summary>
    /// Represents the WPF color extensions.
    /// </summary>
    public static class WPFColorExtensions
    {
        /// <summary>
        /// Converts the color to Hexadecimal string. Example: '#FFFFFF'.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal string result.</returns>
        public static string ConvertToHexString(this Ctrl.Color color)
        {
            int red = color.R;
            int green = color.G;
            int blue = color.B;
            var hexString = string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
            return hexString;
        }
        /// <summary>
        /// Converts the color to Hexadecimal string. Example: '#FFFFFF'.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal string result.</returns>
        public static string ConvertToHexString(this Ctrl.SolidColorBrush color)
        {
            return ConvertToHexString(color.Color);
        }
        /// <summary>
        /// Converts the color to Hexadecimal string. Example: '#FFFFFF'.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal string result.</returns>
        public static string ConvertToHexString(this Ctrl.Brush color)
        {
            return ConvertToHexString(((Ctrl.SolidColorBrush)color).Color);
        }
        /// <summary>
        /// Converts hexadecimal string to the color.
        /// </summary>
        /// <param name="hexString">The hexadecimal color string.</param>
        /// <param name="emptyValueAsBlackColor">Whether return as black color if it's null or empty value.</param>
        /// <returns>The WPF color.</returns>
        public static Ctrl.Color ConvertToControlColor(this string hexString, bool emptyValueAsBlackColor = false)
        {
            if (emptyValueAsBlackColor && string.IsNullOrEmpty(hexString))
            {
                return Ctrl.Colors.Black;
            }
            var color = ColorExtensions.ConvertFromHexString(hexString);
            return Ctrl.Color.FromRgb(color.R, color.G, color.B);
        }
        /// <summary>
        /// Converts hexadecimal string to the color brush.
        /// </summary>
        /// <param name="hexString">The hexadecimal color string.</param>
        /// <param name="emptyValueAsBlackColor">Whether return as black color if it's null or empty value.</param>
        /// <returns>The SolidColorBrush.</returns>
        public static Ctrl.SolidColorBrush ConvertToControlColorBrush(this string hexString, bool emptyValueAsBlackColor = false)
        {
            if (emptyValueAsBlackColor && string.IsNullOrEmpty(hexString))
            {
                return new Ctrl.SolidColorBrush(Ctrl.Colors.Black);
            }
            var ctrlColor = ConvertToControlColor(hexString);
            return new Ctrl.SolidColorBrush(ctrlColor);
        }

        #region all system colors
        private static List<Ctrl.Color> _Colors = new List<Ctrl.Color>();
        /// <summary>
        /// Gets all WPF colors from system implemented.
        /// </summary>
        /// <returns></returns>
        public static List<Ctrl.Color> GetAllControlColors()
        {
            if (_Colors.Count > 0) return _Colors;

            #region colors from system
            List<Color> colors = new List<Color>();
            //
            // 摘要:
            //     AliceBlue, the color that is represented by the RGB value #FFF0F8FF.
            colors.Add(Color.AliceBlue); //Color(240, 248, 255);

            //
            // 摘要:
            //     AntiqueWhite, the color that is represented by the RGB value #FFFAEBD7.
            colors.Add(Color.AntiqueWhite); //Color(250, 235, 215);

            //
            // 摘要:
            //     Aqua, the color that is represented by the RGB value #00ffff.
            colors.Add(Color.Aqua); //Color(0, 255, 255);

            //
            // 摘要:
            //     Aquamarine, the color that is represented by the RGB value #FF7FFFD4.
            colors.Add(Color.Aquamarine); //Color(127, 255, 212);

            //
            // 摘要:
            //     Azure, the color that is represented by the RGB value #FFF0FFFF.
            colors.Add(Color.Azure); //Color(240, 255, 255);

            //
            // 摘要:
            //     Beige, the color that is represented by the RGB value #FFF5F5DC.
            colors.Add(Color.Beige); //Color(245, 245, 220);

            //
            // 摘要:
            //     Bisque, the color that is represented by the RGB value #FFFFE4C4.
            colors.Add(Color.Bisque); //Color(255, 228, 196);

            //
            // 摘要:
            //     Black, the color that is represented by the RGB value #000000.
            colors.Add(Color.Black); //Color(0, 0, 0);

            //
            // 摘要:
            //     BlanchedAlmond, the color that is represented by the RGB value #FFFFEBCD.
            colors.Add(Color.BlanchedAlmond); //Color(255, 235, 205);

            //
            // 摘要:
            //     Blue, the color that is represented by the RGB value #0000ff.
            colors.Add(Color.Blue); //Color(0, 0, 255);

            //
            // 摘要:
            //     BlueViolet, the color that is represented by the RGB value #FF8A2BE2.
            colors.Add(Color.BlueViolet); //Color(138, 43, 226);

            //
            // 摘要:
            //     Brown, the color that is represented by the RGB value #FFA52A2A.
            colors.Add(Color.Brown); //Color(165, 42, 42);

            //
            // 摘要:
            //     BurlyWood, the color that is represented by the RGB value #FFDEB887.
            colors.Add(Color.BurlyWood); //Color(222, 184, 135);

            //
            // 摘要:
            //     CadetBlue, the color that is represented by the RGB value #FF5F9EA0.
            colors.Add(Color.CadetBlue); //Color(95, 158, 160);

            //
            // 摘要:
            //     Chartreuse, the color that is represented by the RGB value #FF7FFF00.
            colors.Add(Color.Chartreuse); //Color(127, 255, 0);

            //
            // 摘要:
            //     Chocolate, the color that is represented by the RGB value #FFD2691E.
            colors.Add(Color.Chocolate); //Color(210, 105, 30);

            //
            // 摘要:
            //     Coral, the color that is represented by the RGB value #FFFF7F50.
            colors.Add(Color.Coral); //Color(255, 127, 80);

            //
            // 摘要:
            //     CornflowerBlue, the color that is represented by the RGB value #FF6495ED.
            colors.Add(Color.CornflowerBlue); //Color(100, 149, 237);

            //
            // 摘要:
            //     Cornsilk, the color that is represented by the RGB value #FFFFF8DC.
            colors.Add(Color.Cornsilk); //Color(255, 248, 220);

            //
            // 摘要:
            //     Crimson, the color that is represented by the RGB value #FFDC143C.
            colors.Add(Color.Crimson); //Color(220, 20, 60);

            //
            // 摘要:
            //     Cyan, the color that is represented by the RGB value #FF00FFFF.
            colors.Add(Color.Cyan); //Color(0, 255, 255);

            //
            // 摘要:
            //     DarkBlue, the color that is represented by the RGB value #FF00008B.
            colors.Add(Color.DarkBlue); //Color(0, 0, 139);

            //
            // 摘要:
            //     DarkCyan, the color that is represented by the RGB value #FF008B8B.
            colors.Add(Color.DarkCyan); //Color(0, 139, 139);

            //
            // 摘要:
            //     DarkGoldenrod, the color that is represented by the RGB value #FFB8860B.
            colors.Add(Color.DarkGoldenrod); //Color(184, 134, 11);

            //
            // 摘要:
            //     DarkGray, the color that is represented by the RGB value #FFA9A9A9.
            colors.Add(Color.DarkGray); //Color(169, 169, 169);

            //
            // 摘要:
            //     DarkGreen, the color that is represented by the RGB value #FF006400.
            colors.Add(Color.DarkGreen); //Color(0, 100, 0);

            //
            // 摘要:
            //     DarkKhaki, the color that is represented by the RGB value #FFBDB76B.
            colors.Add(Color.DarkKhaki); //Color(189, 183, 107);

            //
            // 摘要:
            //     DarkMagenta, the color that is represented by the RGB value #FF8B008B.
            colors.Add(Color.DarkMagenta); //Color(139, 0, 139);

            //
            // 摘要:
            //     DarkOliveGreen, the color that is represented by the RGB value #FF556B2F.
            colors.Add(Color.DarkOliveGreen); //Color(85, 107, 47);

            //
            // 摘要:
            //     DarkOrange, the color that is represented by the RGB value #FFFF8C00.
            colors.Add(Color.DarkOrange); //Color(255, 140, 0);

            //
            // 摘要:
            //     DarkOrchid, the color that is represented by the RGB value #FF9932CC.
            colors.Add(Color.DarkOrchid); //Color(153, 50, 204);

            //
            // 摘要:
            //     DarkRed, the color that is represented by the RGB value #FF8B0000.
            colors.Add(Color.DarkRed); //Color(139, 0, 0);

            //
            // 摘要:
            //     DarkSalmon, the color that is represented by the RGB value #FFE9967A.
            colors.Add(Color.DarkSalmon); //Color(233, 150, 122);

            //
            // 摘要:
            //     DarkSeaGreen, the color that is represented by the RGB value #FF8FBC8F.
            colors.Add(Color.DarkSeaGreen); //Color(143, 188, 143);

            //
            // 摘要:
            //     DarkSlateBlue, the color that is represented by the RGB value #FF483D8B.
            colors.Add(Color.DarkSlateBlue); //Color(72, 61, 139);

            //
            // 摘要:
            //     DarkSlateGray, the color that is represented by the RGB value #FF2F4F4F.
            colors.Add(Color.DarkSlateGray); //Color(47, 79, 79);

            //
            // 摘要:
            //     DarkTurquoise, the color that is represented by the RGB value #FF00CED1.
            colors.Add(Color.DarkTurquoise); //Color(0, 206, 209);

            //
            // 摘要:
            //     DarkViolet, the color that is represented by the RGB value #FF9400D3.
            colors.Add(Color.DarkViolet); //Color(148, 0, 211);

            //
            // 摘要:
            //     DeepPink, the color that is represented by the RGB value #FFFF1493.
            colors.Add(Color.DeepPink); //Color(255, 20, 147);

            //
            // 摘要:
            //     DeepSkyBlue, the color that is represented by the RGB value #FF00BFFF.
            colors.Add(Color.DeepSkyBlue); //Color(0, 191, 255);

            //
            // 摘要:
            //     DimGray, the color that is represented by the RGB value #FF696969.
            colors.Add(Color.DimGray); //Color(105, 105, 105);

            //
            // 摘要:
            //     DodgerBlue, the color that is represented by the RGB value #FF1E90FF.
            colors.Add(Color.DodgerBlue); //Color(30, 144, 255);

            //
            // 摘要:
            //     Firebrick, the color that is represented by the RGB value #FFB22222.
            colors.Add(Color.Firebrick); //Color(178, 34, 34);

            //
            // 摘要:
            //     FloralWhite, the color that is represented by the RGB value #FFFFFAF0.
            colors.Add(Color.FloralWhite); //Color(255, 250, 240);

            //
            // 摘要:
            //     ForestGreen, the color that is represented by the RGB value #FF228B22.
            colors.Add(Color.ForestGreen); //Color(34, 139, 34);

            //
            // 摘要:
            //     Fuchsia, the color that is represented by the RGB value #ff00ff.
            colors.Add(Color.Fuchsia); //Color(255, 0, 255);

            //
            // 摘要:
            //     Gainsboro, the color that is represented by the RGB value #FFDCDCDC.
            colors.Add(Color.Gainsboro); //Color(220, 220, 220);

            //
            // 摘要:
            //     GhostWhite, the color that is represented by the RGB value #FFF8F8FF.
            colors.Add(Color.GhostWhite); //Color(248, 248, 255);

            //
            // 摘要:
            //     Gold, the color that is represented by the RGB value #FFFFD700.
            colors.Add(Color.Gold); //Color(255, 215, 0);

            //
            // 摘要:
            //     Goldenrod, the color that is represented by the RGB value #FFDAA520.
            colors.Add(Color.Goldenrod); //Color(218, 165, 32);

            //
            // 摘要:
            //     Gray, the color that is represented by the RGB value #808080.
            colors.Add(Color.Gray); //Color(128, 128, 128);

            //
            // 摘要:
            //     Green, the color that is represented by the RGB value #008000.
            colors.Add(Color.Green); //Color(0, 128, 0);

            //
            // 摘要:
            //     GreenYellow, the color that is represented by the RGB value #FFADFF2F.
            colors.Add(Color.GreenYellow); //Color(173, 255, 47);

            //
            // 摘要:
            //     Honeydew, the color that is represented by the RGB value #FFF0FFF0.
            colors.Add(Color.Honeydew); //Color(240, 255, 240);

            //
            // 摘要:
            //     HotPink, the color that is represented by the RGB value #FFFF69B4.
            colors.Add(Color.HotPink); //Color(255, 105, 180);

            //
            // 摘要:
            //     IndianRed, the color that is represented by the RGB value #FFCD5C5C.
            colors.Add(Color.IndianRed); //Color(205, 92, 92);

            //
            // 摘要:
            //     Indigo, the color that is represented by the RGB value #FF4B0082.
            colors.Add(Color.Indigo); //Color(75, 0, 130);

            //
            // 摘要:
            //     Ivory, the color that is represented by the RGB value #FFFFFFF0.
            colors.Add(Color.Ivory); //Color(255, 255, 240);

            //
            // 摘要:
            //     Khaki, the color that is represented by the RGB value #FFF0E68C.
            colors.Add(Color.Khaki); //Color(240, 230, 140);

            //
            // 摘要:
            //     Lavender, the color that is represented by the RGB value #FFE6E6FA.
            colors.Add(Color.Lavender); //Color(230, 230, 250);

            //
            // 摘要:
            //     LavenderBlush, the color that is represented by the RGB value #FFFFF0F5.
            colors.Add(Color.LavenderBlush); //Color(255, 240, 245);

            //
            // 摘要:
            //     LawnGreen, the color that is represented by the RGB value #FF7CFC00.
            colors.Add(Color.LawnGreen); //Color(124, 252, 0);

            //
            // 摘要:
            //     LemonChiffon, the color that is represented by the RGB value #FFFFFACD.
            colors.Add(Color.LemonChiffon); //Color(255, 250, 205);

            //
            // 摘要:
            //     LightBlue, the color that is represented by the RGB value #FFADD8E6.
            colors.Add(Color.LightBlue); //Color(173, 216, 230);

            //
            // 摘要:
            //     LightCoral, the color that is represented by the RGB value #FFF08080.
            colors.Add(Color.LightCoral); //Color(240, 128, 128);

            //
            // 摘要:
            //     LightCyan, the color that is represented by the RGB value #FFE0FFFF.
            colors.Add(Color.LightCyan); //Color(224, 255, 255);

            //
            // 摘要:
            //     LightGoldenrodYellow, the color that is represented by the RGB value #FFFAFAD2.
            colors.Add(Color.LightGoldenrodYellow); //Color(250, 250, 210);

            //
            // 摘要:
            //     LightGray, the color that is represented by the RGB value #FFD3D3D3.
            colors.Add(Color.LightGray); //Color(211, 211, 211);

            //
            // 摘要:
            //     LightGreen, the color that is represented by the RGB value #FF90EE90.
            colors.Add(Color.LightGreen); //Color(144, 238, 144);

            //
            // 摘要:
            //     LightPink, the color that is represented by the RGB value #FFFFB6C1.
            colors.Add(Color.LightPink); //Color(255, 182, 193);

            //
            // 摘要:
            //     LightSalmon, the color that is represented by the RGB value #FFFFA07A.
            colors.Add(Color.LightSalmon); //Color(255, 160, 122);

            //
            // 摘要:
            //     LightSeaGreen, the color that is represented by the RGB value #FF20B2AA.
            colors.Add(Color.LightSeaGreen); //Color(32, 178, 170);

            //
            // 摘要:
            //     LightSkyBlue, the color that is represented by the RGB value #FF87CEFA.
            colors.Add(Color.LightSkyBlue); //Color(135, 206, 250);

            //
            // 摘要:
            //     LightSlateGray, the color that is represented by the RGB value #FF778899.
            colors.Add(Color.LightSlateGray); //Color(119, 136, 153);

            //
            // 摘要:
            //     LightSteelBlue, the color that is represented by the RGB value #FFB0C4DE.
            colors.Add(Color.LightSteelBlue); //Color(176, 196, 222);

            //
            // 摘要:
            //     LightYellow, the color that is represented by the RGB value #FFFFFFE0.
            colors.Add(Color.LightYellow); //Color(255, 255, 224);

            //
            // 摘要:
            //     Lime, the color that is represented by the RGB value #00ff00.
            colors.Add(Color.Lime); //Color(0, 255, 0);

            //
            // 摘要:
            //     LimeGreen, the color that is represented by the RGB value #FF32CD32.
            colors.Add(Color.LimeGreen); //Color(50, 205, 50);

            //
            // 摘要:
            //     Linen, the color that is represented by the RGB value #FFFAF0E6.
            colors.Add(Color.Linen); //Color(250, 240, 230);

            //
            // 摘要:
            //     Magenta, the color that is represented by the RGB value #FFFF00FF.
            colors.Add(Color.Magenta); //Color(255, 0, 255);

            //
            // 摘要:
            //     Maroon, the color that is represented by the RGB value #800000.
            colors.Add(Color.Maroon); //Color(128, 0, 0);

            //
            // 摘要:
            //     MediumAquamarine, the color that is represented by the RGB value #FF66CDAA.
            colors.Add(Color.MediumAquamarine); //Color(102, 205, 170);

            //
            // 摘要:
            //     MediumBlue, the color that is represented by the RGB value #FF0000CD.
            colors.Add(Color.MediumBlue); //Color(0, 0, 205);

            //
            // 摘要:
            //     MediumOrchid, the color that is represented by the RGB value #FFBA55D3.
            colors.Add(Color.MediumOrchid); //Color(186, 85, 211);

            //
            // 摘要:
            //     MediumPurple, the color that is represented by the RGB value #FF9370DB.
            colors.Add(Color.MediumPurple); //Color(147, 112, 219);

            //
            // 摘要:
            //     MediumSeaGreen, the color that is represented by the RGB value #FF3CB371.
            colors.Add(Color.MediumSeaGreen); //Color(60, 179, 113);

            //
            // 摘要:
            //     MediumSlateBlue, the color that is represented by the RGB value #FF7B68EE.
            colors.Add(Color.MediumSlateBlue); //Color(123, 104, 238);

            //
            // 摘要:
            //     MediumSpringGreen, the color that is represented by the RGB value #FF00FA9A.
            colors.Add(Color.MediumSpringGreen); //Color(0, 250, 154);

            //
            // 摘要:
            //     MediumTurquoise, the color that is represented by the RGB value #FF48D1CC.
            colors.Add(Color.MediumTurquoise); //Color(72, 209, 204);

            //
            // 摘要:
            //     MediumVioletRed, the color that is represented by the RGB value #FFC71585.
            colors.Add(Color.MediumVioletRed); //Color(199, 21, 133);

            //
            // 摘要:
            //     MidnightBlue, the color that is represented by the RGB value #FF191970.
            colors.Add(Color.MidnightBlue); //Color(25, 25, 112);

            //
            // 摘要:
            //     MintCream, the color that is represented by the RGB value #FFF5FFFA.
            colors.Add(Color.MintCream); //Color(245, 255, 250);

            //
            // 摘要:
            //     MistyRose, the color that is represented by the RGB value #FFFFE4E1.
            colors.Add(Color.MistyRose); //Color(255, 228, 225);

            //
            // 摘要:
            //     Moccasin, the color that is represented by the RGB value #FFFFE4B5.
            colors.Add(Color.Moccasin); //Color(255, 228, 181);

            //
            // 摘要:
            //     NavajoWhite, the color that is represented by the RGB value #FFFFDEAD.
            colors.Add(Color.NavajoWhite); //Color(255, 222, 173);

            //
            // 摘要:
            //     Navy, the color that is represented by the RGB value #000080.
            colors.Add(Color.Navy); //Color(0, 0, 128);

            //
            // 摘要:
            //     OldLace, the color that is represented by the RGB value #FFFDF5E6.
            colors.Add(Color.OldLace); //Color(253, 245, 230);

            //
            // 摘要:
            //     Olive, the color that is represented by the RGB value #808000.
            colors.Add(Color.Olive); //Color(128, 128, 0);

            //
            // 摘要:
            //     OliveDrab, the color that is represented by the RGB value #FF6B8E23.
            colors.Add(Color.OliveDrab); //Color(107, 142, 35);

            //
            // 摘要:
            //     Orange, the color that is represented by the RGB value #ffa500.
            colors.Add(Color.Orange); //Color(255, 165, 0);

            //
            // 摘要:
            //     OrangeRed, the color that is represented by the RGB value #FFFF4500.
            colors.Add(Color.OrangeRed); //Color(255, 69, 0);

            //
            // 摘要:
            //     Orchid, the color that is represented by the RGB value #FFDA70D6.
            colors.Add(Color.Orchid); //Color(218, 112, 214);

            //
            // 摘要:
            //     PaleGoldenrod, the color that is represented by the RGB value #FFEEE8AA.
            colors.Add(Color.PaleGoldenrod); //Color(238, 232, 170);

            //
            // 摘要:
            //     PaleGreen, the color that is represented by the RGB value #FF98FB98.
            colors.Add(Color.PaleGreen); //Color(152, 251, 152);

            //
            // 摘要:
            //     PaleTurquoise, the color that is represented by the RGB value #FFAFEEEE.
            colors.Add(Color.PaleTurquoise); //Color(175, 238, 238);

            //
            // 摘要:
            //     PaleVioletRed, the color that is represented by the RGB value #FFDB7093.
            colors.Add(Color.PaleVioletRed); //Color(219, 112, 147);

            //
            // 摘要:
            //     PapayaWhip, the color that is represented by the RGB value #FFFFEFD5.
            colors.Add(Color.PapayaWhip); //Color(255, 239, 213);

            //
            // 摘要:
            //     PeachPuff, the color that is represented by the RGB value #FFFFDAB9.
            colors.Add(Color.PeachPuff); //Color(255, 218, 185);

            //
            // 摘要:
            //     Peru, the color that is represented by the RGB value #FFCD853F.
            colors.Add(Color.Peru); //Color(205, 133, 63);

            //
            // 摘要:
            //     Pink, the color that is represented by the RGB value #ff66ff.
            colors.Add(Color.Pink); //Color(255, 192, 203);

            //
            // 摘要:
            //     Plum, the color that is represented by the RGB value #FFDDA0DD.
            colors.Add(Color.Plum); //Color(221, 160, 221);

            //
            // 摘要:
            //     PowderBlue, the color that is represented by the RGB value #FFB0E0E6.
            colors.Add(Color.PowderBlue); //Color(176, 224, 230);

            //
            // 摘要:
            //     Purple, the color that is represented by the RGB value #800080.
            colors.Add(Color.Purple); //Color(128, 0, 128);

            //
            // 摘要:
            //     Red, the color that is represented by the RGB value #ff0000.
            colors.Add(Color.Red); //Color(255, 0, 0);

            //
            // 摘要:
            //     RosyBrown, the color that is represented by the RGB value #FFBC8F8F.
            colors.Add(Color.RosyBrown); //Color(188, 143, 143);

            //
            // 摘要:
            //     RoyalBlue, the color that is represented by the RGB value #FF4169E1.
            colors.Add(Color.RoyalBlue); //Color(65, 105, 225);

            //
            // 摘要:
            //     SaddleBrown, the color that is represented by the RGB value #FF8B4513.
            colors.Add(Color.SaddleBrown); //Color(139, 69, 19);

            //
            // 摘要:
            //     Salmon, the color that is represented by the RGB value #FFFA8072.
            colors.Add(Color.Salmon); //Color(250, 128, 114);

            //
            // 摘要:
            //     SandyBrown, the color that is represented by the RGB value #FFF4A460.
            colors.Add(Color.SandyBrown); //Color(244, 164, 96);

            //
            // 摘要:
            //     SeaGreen, the color that is represented by the RGB value #FF2E8B57.
            colors.Add(Color.SeaGreen); //Color(46, 139, 87);

            //
            // 摘要:
            //     SeaShell, the color that is represented by the RGB value #FFFFF5EE.
            colors.Add(Color.SeaShell); //Color(255, 245, 238);

            //
            // 摘要:
            //     Sienna, the color that is represented by the RGB value #FFA0522D.
            colors.Add(Color.Sienna); //Color(160, 82, 45);

            //
            // 摘要:
            //     Silver, the color that is represented by the RGB value #c0c0c0.
            colors.Add(Color.Silver); //Color(192, 192, 192);

            //
            // 摘要:
            //     SkyBlue, the color that is represented by the RGB value #FF87CEEB.
            colors.Add(Color.SkyBlue); //Color(135, 206, 235);

            //
            // 摘要:
            //     SlateBlue, the color that is represented by the RGB value #FF6A5ACD.
            colors.Add(Color.SlateBlue); //Color(106, 90, 205);

            //
            // 摘要:
            //     SlateGray, the color that is represented by the RGB value #FF708090.
            colors.Add(Color.SlateGray); //Color(112, 128, 144);

            //
            // 摘要:
            //     Snow, the color that is represented by the RGB value #FFFFFAFA.
            colors.Add(Color.Snow); //Color(255, 250, 250);

            //
            // 摘要:
            //     SpringGreen, the color that is represented by the RGB value #FF00FF7F.
            colors.Add(Color.SpringGreen); //Color(0, 255, 127);

            //
            // 摘要:
            //     SteelBlue, the color that is represented by the RGB value #FF4682B4.
            colors.Add(Color.SteelBlue); //Color(70, 130, 180);

            //
            // 摘要:
            //     Tan, the color that is represented by the RGB value #FFD2B48C.
            colors.Add(Color.Tan); //Color(210, 180, 140);

            //
            // 摘要:
            //     Teal, the color that is represented by the RGB value #008080.
            colors.Add(Color.Teal); //Color(0, 128, 128);

            //
            // 摘要:
            //     Thistle, the color that is represented by the RGB value #FFD8BFD8.
            colors.Add(Color.Thistle); //Color(216, 191, 216);

            //
            // 摘要:
            //     Tomato, the color that is represented by the RGB value #FFFF6347.
            colors.Add(Color.Tomato); //Color(255, 99, 71);

            //
            // 摘要:
            //     The transparent color, represented by the RGB value #00ffffff.
            //
            // 言论：
            //     The Alpha channel of the Xamarin.Forms.Color.Transparent color is set to 0.
            colors.Add(Color.Transparent); //Color(255, 255, 255, 0);

            //
            // 摘要:
            //     Turquoise, the color that is represented by the RGB value #FF40E0D0.
            colors.Add(Color.Turquoise); //Color(64, 224, 208);

            //
            // 摘要:
            //     Violet, the color that is represented by the RGB value #FFEE82EE.
            colors.Add(Color.Violet); //Color(238, 130, 238);

            //
            // 摘要:
            //     Wheat, the color that is represented by the RGB value #FFF5DEB3.
            colors.Add(Color.Wheat); //Color(245, 222, 179);

            //
            // 摘要:
            //     White, the color that is represented by the RGB value #ffffff.
            colors.Add(Color.White); //Color(255, 255, 255);

            //
            // 摘要:
            //     WhiteSmoke, the color that is represented by the RGB value #FFF5F5F5.
            colors.Add(Color.WhiteSmoke); //Color(245, 245, 245);

            //
            // 摘要:
            //     Yellow, the color that is represented by the RGB value #ffff00.
            colors.Add(Color.Yellow); //Color(255, 255, 0);

            //
            // 摘要:
            //     YellowGreen, the color that is represented by the RGB value #FF9ACD32.
            colors.Add(Color.YellowGreen); //Color(154, 205, 50);
            #endregion

            List<Ctrl.Color> ctrColors = new List<Ctrl.Color>();
            foreach (var color in colors)
            {
                ctrColors.Add(Ctrl.Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            _Colors = ctrColors;
            return ctrColors;
        }
        #endregion
    }
}
