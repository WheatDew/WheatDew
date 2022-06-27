using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Origin
{
    public delegate void CommandSubject(string componentKey);

    public class NewCommandSystem : MonoBehaviour
    {

        public static Dictionary<string, NewCommand> commands = new Dictionary<string, NewCommand>();

        public static void Execute(string command)
        {
            string[] slices = command.Split(' ');
            commands[slices[0]].Execute(slices[1]);
        }
    }

    public class NewCommand
    {
        public CommandSubject subject;

        public NewCommand(string commandName,CommandSubject commandSubject)
        {
            subject = commandSubject;
            NewCommandSystem.commands.Add(commandName, this);
            Debug.Log(NewCommandSystem.commands.Count);
        }

        public void Execute(string param)
        {
            subject(param);
        }
    }
}

