using System;

namespace TourneyKeeper.Common.Managers.TableGenerators
{
    internal static class TableGeneratorFactory
    {
        internal static ITableGenerator GetGenerator(TableGenerator tableGenerator)
        {
            switch (tableGenerator)
            {
                case TableGenerator.Linear:
                    return new TableGeneratorLinear();
                case TableGenerator.Random:
                    return new TableGeneratorRandom();
                case TableGenerator.Unique:
                    return new TableGeneratorUnique();
                default:
                    LogManager.LogError("No such table generator");
                    throw new Exception("No such table generator");
            }
        }
    }
}