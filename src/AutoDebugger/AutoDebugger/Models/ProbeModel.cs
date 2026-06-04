// ReSharper disable All
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AutoDebugger.Models
{
    internal class ProbeModel
    {
        internal class Attributes
        {
            public bool disabled { get; set; }
            public Metadata metadata { get; set; }
            public Probe probe { get; set; }
        }

        internal class Capture
        {
            public int max_reference_depth { get; set; }
        }

        internal class Data
        {
            public string type { get; set; }
            public Attributes attributes { get; set; }
        }

        internal class Enablement
        {
            public List<Query> queries { get; set; }
        }

        internal class Metadata
        {
            public string service_name { get; set; }
            public string type { get; set; }
            public Enablement enablement { get; set; }
        }

        internal class Probe
        {
            public Capture capture { get; set; }
            public string template { get; set; }
            public List<Segment> segments { get; set; }
            public bool capture_snapshot { get; set; }
            public Sampling sampling { get; set; }
            public string language { get; set; }
            public Where where { get; set; }
            public string evaluate_at { get; set; }
            public List<object> tags { get; set; }
        }

        internal class Query
        {
            public int limit { get; set; }
            public string text { get; set; }
            public List<Tag> tags { get; set; }
        }

        internal class Root
        {
            public Data data { get; set; }
            public string _authentication_token { get; set; }
        }

        internal class Sampling
        {
            public int snapshots_per_second { get; set; }
        }

        internal class Segment
        {
            public string str { get; set; }
        }

        internal class Tag
        {
            public string key { get; set; }
            public List<Value> values { get; set; }
        }

        internal class Value
        {
            public string value { get; set; }
            public bool is_excluded { get; set; }
        }

        internal class Where
        {
            public string source_file { get; set; }
            public List<string> lines { get; set; }
        }
    }

    internal class ProbeDefinition
    {
        public string sourceFile { get; set; }
        public string line { get; set; }
    }
}
