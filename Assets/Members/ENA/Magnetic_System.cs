using UnityEngine;

namespace Members.ENA
{
    public enum MagnetDir
    {
        ClockWise,
        CounterClockwise
    }
    public class MagneticSystem
    {
        public bool magnetic;

        public string Magnetic(bool value)
        {
            magnetic = value;
            if (magnetic)
            {
                return nameof(MagnetDir.ClockWise);
            }
            return nameof(MagnetDir.CounterClockwise);
        }
    }
}
