using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Enums
{
    public static class StandardInk
    {
        public const int Basic = 0;
        public const int Common = 40;
        public const int Rare = 100;
        public const int Epic = 300;
        public const int Mythic = 600;
        public const int Fabled = 1600;

        public static int GetInkCost(RarityType type)
        {
            if(type == RarityType.Basic)
            {
                return Basic;
            }
            else if(type == RarityType.Common)
            {
                return Common;
            }
            else if(type == RarityType.Rare)
            {
                return Rare;
            }
            else if(type == RarityType.Epic)
            {
                return Epic;
            }
            else if(type == RarityType.Mythic)
            {
                return Mythic;
            }
            else if(type == RarityType.Fabled)
            {
                return Fabled;
            }
            else
            {
                throw (new ArgumentException());
            }
        }
    }

    public static class FancyInk
    {
        public const int Basic = 0;
        public const int Common = 400;
        public const int Rare = 800;
        public const int Epic = 1500;
        public const int Mythic = 2400;
        public const int Fabled = 3200;
    }
}
