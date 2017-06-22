using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MercuryEngApp.Views;
using System.Collections.ObjectModel;

namespace MercuryEngApp.Test.MercuryEngApp.Common
{
    [TestClass]
    public class PacketTest
    {
        /// <summary>
        /// Get Byte Array from Binary File
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteArray()
        {
            return File.ReadAllBytes("SourceFiles/Mock.txt");
        }

        /// <summary>
        /// Get Packet Structure Success Test Method
        /// </summary>
        [TestMethod]
        public void GetPacketTreeStructureSuccess()
        {
            byte[] byteArray = GetByteArray();

            PacketControl packet = new PacketControl(true);
            ItemsMenu root = packet.GetTreeStructure(byteArray);

            int count = root.Items.Count;
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Get Packet Structure Fail Test Method
        /// </summary>
        [TestMethod]
        public void GetPacketTreeStructureFail()
        {
            byte[] byteArray = null;

            PacketControl packet = new PacketControl(true);
            ItemsMenu root = packet.GetTreeStructure(byteArray);

            int count = root.Items.Count;
            Assert.IsFalse(count > 0);
        }

        /// <summary>
        /// Get Binary Data Success Test Method
        /// </summary>
        [TestMethod]
        public void GetBinaryDataSuccess()
        {
            byte[] byteArray = GetByteArray();

            PacketControl packet = new PacketControl(true);
            ObservableCollection<HexRecord> listHexRecord = packet.GetBinaryData(byteArray);

            int count = listHexRecord.Count;
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Get Binary Data Fail Test Method
        /// </summary>
        [TestMethod]
        public void GetBinaryDataFail()
        {
            byte[] byteArray = null;

            PacketControl packet = new PacketControl(true);
            ObservableCollection<HexRecord> listHexRecord = packet.GetBinaryData(byteArray);

            int count = listHexRecord.Count;
            Assert.IsFalse(count > 0);
        }
    }
}
