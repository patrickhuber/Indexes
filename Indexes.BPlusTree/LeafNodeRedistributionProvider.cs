using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public class LeafNodeRedistributionProvider<TKey, TValue> : INodeRedistributionProvider<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public void RedistributeFromHighToLow(RedistributeRequest<TKey, TValue> request)
        {
            throw new NotImplementedException();
        }

        public void RedistributeFromLowToHigh(RedistributeRequest<TKey, TValue> request)
        {
            throw new NotImplementedException();
        }

        public int CalculateMiddleIndex(RedistributeRequest<TKey, TValue> request)
        {
            throw new NotImplementedException();
        }
    }
}
