namespace Core.Models.ReportModels
{
    public class ReadPointerModel
    {
        /// <summary>
        ///
        /// </summary>
        public int ExamId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ExamSnapShotId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int OffsetByte { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int RangeOffsetByte { get; set; }
    }
}