using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsbTcdLibrary.CommunicationProtocol;

namespace MercuryEngApp.Test.UsbWPFLibrary.CommunicationProtocol
{
    [TestClass]
    public class TCDTest
    {
        [TestMethod]
        public void TCDReadInfoResponseTest()
        {
            TCDReadInfoResponse readInfoResponse = new TCDReadInfoResponse();
            Assert.IsTrue(readInfoResponse != null);
        }

        [TestMethod]
        public void TCDRequestTest()
        {
            TCDRequest tcdRequest = new TCDRequest();
            Assert.IsTrue(tcdRequest != null);
        }

        [TestMethod]
        public void TCDRequestClearValuesTest()
        {
            TCDRequest tcdRequest = new TCDRequest();
            tcdRequest.ClearValues();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TCDResponseTest()
        {
            TCDResponse tcdResponse = new TCDResponse();
            Assert.IsTrue(tcdResponse != null);
        }

        [TestMethod]
        public void TCDWriteInfoRequestTest()
        {
            TCDWriteInfoRequest writeInfoRequest =
                new TCDWriteInfoRequest();
            Assert.IsTrue(writeInfoRequest != null);
        }
    }
}