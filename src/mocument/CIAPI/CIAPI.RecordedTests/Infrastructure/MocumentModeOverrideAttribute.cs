using System;

namespace CIAPI.RecordedTests.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MocumentModeOverrideAttribute : Attribute
    {
        public MocumentMode Mode { get; set; }
        public MocumentModeOverrideAttribute(MocumentMode mode)
        {
            Mode = mode;
        }
    }
    public enum MocumentMode
    {
        Record,
        Play
    }
}
