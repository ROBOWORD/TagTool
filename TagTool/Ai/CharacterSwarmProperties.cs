using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x38)]
    public class CharacterSwarmProperties : TagStructure
	{
        public short ScatterKilledCount;

        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Unused;

        public float ScatterRadius;
        public float ScatterTime;
        public Bounds<float> HoundDistance;
        public Bounds<float> InfectionTime;
        public float PerlinOffsetScale;
        public Bounds<float> OffsetPeriod;
        public float PerlinIdleMovementThreshold;
        public float PerlinCombatMovementThreshold;
        public float StuckTime;
        public float StuckDistance;
    }
}
