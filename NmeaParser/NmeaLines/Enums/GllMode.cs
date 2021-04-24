namespace NmeaParser.NmeaLines.Enums
{
    public enum GllMode
    {
        /// <summary>
        /// Autonomous mode
        /// </summary>
        Autonomous,
        /// <summary>
        ///  Differential mode
        /// </summary>
        Differential,
        /// <summary>
        ///  Estimated (dead reckoning) mode
        /// </summary>
        EstimatedDeadReckoning,
        /// <summary>
        /// Manual input mode
        /// </summary>
        Manual,
        /// <summary>
        /// Simulator mode
        /// </summary>
        Simulator,
        /// <summary>
        /// Data not valid
        /// </summary>
        DataNotValid
    }
}