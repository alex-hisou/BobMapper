using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BobMapper.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string SaveFileDialog(string filter, string defaultExt)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }

    public interface IFileDialogService
    {
        string SaveFileDialog(string filter, string defaultExt);
    }
}
