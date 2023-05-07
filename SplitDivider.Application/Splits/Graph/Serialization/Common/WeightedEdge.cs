using System.Xml.Serialization;
using QuikGraph;

namespace SplitDivider.Application.Splits.Graph.Serialization.Common;

public class WeightedEdge<TVertex, TWeight> : Edge<TVertex>
{
    [XmlAttribute("weight")]
    public TWeight Weight { get; set; }

    public WeightedEdge(TVertex source, TVertex target)
        : base(source, target) { }

    public WeightedEdge(TVertex source, TVertex target, TWeight weight)
        : this(source, target)
    {
        Weight = weight;
    }
}