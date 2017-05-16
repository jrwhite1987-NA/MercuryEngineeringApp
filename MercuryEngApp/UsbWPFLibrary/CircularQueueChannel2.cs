// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 06-30-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="CircularQueueChannel2.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class CircularQueueChannel2.
    /// </summary>
    public class CircularQueueChannel2
    {
        /// <summary>
        /// The write head
        /// </summary>
        private int writeHead;

        /// <summary>
        /// The read head
        /// </summary>
        private int _readHead;

        /// <summary>
        /// Gets or sets the read head.
        /// </summary>
        /// <value>The read head.</value>
        private int ReadHead
        {
            get
            {
                return _readHead;
            }
            set
            {
                if (value == writeHead + 1)
                {
                    _readHead = -1;
                    writeHead = -1;
                }
                else if (value == 0 & writeHead == (max - 1))
                {
                    _readHead = -1;
                    writeHead = -1;
                }
                else
                {
                    _readHead = value;
                }
            }
        }

        /// <summary>
        /// The maximum
        /// </summary>
        private int max;

        /// <summary>
        /// The queue
        /// </summary>
        private byte[] queue;

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public byte[] Items
        {
            get { return queue; }
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// The thread safe lock channel2
        /// </summary>
        private static Object threadSafeLockChannel2 = new Object();

        /// <summary>
        /// The handle channel2 queue
        /// </summary>
        private static CircularQueueChannel2 handleChannel2Queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularQueueChannel2"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        private CircularQueueChannel2(int size)
        {
            max = size;
            queue = new byte[size];
            ReadHead = -1;
            writeHead = -1;
        }

        /// <summary>
        /// Returns the channel 2 queue
        /// </summary>
        /// <value>The channel2 queue.</value>
        public static CircularQueueChannel2 Channel2Queue
        {
            get
            {
                if (handleChannel2Queue == null)
                {
                    int circularQueueSize = DMIProtocol.PACKET_SIZE * 2;
                    handleChannel2Queue = new CircularQueueChannel2(circularQueueSize);
                }
                return handleChannel2Queue;
            }
        }

        /// <summary>
        /// Dequeues a single element from the current queue
        /// </summary>
        /// <returns>The element dequeued from the queue</returns>
        public byte Dequeue()
        {
            lock (threadSafeLockChannel2)
            {
                byte element;
                if ((ReadHead == writeHead) && (-1 == writeHead))
                {
                    return 0;
                }
                else
                {
                    element = queue[ReadHead];
                    Count--;
                    if (ReadHead == writeHead)
                    {
                        ReadHead = -1;
                        writeHead = -1;
                    }
                    else
                    {
                        ReadHead = (ReadHead + 1) % max;
                    }

                    return element;
                }
            }
        }

        /// <summary>
        /// Enqueues a new element to the queue
        /// </summary>
        /// <param name="element">The byte to be enqueued to the current queue</param>
        public void Enqueue(byte element)
        {
            lock (threadSafeLockChannel2)
            {
                if (ReadHead == (writeHead + 1) % max)
                {
                    return;
                }
                else
                {
                    writeHead = (writeHead + 1) % max;
                    queue[writeHead] = element;
                    Count++;
                    if (ReadHead == -1)
                    {
                        ReadHead = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the first element of the queue
        /// </summary>
        /// <returns>The byte present at the first position of the queue</returns>
        public byte First()
        {
            lock (threadSafeLockChannel2)
            {
                if (ReadHead != -1)
                {
                    return queue[ReadHead];
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns the sync code formed from first 8 bytes
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] PeekSyncCode()
        {
            lock (threadSafeLockChannel2)
            {
                const int COMPARE_COUNT = 7;
                int compareReadHead = max - COMPARE_COUNT;
                byte[] syncCode = new byte[DMIProtocol.SYNC_SIZE];
                int noOfItems = max - ReadHead;
                if (COMPARE_COUNT < Count && -1 != ReadHead)
                {
                    if (ReadHead < compareReadHead)
                    {
                        Array.Copy(queue, ReadHead, syncCode, 0, DMIProtocol.SYNC_SIZE);
                    }
                    else
                    {
                        Array.Copy(queue, ReadHead, syncCode, 0, noOfItems);
                        Array.Copy(queue, 0, syncCode, noOfItems, DMIProtocol.SYNC_SIZE - noOfItems);
                    }
                }
                return syncCode;
            }
        }

        /// <summary>
        /// Returns a packet after dequeueing from the current channel queue
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] DequeuePacket()
        {
            lock (threadSafeLockChannel2)
            {
                byte[] packetData = new byte[DMIProtocol.PACKET_SIZE];
                int noOfItems = max - ReadHead;

                if (DMIProtocol.PACKET_SIZE - 1 < Count && -1 != ReadHead)
                {
                    if (ReadHead <= DMIProtocol.PACKET_SIZE)
                    {
                        Array.Copy(queue, ReadHead, packetData, 0, DMIProtocol.PACKET_SIZE);
                    }
                    else
                    {
                        Array.Copy(queue, ReadHead, packetData, 0, noOfItems);
                        Array.Copy(queue, 0, packetData, noOfItems, DMIProtocol.PACKET_SIZE - noOfItems);
                    }
                    ReadHead = (ReadHead + DMIProtocol.PACKET_SIZE) % max;

                    Count -= DMIProtocol.PACKET_SIZE;
                    return packetData;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}