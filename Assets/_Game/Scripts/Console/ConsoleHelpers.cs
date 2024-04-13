using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Console
{
    public static class ConsoleHelpers
    {
        public static ConsoleCommand[] GetAllCommands()
        {
            return Resources.LoadAll<ConsoleCommand>("ConsoleCommands");
        }

        public static bool IsContainsCommand(this IEnumerable<ConsoleCommand> commands, ConsoleCommand commandObj)
        {
            return commands.Where(command => command is not null)
                .Any(command => command.mainWord == commandObj.mainWord);
        }
    }
}