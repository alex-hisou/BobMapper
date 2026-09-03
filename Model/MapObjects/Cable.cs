using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model.MapObjects
{
    public class Cable
    {

        private List<SnapCoordinate> coordinates = new();

        public List<SnapCoordinate> Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
        private string colourHex;

        public string ColourHex
        {
            get { return colourHex; }
            set { colourHex = value;}
        }

        public Cable()
        {
            ColourHex = "#FF0000";
        }

        [JsonConstructor]
        public Cable(string colourHex, List<SnapCoordinate> coordinates)
        {
            ColourHex = colourHex;
            Coordinates = coordinates;
        }

    }
}
