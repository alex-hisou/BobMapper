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

        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private Prop startButton;
        [JsonIgnore]
        public Prop StartButton
        {
            get { return startButton; }
            set { startButton = value; }
        }

        public Cable(Prop startButton)
        {
            ColourHex = "#FF0000";
            StartButton = startButton;
        }

        [JsonConstructor]
        public Cable(string colourHex, List<SnapCoordinate> coordinates, int duration)
        {
            ColourHex = colourHex;
            Coordinates = coordinates;
            Duration = duration;
        }

    }
}
