using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chute_and_Ladders_with_Arithmetic
{
    internal class Block
    {
        public int blockIndex;
        public Position2D position;
        public Block(int blockIndex, Position2D position)
        {
            this.blockIndex = blockIndex;
            this.position = position;
        }

        public Position2D GetBlockPosition()
        {
            return this.position;
        }
    }
}
