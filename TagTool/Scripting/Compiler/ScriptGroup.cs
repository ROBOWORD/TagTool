﻿namespace TagTool.Scripting.Compiler
{
    public class ScriptGroup : IScriptSyntax
    {
        public IScriptSyntax Head;
        public IScriptSyntax Tail;
    }
}