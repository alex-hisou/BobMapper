using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BobMapper.Model.MapObjects
{
    public class NPC : ISinglePointObject, INotifyPropertyChanged
    {

        public NPCType Type { get; set; }
        public Coordinate Coordinates { get; set; }

        private int rotation;
        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string texture;
        [JsonIgnore]
        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }

        private bool attachLoot;
        public bool AttachLoot
        {
            get { return attachLoot; }
            set { attachLoot = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttachLoot))); } 
        }

        public NPC(Coordinate coordinates, NPCType type, int rotation, bool attachLoot)
        {
            Coordinates = coordinates;
            Type = type;
            SetNPCType();
            Rotation = rotation;
            AttachLoot = attachLoot;
        }

        public void DeleteObject()
        {
            throw new NotImplementedException();
        }

        private void SetNPCType() //Why are we still here? Just to suffer.
        {
            switch (Type)
            {
                //TODO: Rewrite with aliases and finish
                case NPCType.BulkyCop:
                    Texture = "/Resources/NPCTextures/Guard.png";
                    break;
                case NPCType.BaldCop:
                    Texture = "/Resources/NPCTextures/BaldGuard.png";
                    break;
                case NPCType.RedDressLady:
                    Texture = "/Resources/NPCTextures/Female.png";
                    break;
            }
        }
        public enum NPCType
        {
            BulkyCop,
            BaldCop,
            RedDressLady,
            RedShirtGuy,
            Grandma,
            Dog,
            Agent,
            Scientist,
            RedDressLady2,
            SkinnyCop,
            BaldCop_Flashlight,
            SecretSam,
            Biff
        }

        [JsonIgnore]
        public static Array NPCTypeValues => Enum.GetValues(typeof(NPCType));
    }
}
