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
        public bool isSpecial;
        public SpecialBlockType specialBlockType;
        public int connectedBlockNumber;

        public Block(int blockIndex, Position2D position)
        {
            this.blockIndex = blockIndex;
            this.position = position;
        }

        public Block(int blockIndex, Position2D position, bool isSpecial, SpecialBlockType specialBlockType, int connectedBlockNumber)
        {
            this.blockIndex = blockIndex;
            this.position = position;
            this.isSpecial = isSpecial;
            this.specialBlockType = specialBlockType;
            this.connectedBlockNumber = connectedBlockNumber;
        }

        public Position2D GetBlockPosition()
        {
            return this.position;
        }
    }

    public enum SpecialBlockType
    {
        Chute,
        Ladder
    }
}
