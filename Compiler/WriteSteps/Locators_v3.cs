using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler.WriteSteps
{
    internal class Locators_v3
    {
        internal List<byte> locatorsOutput;
        private int currentLocatorId = 1;

        internal Locators_v3(List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs)
        {
            locatorsOutput = new List<byte>();
            locatorsOutput.AddRange([0x0B, 0x00, 0x00, 0x00]); // SECTION HEAD
            byte[] locators_v3 = Encoding.ASCII.GetBytes("Locators_v3");
            locatorsOutput.AddRange(locators_v3);

            List<byte> locatorByteBuffer =
            [
                .. PathPointsAsBytes(pathPoints),
                .. NPCsAsBytes(npcs),
                .. MiscsAsBytes(miscs),
                .. AddLocatorQueueAsBytes(),
            ];
            locatorsOutput.AddRange(BitConverter.GetBytes(locatorByteBuffer.Count));

            locatorsOutput.AddRange(locatorByteBuffer);
        }

        private List<byte> NPCsAsBytes(List<NPC> npcs)
        {
            List<byte> byteNPCs = new List<byte>();
            for (int i = 0; i < npcs.Count; i++)
            {
                byte[] currentByteNPC = new byte[76];
                NPC npc = npcs[i];
                FloatCoordinate npcCompiledCoordinate = new FloatCoordinate(npc.Coordinates, npc.Rotation);
                Array.Copy(npcCompiledCoordinate.CompiledBytes, 0, currentByteNPC, 0, 16);
                currentByteNPC[16] = 0x01; //NPC Header
                Array.Copy(BitConverter.GetBytes(currentLocatorId), 0, currentByteNPC, 20, 4);
                Array.Copy(BitConverter.GetBytes(npc.FirstPathPointId), 0, currentByteNPC, 56, 4);
                Array.Copy(BitConverter.GetBytes((int)npc.Type), 0, currentByteNPC, 60, 4);
                if (npc.AttachLoot)
                {
                    QueuedLocator queueAttachLoot = new QueuedLocator(QueuedLocator.LocatorTypes.Loot, npc.Coordinates);
                    Compiler.locatorQueue.Add(queueAttachLoot);
                }
                if (npc.AttachMainLoot)
                {
                    QueuedLocator queueAttachMainLoot = new QueuedLocator(QueuedLocator.LocatorTypes.MainLoot, npc.Coordinates);
                    Compiler.locatorQueue.Add(queueAttachMainLoot);
                }
                byteNPCs.AddRange(currentByteNPC);
                currentLocatorId++;
            }
            return byteNPCs;
        }

        private List<byte> PathPointsAsBytes(List<PathPoint> pathPoints)
        {
            List<byte> bytePathPoints = new List<byte>();
            //int[] connectFromIds = CompilerServices.GetConnectFromIds(pathPoints);
            for (int i = 0; i < pathPoints.Count; i++)
            {
                byte[] currentBytePathPoint = new byte[76];
                PathPoint point = pathPoints[i];
                //TODO: What the fuck?
                FloatCoordinate pathPointCompileCoordinate = new(point.Coordinates, point.Rotation);
                Array.Copy(pathPointCompileCoordinate.CompiledBytes, 0, currentBytePathPoint, 0, pathPointCompileCoordinate.CompiledBytes.Length);
                currentBytePathPoint[16] = 0x05; //Path Point Header
                Array.Copy(BitConverter.GetBytes(currentLocatorId), 0, currentBytePathPoint, 20, 4);
                int connectFromId = 0;
                connectFromId = pathPoints.FirstOrDefault(x => x.ConnectToId == point.Id).Id;
                Array.Copy(BitConverter.GetBytes(connectFromId), 0, currentBytePathPoint, 56, 4);
                //currentBytePathPoint[56] = Convert.ToByte(connectFromIds[i]);
                Array.Copy(BitConverter.GetBytes(point.Duration), 0, currentBytePathPoint, 60, 4);
                int connectToId = 0;
                if(point.ConnectToId.HasValue)
                {
                    connectToId = (int)point.ConnectToId;
                }
                Array.Copy(BitConverter.GetBytes(connectToId), 0, currentBytePathPoint, 64, 4);
                bytePathPoints.AddRange(currentBytePathPoint);
                currentLocatorId++;
            }
            currentLocatorId = pathPoints.Max(x => x.Id) + 1;
            return bytePathPoints;
        }

        private List<byte> MiscsAsBytes(List<Misc> miscs)
        {
            List<byte> byteMiscs = new List<byte>();
            for (int i = 0; i < miscs.Count; i++)
            {
                Misc misc = miscs[i];
                byte[] currentByteMisc = new byte[76];
                FloatCoordinate miscCompiledCoordinate = new(misc.Coordinates, misc.Rotation);
                Array.Copy(miscCompiledCoordinate.CompiledBytes, 0, currentByteMisc, 0, 16);
                currentByteMisc[16] = Convert.ToByte((int)misc.Type); //Hacky way to get header from enum value
                Array.Copy(BitConverter.GetBytes(currentLocatorId), 0, currentByteMisc, 20, 4);
                //TODO: Check if any other params exist for the different types of miscs
                byteMiscs.AddRange(currentByteMisc);
                currentLocatorId++;
            }
            return byteMiscs;
        }

        private List<byte> AddLocatorQueueAsBytes()
        {
            List<byte> byteLocators = new List<byte>();
            for (int i = 0; i < Compiler.locatorQueue.Count; i++)
            {
                QueuedLocator locator = Compiler.locatorQueue[i];
                byte[] currentByteLocator = new byte[76];
                FloatCoordinate locatorCompiledCoordinate = new(locator.Coordinates, locator.Rotation);
                Array.Copy(locatorCompiledCoordinate.CompiledBytes, 0, currentByteLocator, 0, 16);
                currentByteLocator[16] = Convert.ToByte((int)locator.LocatorType);
                Array.Copy(BitConverter.GetBytes(currentLocatorId), 0, currentByteLocator, 20, 4);
                //Same thing here
                byteLocators.AddRange(currentByteLocator);
                currentLocatorId++;
            }
            return byteLocators;
        }
    }
}
