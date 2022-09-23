using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate string DriveRule();
public delegate void DriveCache();
public class DriveData
{

}

public class CDrive : MonoBehaviour
{
    [HideInInspector] public string key;
    public Dictionary<string, DriveRule> rules = new Dictionary<string, DriveRule>();
    public Dictionary<string, DriveCache> caches = new Dictionary<string, DriveCache>();
    public Dictionary<string, DriveRule> refrules = new Dictionary<string, DriveRule>();

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        Init();

    }

    virtual public void Init()
    {

    }

    //¹¦ÄÜº¯Êý
    public void AddDriveData(string address, DriveRule driveDataFunction)
    {
        rules.Add(address, driveDataFunction);
    }

    public void AddDriveCache(string commandName, string commandString)
    {
        string[] commands = commandString.Split('\n');

        for (int i = 0; i < commands.Length; i++)
        {
            string[] values = commands[i].Split(' ');
            DriveRule[] driveRules = new DriveRule[values.Length];
            for (int n = 0; n < values.Length; n++)
            {
                if (rules.ContainsKey(values[n]))
                {
                    driveRules[n] = rules[values[n]];
                }
                else
                {
                    string s = values[n];
                    driveRules[n] = () => s;
                }
            }
            if (caches.ContainsKey(commandName))
            {
                caches[commandName] += () =>
                {
                    string[] values = new string[driveRules.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = driveRules[i]();
                    }
                    //SCommand.Execute(values);
                };
            }
            else
            {
                caches.Add(commandName, () =>
                {
                    string[] values = new string[driveRules.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = driveRules[i]();
                    }
                    //SCommand.Execute(values);
                });
            }

        }

    }


}


