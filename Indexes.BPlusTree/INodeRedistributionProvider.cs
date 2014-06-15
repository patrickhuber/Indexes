using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public interface INodeRedistributionProvider<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        void RedistributeFromHighToLow(RedistributeRequest<TKey, TValue> request);
        void RedistributeFromLowToHigh(RedistributeRequest<TKey, TValue> request);        
    }
}
