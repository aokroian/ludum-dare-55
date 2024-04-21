using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Console
{
    public static class ConsoleHelpers
    {
        private static ConsoleCommand[] _allCommands;

        public static ConsoleCommand[] GetAllCommands()
        {
            return _allCommands ??= Resources.LoadAll<ConsoleCommand>("ConsoleCommands");
        }

        public static bool IsContainsCommand(this List<string> commands, ConsoleCommand commandObj)
        {
            return commands.Where(command => command is not null)
                .Any(command => command == commandObj.mainWord);
        }
    }
}