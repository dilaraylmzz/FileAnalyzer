using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FileAnalyzer.UI
{
    public class FilePicker
    {
        private readonly IEnumerable<string> _supportedExtensions;

        public FilePicker(IEnumerable<string> supportedExtensions)
        {
            _supportedExtensions = supportedExtensions;
        }

        /// <summary>Shows the OpenFileDialog. Returns null if the user cancels.</summary>
        public string PickFile()
        {
            string patterns = string.Join(";", _supportedExtensions.Select(e => "*" + e));

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Analiz edilecek dosyayı seçin";
                dialog.Filter = $"Desteklenen dosyalar|{patterns}|Tüm dosyalar|*.*";
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }
    }
}
