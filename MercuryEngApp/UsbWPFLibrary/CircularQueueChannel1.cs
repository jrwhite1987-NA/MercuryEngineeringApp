// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 06-30-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="CircularQueueChannel1.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class CircularQueueChannel1.
    /// </summary>
    public class CircularQueueChannel1
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
        /// The reset value
        /// </summary>
        private const int RESET_VALUE = -1;

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
                    _readHead = RESET_VALUE;
                    writeHead = RESET_VALUE;
                }
                else if (value == 0 & writeHead == (max - 1))
                {
                    _readHead = RESET_VALUE;
                    writeHead = RESET_VALUE;
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
        /// The thread safe lock channel1
        /// </summary>
        private static Object threadSafeLockChannel1 = new Object();

        /// <summary>
        /// The handle channel1 queue
        /// </summary>
        private static CircularQueueChannel1 handleChannel1Queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularQueueChannel1"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        private CircularQueueChannel1(int size)
        {
            max = size;
            queue = new byte[size];
            ReadHead = RESET_VALUE;
            writeHead = RESET_VALUE;
        }

        /// <summary>
        /// Returns the channel 2 queue
        /// </summary>
        /// <value>The channel1 queue.</value>
        public static CircularQueueChannel1 Channel1Queue
        {
            get
            {
                if (handleChannel1Queue == null)
                {
                    int circularQueueSize = DMIProtocol.PACKET_SIZE * 2;
                    handleChannel1Queue = new CircularQueueChannel1(circularQueueSize);
                }
                return handleChannel1Queue;
            }
        }

        /// <summary>
        /// Dequeues a single element from the current queue
        /// </summary>
        /// <returns>The element dequeued from the queue</returns>
        public byte Dequeue()
        {
            lock (threadSafeLockChannel1)
            {
                byte element;
                if ((ReadHead == writeHead) && (writeHead == RESET_VALUE))
                {
                    return Constants.VALUE_0;
                }
                else
                {
                    element = queue[ReadHead];
                    Count--;
                    if (ReadHead == writeHead)
                    {
                        ReadHead = RESET_VALUE;
                        writeHead = RESET_VALUE;
                    }
                    else
                    {
                        ReadHead = (++ReadHead) % max;
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
            lock (threadSafeLockChannel1)
            {
                if (ReadHead == (writeHead + 1) % max)
                {
                    return;
                }
                else
                {
                    writeHead = (++writeHead) % max;
                    queue[writeHead] = element;
                    Count++;
                    if (ReadHead == RESET_VALUE)
                    {
                        ReadHead = Constants.VALUE_0;
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
            lock (threadSafeLockChannel1)
            {
                if (ReadHead != RESET_VALUE)
                {
                    return queue[ReadHead];
                }
                else
                {
                    return Constants.VALUE_0;
                }
            }
        }

        /// <summary>
        /// Returns the sync code formed from first 8 bytes
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] PeekSyncCode()
        {
            lock (threadSafeLockChannel1)
            {
                byte[] syncCode = new byte[DMIProtocol.SYNC_SIZE];
                int noOfItems = max - ReadHead;

                const int COMPARE_COUNT = 7;
                int compareReadHead = max - COMPARE_COUNT;

                if (Count >= DMIProtocol.SYNC_SIZE && RESET_VALUE != ReadHead)
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
            lock (threadSafeLockChannel1)
            {
                byte[] packetData = new byte[DMIProtocol.PACKET_SIZE];
                int noOfItems = max - ReadHead;

                if (Count >= DMIProtocol.PACKET_SIZE && RESET_VALUE != ReadHead)
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