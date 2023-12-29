using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Algorithms;

public class KargersMinCutImpl<TVertex> where TVertex : IComparable<TVertex>
{
    private int _verticesCount;
    private int _edgesCount;

    private int[] _parentIds;
    private int[] _rank;

    private Random _rd = new Random();

    public KargersMinCutImpl(int vCount, int eCount)
    {
        _verticesCount = vCount;
        _edgesCount = eCount;

        _parentIds = new int[vCount];
        _rank = new int[vCount];
        
        for (var i = 0; i < vCount; i++)
        {
            _parentIds[i] = i;
            _rank[i] = 0;
        }
    }
    
    public MinCut<TVertex> ComputeMinCut(Graph<TVertex, int> g){
        var first = new Graph<TVertex, int>();
        var second = new Graph<TVertex, int>();
        
        //todo: traverse and add vertices to sub graphs based on parent
        
        var vertices = _verticesCount;

        while (vertices>2)
        {
            var rdV = _rd.Next(_verticesCount);
            var rdEdges = g.GetEdges(rdV);
            
            var set1 = FindParentId(rdV);

            var rdEInd = _rd.Next(rdEdges.Count);

            var set2 = FindParentId(rdEdges[rdEInd].DestinationVertexId);

            if (set1 != set2)
            {
                Union(rdV, rdEdges[rdEInd].DestinationVertexId);
                vertices--;
            }
        }
        
        int cut = 0;
        
        for (var i = 0; i < _verticesCount; i++)
        {
            var set1 = FindParentId(i);
            
            var iEdges = g.GetEdges(i);

            for (var j = 0; j < iEdges.Count; j++)
            {
                var set2 = FindParentId(iEdges[j].DestinationVertexId);

                if (set1 != set2)
                {
                    cut += iEdges[j].Value;
                }
            }
        }
        
        return new MinCut<TVertex>(first, second, cut);
    }

    public int FindParentId(int vId)
    {
        if (vId == _parentIds[vId]) return vId;
        
        return _parentIds[vId] = FindParentId(_parentIds[vId]);
    }
    
    public void Union(int V1Ind, int V2Ind)
    {
        V1Ind = FindParentId(V1Ind);
        V2Ind = FindParentId(V2Ind);
        
        if (V1Ind != V2Ind)
        {
            if(_rank[V1Ind] < _rank[V2Ind])
            {
                (V1Ind, V2Ind) = (V2Ind, V1Ind);
            }
            
            _parentIds[V2Ind] = V1Ind;
            
            if(_rank[V1Ind] == _rank[V2Ind])
                _rank[V1Ind]++;
        }
    }
}