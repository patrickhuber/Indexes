using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BTree
{
    public class BlockSerializer
    {
        public Block Deserialize(Stream stream)
        {
            var block = new Block();
            var reader = new BinaryReader(stream);
            block.Id = reader.ReadUInt32();
            block.RootId = reader.ReadUInt32();
            block.NextId = reader.ReadUInt32();
            block.Time = reader.ReadUInt32();
            block.Length = reader.ReadUInt16();
            block.Level = reader.ReadByte();
            block.Type = reader.ReadByte();

        }
    }
}
