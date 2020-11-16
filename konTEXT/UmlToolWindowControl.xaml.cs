using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;

namespace konTEXT
{
    /// <summary>
    /// Interaction logic for UmlToolWindowControl.
    /// </summary>
    public partial class UmlToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmlToolWindowControl"/> class.
        /// </summary>
        public UmlToolWindowControl()
        {
            this.InitializeComponent();

            DataContext = new UmlToolWindowViewModel();
        }

        private void SaveToPng(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)UmlImage.Source));
                encoder.Save(fileStream);
            }
        }

        private void UmlImage_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Save an Image File",
                RestoreDirectory = true,
                FileName = "konText_image.png"
            };

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveToPng(saveFileDialog.FileName);
            }
        }
    }
}
