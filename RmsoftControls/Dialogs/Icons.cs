using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace RmsoftControls.Dialogs
{
    public sealed class Icons
    {
        public static readonly BitmapSource ErrorIcon32 = Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Error.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public static readonly BitmapSource WarningIcon32 = Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Warning.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public static readonly BitmapSource QuestionIcon32 = Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Question.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public static readonly BitmapSource InformationIcon32 = Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Information.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
}
