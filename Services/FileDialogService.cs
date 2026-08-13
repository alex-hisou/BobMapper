using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BobMapper.Services
{
    public class FileDialogService
    {
        public string SaveFileDialog(string filter, string defaultExt, string defaultName)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = defaultName
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string LoadFileDialog(string filter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = filter;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                return dialog.FileName;
            }
            return "";
        }
    }
}
