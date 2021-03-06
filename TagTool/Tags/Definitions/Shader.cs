using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "shader", Tag = "rmsh", Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "shader", Tag = "rmsh", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class Shader : RenderMethod
    {
        public StringId Material;

        [TagField(Flags = TagFieldFlags.Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;
    }
}