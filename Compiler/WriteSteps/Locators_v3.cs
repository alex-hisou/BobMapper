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

        internal Locators_v3(List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs)
        {
            locatorsOutput = new List<byte>();
            locatorsOutput.AddRange([0x0B, 0x00, 0x00, 0x00]); // SECTION HEAD
            byte[] locators_v3 = Encoding.ASCII.GetBytes("Locators_v3");
            locatorsOutput.AddRange(locators_v3);

            List<byte> locatorByteBuffer =
            [
                .. NPCsAsBytes(npcs),
                .. PathPointsAsBytes(pathPoints),
                .. MiscsAsBytes(miscs),
                .. AddLocatorQueueAsBytes(),
            ];
            locatorsOutput.AddRange([0xD4, 0x06, 0x00, 0x00]);
            locatorsOutput.AddRange([0x00, 0x00]);

            locatorsOutput.AddRange(locatorByteBuffer);
        }

        private List<byte> NPCsAsBytes(List<NPC> npcs)
        {
            List<byte> byteNPCs = new List<byte>();
            for (int i = 0; i < npcs.Count; i++)
            {
                byte[] currentByteNPC = new byte[76];
                NPC npc = npcs[i];
                _64CompiledCoordinate npcCompiledCoordinate = new _64CompiledCoordinate(npc.Coordinates, npc.Rotation);
                Array.Copy(npcCompiledCoordinate.CompiledBytes, 0, currentByteNPC, 2, 14);
                currentByteNPC[16] = 0x01; //NPC Header
                currentByteNPC[20] = Convert.ToByte(i);
                currentByteNPC[60] = Convert.ToByte((int)npc.Type);
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
            }
            return byteNPCs;
        }

        private List<byte> PathPointsAsBytes(List<PathPoint> pathPoints)
        {
            List<byte> bytePathPoints = new List<byte>();
            int[] connectFromIds = CompilerServices.GetConnectFromIds(pathPoints);
            for (int i = 0; i < pathPoints.Count; i++)
            {
                byte[] currentBytePathPoint = new byte[76];
                PathPoint point = pathPoints[i];
                _64CompiledCoordinate pathPointCompileCoordinate = new(point.Coordinates, point.Rotation);
                Array.Copy(pathPointCompileCoordinate.CompiledBytes, 0, currentBytePathPoint, 1, 14);
                currentBytePathPoint[15] = 0x05; //Path Point Header
                currentBytePathPoint[19] = Convert.ToByte(point.Id);
                currentBytePathPoint[55] = Convert.ToByte(connectFromIds[i]);
                currentBytePathPoint[59] = Convert.ToByte(point.Duration);
                currentBytePathPoint[63] = Convert.ToByte(point.ConnectToId);
                bytePathPoints.AddRange(currentBytePathPoint);
            }
            return bytePathPoints;
        }

        private List<byte> MiscsAsBytes(List<Misc> miscs)
        {
            List<byte> byteMiscs = new List<byte>();
            foreach (Misc misc in miscs)
            {
                byte[] currentByteMisc = new byte[76];
                _64CompiledCoordinate miscCompiledCoordinate = new(misc.Coordinates, misc.Rotation);
                Array.Copy(miscCompiledCoordinate.CompiledBytes, 0, currentByteMisc, 2, 14);
                currentByteMisc[16] = Convert.ToByte((int)misc.Type); //Hacky way to get header from enum value
                //TODO: Check if any other params exist for the different types of miscs
                byteMiscs.AddRange(currentByteMisc);

            }
            return byteMiscs;
        }

        private List<byte> AddLocatorQueueAsBytes()
        {
            List<byte> byteLocators = new List<byte>();
            foreach (QueuedLocator locator in Compiler.locatorQueue)
            {
                byte[] currentByteLocator = new byte[76];
                _64CompiledCoordinate locatorCompiledCoordinate = new(locator.Coordinates, locator.Rotation);
                Array.Copy(locatorCompiledCoordinate.CompiledBytes, 0, currentByteLocator, 2, 14);
                currentByteLocator[16] = Convert.ToByte((int)locator.LocatorType);
                //Same thing here
                byteLocators.AddRange(currentByteLocator);
            }
            return byteLocators;
        }
    }
}
