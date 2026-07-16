using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BobMapper.Model.MapObjects
{
    public class NPC : ISinglePointObject, INotifyPropertyChanged
    {

        private NPCType type;

        public NPCType Type
        {
            get { return type; }
            set { type = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
                SetNPCTexture();
            }
        }

        public SnapCoordinate Coordinates { get; set; }

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation)));
            }
        }

        private int firstPathPointId;

        public int FirstPathPointId
        {
            get { return firstPathPointId; }
            set { firstPathPointId = value; }
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

        private bool attachMainLoot;
        public bool AttachMainLoot
        {
            get { return attachMainLoot; }
            set
            {
                attachMainLoot = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttachMainLoot)));
            }
        }

        public NPC(SnapCoordinate coordinates, NPCType type, int rotation, bool attachLoot, bool attachMainLoot, int firstPathPointId)
        {
            Coordinates = coordinates;
            Type = type;
            SetNPCTexture();
            Rotation = rotation;
            AttachLoot = attachLoot;
            AttachMainLoot = attachMainLoot;
            FirstPathPointId = firstPathPointId;
        }

        public void DeleteObject()
        {
            throw new NotImplementedException();
        }

        private void SetNPCTexture() //Why are we still here? Just to suffer.
        {
            switch (Type)
            {
                case NPCType.BulkyCop:
                    Texture = "/Resources/NPCTextures/Guard.png";
                    break;
                case NPCType.BaldCop:
                    Texture = "/Resources/NPCTextures/BaldGuard.png";
                    break;
                case NPCType.RedDressLady:
                    Texture = "/Resources/NPCTextures/Female.png";
                    break;
                case NPCType.RedShirtGuy:
                    Texture = "/Resources/NPCTextures/ShirtGuy.png";
                    break;
                case NPCType.Grandma:
                    Texture = "/Resources/NPCTextures/Hag.png";
                    break;
                case NPCType.Dog:
                    Texture = "/Resources/NPCTextures/Dog.png";
                    break;
                case NPCType.Agent:
                    Texture = "/Resources/NPCTextures/Agent.png";
                    break;
                case NPCType.Scientist:
                    Texture = "/Resources/NPCTextures/Scientist.png";
                    break;
                case NPCType.RedDressLady2:
                    Texture = "/Resources/NPCTextures/Female.png";
                    break;
                case NPCType.SkinnyCop:
                    Texture = "/Resources/NPCTextures/SkinnyCop.png";
                    break;
                case NPCType.BaldCop_Flashlight:
                    Texture = "/Resources/NPCTextures/BaldGuardFL.png";
                    break;
                case NPCType.SecretSam:
                    Texture = "/Resources/NPCTextures/Dealer.png";
                    break;
                case NPCType.Biff:
                    Texture = "/Resources/NPCTextures/Biff.png";
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
