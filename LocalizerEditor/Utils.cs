using System.Windows.Media;

namespace LocalizerEditor
{
    static class Utils
    {
        /// <summary>
        /// Brush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Brush ToBrush(this Color color)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            solidColorBrush.Freeze();
            return solidColorBrush;
        }
    }
}
