using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.ViewModel
{
    internal class ShortcutListViewModel : ViewModelBase
    {
        internal ShortcutListViewModel() 
        {

        }

        public List<Shortcut> Shortcuts { get; set; } =
        [
            new("Select tool", "S", ""),
            new("Move tool", "E", ""),
            new("Rotate tool", "R", ""),
            new("Draw Wall tool", "W", ""),
            new("Place Prop tool", "O", ""),
            new("Add NPC tool", "N", ""),
            new("Add Path Point tool", "P", ""),
            new("Add Misc tool", "M", ""),
            new("Add Door tool", "D", ""),
            new("Add Loot tool", "L", ""),
            new("Delete selected object", "Delete", ""),
            new("Zoom In", "+", "Shift"),
            new("Zoom Out", "-", "Shift"),
            new("Reset Zoom", "*", "Shift"),
            new("Camera pan", "Arrow keys", "Shift"),
            new("Reset camera pan", "C", "Shift"),
            new("Save map", "S", "Control"),
            new("Compile map", "C", "Control")
        ];

        public class Shortcut
        {
            public string Action { get; set; }
            public string Key { get; set; }
            public string Modifier { get; set; }

            internal Shortcut(string action, string key, string modifier)
            {
                Action = action;
                Key = key;
                Modifier = modifier;
            }

        }
    }
}
