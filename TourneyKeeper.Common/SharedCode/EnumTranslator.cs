using System;
using System.Linq;
using TourneyKeeper.Common.Exceptions;

namespace TourneyKeeper.Common.EnumTranslator
{
    public static class EnumTranslator
    {
        public static string Translate(RequiredToReportEnum value)
        {
            switch(value)
            {
                case RequiredToReportEnum.PlayerInGame:
                    return "Player in game";
                case RequiredToReportEnum.PlayerInTournament:
                    return "Player in tournament";
                default:
                    throw new Exception("Unknown reporting enum value");
            }
        }
    }
}
