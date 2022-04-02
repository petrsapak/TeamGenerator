using Microsoft.Win32;
using System.IO;
using TeamGenerator.Infrastructure.Services;

namespace TeamGenerator.Services
{
    internal class OpenFileDialogService
    {
        private readonly IStatusMessageService statusMessageService;

        public OpenFileDialogService(IStatusMessageService statusMessageService)
        {
            this.statusMessageService = statusMessageService;
        }

        public string ShowOpenFileDialog(string dialogTitle, string defaultExtension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = defaultExtension;
            openFileDialog.Title = dialogTitle;

            if (openFileDialog.ShowDialog() == true)
            {
                return File.ReadAllText(openFileDialog.FileName);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
