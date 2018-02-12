using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gui_widget_sprite_animation_definition", Tag = "wspr", Size = 0x2C)]
    [TagStructure(Name = "gui_widget_sprite_animation_definition", Tag = "wspr", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    public class GuiWidgetSpriteAnimationDefinition
    {
        public uint AnimationFlags;
        public List<AnimationDefinitionBlock> AnimationDefinition;
        public byte[] Data;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;

        [TagStructure(Size = 0x14)]
        public class AnimationDefinitionBlock
        {
            public uint Frame;
            public short SpriteIndex;
            public short SpriteIndex2;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
        }
    }
}