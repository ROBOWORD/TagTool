using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "input_globals", Tag = "inpg", Size = 0x34)]
    public class InputGlobals
    {
        public int Unknown;
        public float Unknown2;
        public byte[] Unknown3;
        public byte[] Unknown4;
        public int Unknown5;
    }
}