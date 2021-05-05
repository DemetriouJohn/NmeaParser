namespace NmeaParser
{
    /// <summary>
    /// Represents a single visible satellite in the fix
    /// </summary>
    public class FixSatellite
    {
        internal FixSatellite(int id, int elevation, int azimuth, int snr)
        {
            ID = id;
            Elevation = elevation;
            Azimuth = azimuth;
            SNR = snr;
        }

        /// <summary>
        /// PRN identifier of the satellite
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Elevation in degrees relative to horizon (-90 to 90)
        /// </summary>
        public int Elevation { get; }

        /// <summary>
        /// Azimuth in degrees relative to true north (0 to 359)
        /// </summary>
        public int Azimuth { get; }

        /// <summary>
        /// Signal to noise ratio in dB
        /// </summary>
        public int SNR { get; }
    }
}
