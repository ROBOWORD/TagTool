﻿using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class CreateTagCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public CreateTagCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "CreateTag",
                  "Creates a new tag of the specified tag group in the current tag cache.",

                  "CreateTag <group tag> [index = *]",

                  "Creates a new tag of the specified tag group in the current tag cache.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

        begin:
            var groupTagString = args[0];

            if (groupTagString.Length > 4)
            {
                Console.WriteLine($"ERROR: Invalid group tag: {groupTagString}");
                return true;
            }

            var groupTag = Tag.Null;

            try
            {
                groupTag = CacheContext.ParseGroupTag(groupTagString);
            }
            catch (KeyNotFoundException)
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };

                for (var i = 0; i < chars.Length; i++)
                    chars[i] = groupTagString[i];

                groupTag = new Tag(new string(chars));
            }

            if (!TagGroup.Instances.ContainsKey(groupTag))
            {
                Console.WriteLine($"ERROR: No tag group definition for group tag '{groupTag}'!");
                Console.Write($"(BE CAREFUL WITH THIS!!!) Define '{groupTag}' tag group? [y/n]: ");

                var answer = Console.ReadLine().ToLower();

                if (answer != "y" && answer != "yes")
                    return true;

                Console.WriteLine("Enter the tag group specification in the following format");
                Console.WriteLine("<group tag> [parent group tag] [grandparent group tag] <group name>:");
                Console.WriteLine();
                Console.Write($"{groupTag} specification> ");

                answer = Console.ReadLine();

                var groupArgs = ArgumentParser.ParseCommand(answer, out string redirect);

                switch (groupArgs.Count)
                {
                    case 2: new TagGroup(new Tag(groupArgs[0]), Tag.Null, Tag.Null, CacheContext.GetStringId(groupArgs[1])); break;
                    case 3: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), Tag.Null, CacheContext.GetStringId(groupArgs[2])); break;
                    case 4: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), new Tag(groupArgs[2]), CacheContext.GetStringId(groupArgs[3])); break;
                    default: return false;
                }

                goto begin;
            }

            CachedTagInstance instance = null;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                if (args.Count == 2)
                {
                    var tagIndex = -1;

                    if (!CacheContext.TryGetTag(args[1], out var tag))
                    {
                        if (args[1].StartsWith("0x"))
                            tagIndex = Convert.ToInt32(args[1], 16);
                        else
                            return false;
                    }
                    else
                    {
                        tagIndex = tag.Index;
                    }

                    while (tagIndex >= CacheContext.TagCache.Index.Count)
                        CacheContext.TagCache.AllocateTag();

                    if (tagIndex < CacheContext.TagCache.Index.Count)
                    {
                        if (CacheContext.TagCache.Index[tagIndex] != null)
                        {
                            var oldInstance = CacheContext.TagCache.Index[tagIndex];
                            CacheContext.TagCache.Index[tagIndex] = null;
                            CacheContext.TagCache.SetTagDataRaw(stream, oldInstance, new byte[] { });
                        }

                        instance = new CachedTagInstance(tagIndex, TagGroup.Instances[groupTag]);
                        CacheContext.TagCache.Index[tagIndex] = instance;
                    }
                }

                if (instance == null)
                    instance = CacheContext.TagCache.AllocateTag(TagGroup.Instances[groupTag]);

                CacheContext.Serialize(stream, instance, Activator.CreateInstance(TagDefinition.Find(groupTag)));
            }

            var tagName = instance.Name ?? $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}");

            return true;
        }
    }
}
