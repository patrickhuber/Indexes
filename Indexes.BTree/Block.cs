using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BTree
{
    /// <summary>
    /// <see cref="http://people.csail.mit.edu/jaffer/wb/Tree-format.html#Tree-format"/>
    /// </summary>
    public class Block
    {
        public uint Id { get; set; }
        public uint RootId { get; set; }
        public uint NextId { get; set; }



        public uint Time { get; set; }

        public ushort Length { get; set; }
    }
}
