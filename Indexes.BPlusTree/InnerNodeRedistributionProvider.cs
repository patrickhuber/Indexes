using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public class InnerNodeRedistributionProvider<TKey, TValue> : INodeRedistributionProvider<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public void RedistributeFromHighToLow(RedistributeRequest<TKey, TValue> request)
        {
            RedistributeKeysFromHighToLow(request);
            RedistributeChildrenFromHighToLow(request);
        }

        private void RedistributeKeysFromHighToLow(RedistributeRequest<TKey, TValue> request)
        {            
        }

        private void RedistributeChildrenFromHighToLow(RedistributeRequest<TKey, TValue> request)
        {
            throw new NotImplementedException();
        }

        public void RedistributeFromLowToHigh(RedistributeRequest<TKey, TValue> request)
        {            
            RedistributeKeysFromLowToHigh(request);
            RedistributeChildrenFromLowToHigh(request);
        }

        private void RedistributeKeysFromLowToHigh(RedistributeRequest<TKey, TValue> request)
        {
            while (!KeysAreDistributed(request))
            { 
            }
        }

        private void RedistributeChildrenFromLowToHigh(RedistributeRequest<TKey, TValue> request)
        {
            while (!ChildrenAreDistributed(request))
            { 
            }
        }

        public int CalculateMiddleIndex(RedistributeRequest<TKey, TValue> request)
        {
            throw new NotImplementedException();
        }

        private bool KeysAreDistributed(RedistributeRequest<TKey, TValue> request)
        {
            return true;
        }

        private bool ChildrenAreDistributed(RedistributeRequest<TKey, TValue> request)
        {
            return true;
        }
    }
}
