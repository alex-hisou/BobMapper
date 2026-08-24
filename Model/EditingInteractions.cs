using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BobMapper.Data;
using BobMapper.Model.MapObjects;
using CommunityToolkit.Mvvm.Input;
using static BobMapper.Model.MapManager;

namespace BobMapper.Model
{
    public partial class EditingInteractions
    {
        private ObjectCollection currentObjectCollection;

        public ObjectCollection CurrentObjectCollection
        {
            get { return currentObjectCollection; }
            set { currentObjectCollection = value; }
        }

        private Selections currentSelections;

        public Selections CurrentSelections
        {
            get { return currentSelections; }
            set { currentSelections = value; }
        }

        private MapProperties currentMapProperties;

        public MapProperties CurrentMapProperties
        {
            get { return currentMapProperties; }
            set { currentMapProperties = value; }
        }


        public EditingInteractions(ObjectCollection objectCollection, Selections selections, MapProperties mapProperties) 
        {
            CurrentObjectCollection = objectCollection;
            CurrentSelections = selections;
            CurrentMapProperties = mapProperties;
        }

        public void HandleClickEmpty(Coordinate placementPos)
        {
            switch (CurrentSelections.SelectedTool)
            {
                case Tools.AddProp:
                    SnapCoordinate snappedPropPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    string validPropTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Prop, CurrentMapProperties.Tileset, true);
                    Prop prop = new Prop(snappedPropPlacementPos, 0, validPropTexture);
                    CurrentObjectCollection.CurrentProps.Add(prop);
                    if (prop.PropTexture == "/Resources/PropTextures/Teleporter.png")
                    {
                        SnapCoordinate tele2Coordinate = new(prop.Coordinates.SnappedXPos, prop.Coordinates.SnappedYPos);
                        tele2Coordinate.SnappedXPos += 2;
                        Prop tele2 = new Prop(tele2Coordinate, 0, "/Resources/PropTextures/Teleporter.png");
                        CurrentObjectCollection.CurrentProps.Add(tele2);
                    }
                    if (prop.PropTexture == "/Resources/PropTextures/TelePad.png")
                    {
                        SnapCoordinate tele2Coordinate = new(prop.Coordinates.SnappedXPos, prop.Coordinates.SnappedYPos);
                        tele2Coordinate.SnappedXPos += 1;
                        Prop tele2 = new Prop(tele2Coordinate, 0, "/Resources/PropTextures/TelePad.png");
                        CurrentObjectCollection.CurrentProps.Add(tele2);
                    }
                    if (UserSettings.Instance.AutoSelect)
                    {
                        SelectObject(prop);
                    }
                    break;
                case Tools.AddNPC:
                    SnapCoordinate snappedNPCPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    NPC npc = new NPC(snappedNPCPlacementPos, NPC.NPCType.BulkyCop, 0, false, false);
                    CurrentObjectCollection.CurrentNPCs.Add(npc);
                    if (UserSettings.Instance.AutoSelect)
                    {
                        SelectObject(npc);
                    }
                    break;
                case Tools.AddPathPoint:
                    SnapCoordinate snappedPathPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    int lastId = 0;
                    if (CurrentObjectCollection.CurrentPathPoints.Count > 0)
                    { lastId = CurrentObjectCollection.CurrentPathPoints.Max(x => x.Id); }
                    PathPoint pathPoint = new PathPoint(snappedPathPlacementPos, 0, lastId + 1, 0);
                    AttachNewPathPointHandler(pathPoint);
                    CurrentObjectCollection.CurrentPathPoints.Add(pathPoint);
                    if (UserSettings.Instance.AutoSelect)
                    {
                        SelectObject(pathPoint);
                    }
                    break;
                case Tools.AddMisc:
                    SnapCoordinate snappedMiscPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    Misc misc = new Misc(snappedMiscPlacementPos, Misc.MiscObjects.Key);
                    CurrentObjectCollection.CurrentMiscs.Add(misc);
                    if (UserSettings.Instance.AutoSelect)
                    {
                        SelectObject(misc);
                    }
                    break;
                case Tools.AddLoot:
                    SnapCoordinate snappedLootPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    string validLootTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Loot, CurrentMapProperties.Tileset, true);
                    Loot loot = new Loot(validLootTexture, snappedLootPlacementPos, 0);
                    CurrentObjectCollection.CurrentLoots.Add(loot);
                    if (UserSettings.Instance.AutoSelect)
                    {
                        SelectObject(loot);
                    }
                    break;
                default:
                    break;
            }
        }

        internal void AttachAllPathPointHandlers()
        {
            foreach (PathPoint pathPoint in CurrentObjectCollection.CurrentPathPoints)
            {
                AttachNewPathPointHandler(pathPoint);
                ResolvePathPointConnection(pathPoint);
            }
        }

        private void ResolvePathPointConnection(PathPoint pathPoint)
        {
            var target = CurrentObjectCollection.CurrentPathPoints.FirstOrDefault(x => x.Id == pathPoint.ConnectToId);
            if (target != null)
            {
                pathPoint.ConnectedPathPoint = target;
            }
            else if (pathPoint.ConnectToId.HasValue)
            {
                SystemSounds.Exclamation.Play();
            }
        }

        internal void AttachNewPathPointHandler(PathPoint pathPoint)
        {
            pathPoint.ConnectionPointChanged += FillPathPointConnectCoordinate;
        }

        public void FillPathPointConnectCoordinate(object sender, EventArgs e)
        {
            ResolvePathPointConnection((PathPoint)sender);
        }

        [RelayCommand]
        public void DeleteObject()
        {
            var result = MessageBox.Show("Do you want to delete the selected object?", "Delete object", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            int toDeleteId;
            switch (CurrentSelections.SelectedObjectType)
            {
                case ObjectType.Wall:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentWalls.IndexOf(CurrentSelections.SelectedWall);
                        CurrentSelections.SelectedWall = null;
                        CurrentObjectCollection.CurrentWalls.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Prop:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentProps.IndexOf(CurrentSelections.SelectedProp);
                        CurrentSelections.SelectedProp = null;
                        CurrentObjectCollection.CurrentProps.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.NPC:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentNPCs.IndexOf(CurrentSelections.SelectedNPC);
                        CurrentSelections.SelectedNPC = null;
                        CurrentObjectCollection.CurrentNPCs.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.PathPoint:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentPathPoints.IndexOf(CurrentSelections.SelectedPathPoint);
                        CurrentSelections.SelectedPathPoint = null;
                        CurrentObjectCollection.CurrentPathPoints.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Misc:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentMiscs.IndexOf(CurrentSelections.SelectedMisc);
                        CurrentSelections.SelectedMisc = null;
                        CurrentObjectCollection.CurrentMiscs.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Door:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentDoors.IndexOf(CurrentSelections.SelectedDoor);
                        CurrentSelections.SelectedDoor = null;
                        CurrentObjectCollection.CurrentDoors.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Loot:
                    {
                        toDeleteId = CurrentObjectCollection.CurrentLoots.IndexOf(CurrentSelections.SelectedLoot);
                        CurrentSelections.SelectedLoot = null;
                        CurrentObjectCollection.CurrentLoots.RemoveAt(toDeleteId);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            CurrentSelections.SelectedObjectType = ObjectType.None;
        }

        [RelayCommand]
        public void SetObjectTexture(object sender)
        {
            //SUUUUUUUUPER BAAAAAAAD!!!!!
            string parsedSender = (string)sender;
            switch (parsedSender)
            {
                case "PropTexture":
                    string validPropTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Prop, CurrentMapProperties.Tileset, false);
                    if (CurrentSelections.SelectedTexture != validPropTexture)
                    {
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                    CurrentSelections.SelectedProp.PropTexture = CurrentSelections.SelectedTexture;
                    break;
                case "LootTexture":
                    string validLootTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Loot, CurrentMapProperties.Tileset, false);
                    if (CurrentSelections.SelectedTexture != validLootTexture)
                    {
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                    CurrentSelections.SelectedLoot.Texture = CurrentSelections.SelectedTexture;
                    break;
                case "WallTexture1":
                    string validWallTexture1 = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Wall, CurrentMapProperties.Tileset, false);
                    if (CurrentSelections.SelectedTexture != validWallTexture1)
                    {
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                    CurrentSelections.SelectedWall.Texture1 = CurrentSelections.SelectedTexture;
                    break;
                case "WallTexture2":
                    string validWallTexture2 = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Wall, CurrentMapProperties.Tileset, false);
                    if (CurrentSelections.SelectedTexture != validWallTexture2)
                    {
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                    CurrentSelections.SelectedWall.Texture2 = CurrentSelections.SelectedTexture;
                    break;
                case "DoorTexture":
                    string validDoorTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Door, CurrentMapProperties.Tileset, false);
                    if (CurrentSelections.SelectedTexture != validDoorTexture)
                    {
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                    CurrentSelections.SelectedDoor.Texture1 = CurrentSelections.SelectedTexture;
                    break;
            }
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            if (CurrentSelections.SelectedTool == Tools.Select)
            {
                SelectObject(sender);
            }
            if (CurrentSelections.SelectedTool == Tools.ChangeFloor && sender is Floor)
            {
                Floor floor = (Floor)sender;
                string validFloorTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Floor, CurrentMapProperties.Tileset, false);
                if (CurrentSelections.SelectedTexture != validFloorTexture)
                {
                    SystemSounds.Exclamation.Play();
                    return;
                }
                floor.Texture1 = CurrentSelections.SelectedTexture;
                floor.SetOpacity(CurrentMapProperties.IsApartment);
            }
        }

        [RelayCommand]
        public void RightClickObject(object sender)
        {
            if (CurrentSelections.SelectedTool == Tools.ChangeFloor && sender is Floor)
            {
                Floor floor = (Floor)sender;
                floor.Flip++;
            }
        }

        private void ResetSelection()
        {
            switch (CurrentSelections.SelectedObjectType)
            {
                case ObjectType.Wall:
                    {
                        CurrentSelections.SelectedWall = null;
                        break;
                    }
                case ObjectType.Prop:
                    {
                        CurrentSelections.SelectedProp = null;
                        break;
                    }
                case ObjectType.NPC:
                    {
                        CurrentSelections.SelectedNPC = null;
                        break;
                    }
                case ObjectType.PathPoint:
                    {
                        CurrentSelections.SelectedPathPoint = null;
                        break;
                    }
                case ObjectType.Floor:
                    {
                        CurrentSelections.SelectedFloor = null;
                        break;
                    }
                case ObjectType.Misc:
                    {
                        CurrentSelections.SelectedMisc = null;
                        break;
                    }
                case ObjectType.Door:
                    {
                        CurrentSelections.SelectedDoor = null;
                        break;
                    }
                case ObjectType.Loot:
                    {
                        CurrentSelections.SelectedLoot = null;
                        break;
                    }
            }
            CurrentSelections.SelectedObjectType = ObjectType.None;
        }

        [RelayCommand]
        public void SetTexture(object sender)
        {
            CurrentSelections.SelectedTexture = (string)sender;
        }

        public void SelectObject(object sender)
        {
            //Not the best code, but this will do
            ResetSelection();
            int selectedObjectIndex;
            CurrentSelections.SelectedObjectType = TypeSchema[sender.GetType()];
            switch (TypeSchema[sender.GetType()])
            {
                case ObjectType.Wall:
                    selectedObjectIndex = CurrentObjectCollection.CurrentWalls.IndexOf((Wall)sender);
                    CurrentSelections.SelectedWall = CurrentObjectCollection.CurrentWalls[selectedObjectIndex];
                    break;
                case ObjectType.Prop: //Prop
                    selectedObjectIndex = CurrentObjectCollection.CurrentProps.IndexOf((Prop)sender);
                    CurrentSelections.SelectedProp = CurrentObjectCollection.CurrentProps[selectedObjectIndex];
                    break;
                case ObjectType.NPC: //NPC
                    selectedObjectIndex = CurrentObjectCollection.CurrentNPCs.IndexOf((NPC)sender);
                    CurrentSelections.SelectedNPC = CurrentObjectCollection.CurrentNPCs[selectedObjectIndex];
                    break;
                case ObjectType.PathPoint: //PathPoint
                    selectedObjectIndex = CurrentObjectCollection.CurrentPathPoints.IndexOf((PathPoint)sender);
                    CurrentSelections.SelectedPathPoint = CurrentObjectCollection.CurrentPathPoints[selectedObjectIndex];
                    break;
                case ObjectType.Floor: //Floor TODO: Implement
                    /*
                    selectedObjectIndex = CurrentFloors.IndexOf((Floor)sender);
                    SelectedFloor = CurrentFloors[selectedObjectIndex];
                    selectedObjectType = ObjectType.Floor;
                    */
                    break;
                case ObjectType.Misc: //Misc
                    selectedObjectIndex = CurrentObjectCollection.CurrentMiscs.IndexOf((Misc)sender);
                    CurrentSelections.SelectedMisc = CurrentObjectCollection.CurrentMiscs[selectedObjectIndex];
                    break;
                case ObjectType.Door:
                    selectedObjectIndex = CurrentObjectCollection.CurrentDoors.IndexOf((Door)sender);
                    CurrentSelections.SelectedDoor = CurrentObjectCollection.CurrentDoors[selectedObjectIndex];
                    break;
                case ObjectType.Loot:
                    selectedObjectIndex = CurrentObjectCollection.CurrentLoots.IndexOf((Loot)sender);
                    CurrentSelections.SelectedLoot = CurrentObjectCollection.CurrentLoots[selectedObjectIndex];
                    break;
                default:
                    throw new Exception("Invalid object type");
            }
        }

        private Dictionary<Type, ObjectType> TypeSchema = new Dictionary<Type, ObjectType>()
        {
            {typeof(Wall), ObjectType.Wall},
            {typeof(Prop), ObjectType.Prop},
            {typeof(NPC), ObjectType.NPC},
            {typeof(PathPoint), ObjectType.PathPoint},
            {typeof(Floor), ObjectType.Floor},
            {typeof(Misc), ObjectType.Misc},
            {typeof(Door), ObjectType.Door },
            {typeof(Loot), ObjectType.Loot }
        };
    }
}
