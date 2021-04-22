namespace NmeaParser
{
    /// <summary>
    /// Represents information about the current fix
    /// </summary>
    public class FixData
    {
        internal FixData(bool isAuto, FixStatus status, double pdop, double hdop, double vdop, string[] satelliteIds)
        {
            IsAuto = isAuto;
            Status = status;
            PDOP = pdop;
            HDOP = hdop;
            VDOP = vdop;
            SatelliteIds = satelliteIds ?? new string[0];
        }

        /// <summary>
        /// Is the receiver in automatic fix mode
        /// </summary>
        public bool IsAuto { get; }

        /// <summary>
        /// Current fix status 
        /// </summary>
        public FixStatus Status { get; }

        /// <summary>
        /// Primary dilution of precision
        /// </summary>
        public double PDOP { get; }

        /// <summary>
        /// Horizontal dilution of precision
        /// </summary>
        public double HDOP { get; }

        /// <summary>
        /// Vertical dilution of precision
        /// </summary>
        public double VDOP { get; }

        /// <summary>
        /// Does this fix mode represent a satellite fix
        /// </summary>
        public bool IsFixed => Status != FixStatus.None;

        /// <summary>
        /// Satellite IDs used in the current fix
        /// </summary>
        public string[] SatelliteIds { get; }
    }

    /// <summary>
    /// Type of fix
    /// </summary>
    public enum FixStatus
    {
        /// <summary>
        /// No satellite fix
        /// </summary>
        None,
        /// <summary>
        /// Satellite fix acquired, 2D only (low accuracy)
        /// </summary>
        /// <remarks>
        /// Typically, this means that the receiver only has a fix on 3 or fewer satellites
        /// </remarks>
        Fixed2D,
        /// <summary>
        /// Satellite fix acquired, 3D only (high accuracy)
        /// </summary>
        /// <remarks>
        /// Typically, this means that the receiver has a fix on more than 3 satellites.
        /// </remarks>
        Fixed3D,
    }
}
