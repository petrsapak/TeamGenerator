using Microsoft.Win32;
using System.IO;
using TeamGenerator.Infrastructure.Services;

namespace TeamGenerator.Services
{
    internal class SaveFileDialogService
    {
        private readonly IStatusMessageService statusMessageService;
        public SaveFileDialogService(IStatusMessageService statusMessageService)
        {
            this.statusMessageService = statusMessageService;
        }

        public void ShowSaveFileDialog(string dialogTitle, string defaultExtension, string data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = defaultExtension;
            saveFileDialog.Title = dialogTitle;

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, data);
                statusMessageService.UpdateStatusMessage($"Data saved at {saveFileDialog.FileName}.");
            }
        }
    }
}
