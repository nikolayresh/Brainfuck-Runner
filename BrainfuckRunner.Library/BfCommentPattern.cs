using System;

namespace BrainfuckRunner.Library
{
    public sealed class BfCommentPattern
    {
        public static readonly string PresetStartTag = "//";

        public string StartTag { get; set; } = PresetStartTag;

        public string EndTag { get; set; } = Environment.NewLine;
    }
}
