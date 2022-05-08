using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chute_and_Ladders_with_Arithmetic
{
    public static class GameConfig
    {
        public const double BLOCK_X_SIZE = 55;
        public const double BLOCK_Y_SIZE = 55;

        public const double BOARD_START_X = 20;
        public const double BOARD_START_Y = 535;

        public const int MAX_X = 10;
        public const int MAX_Y = 10;

        public static Dictionary<int, SpecialBlockGameConfig> gameBoardSpecialBlocksConfig =
            new Dictionary<int, SpecialBlockGameConfig>()
            {
                { 1, new SpecialBlockGameConfig(38, SpecialBlockType.Ladder) },
                { 4, new SpecialBlockGameConfig(14, SpecialBlockType.Ladder) },
                { 9, new SpecialBlockGameConfig(31, SpecialBlockType.Ladder) },
                { 21, new SpecialBlockGameConfig(42, SpecialBlockType.Ladder) },
                { 28, new SpecialBlockGameConfig(84, SpecialBlockType.Ladder) },
                { 36, new SpecialBlockGameConfig(44, SpecialBlockType.Ladder) },
                { 51, new SpecialBlockGameConfig(67, SpecialBlockType.Ladder) },
                { 80, new SpecialBlockGameConfig(100, SpecialBlockType.Ladder) },
                { 71, new SpecialBlockGameConfig(91, SpecialBlockType.Ladder) },

                { 16, new SpecialBlockGameConfig(6, SpecialBlockType.Chute) },
                { 47, new SpecialBlockGameConfig(26, SpecialBlockType.Chute) },
                { 49, new SpecialBlockGameConfig(11, SpecialBlockType.Chute) },
                { 56, new SpecialBlockGameConfig(53, SpecialBlockType.Chute) },
                { 62, new SpecialBlockGameConfig(19, SpecialBlockType.Chute) },
                { 64, new SpecialBlockGameConfig(60, SpecialBlockType.Chute) },
                { 87, new SpecialBlockGameConfig(24, SpecialBlockType.Chute) },
                { 93, new SpecialBlockGameConfig(73, SpecialBlockType.Chute) },
                { 95, new SpecialBlockGameConfig(75, SpecialBlockType.Chute) },
                { 98, new SpecialBlockGameConfig(78, SpecialBlockType.Chute) },
            };
    }

    public class SpecialBlockGameConfig
    {
        public int connectedBlockNumber = 0;
        public SpecialBlockType specialBlockType;

        public SpecialBlockGameConfig(int connectedBlockNumber, SpecialBlockType specialBlockType)
        {
            this.connectedBlockNumber = connectedBlockNumber;
            this.specialBlockType = specialBlockType;
        }
    }
}
