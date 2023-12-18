using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StartUp : MonoBehaviour
{
    private void Start()
    {
        startPython();
    }

    private void startPython()
    {
        string path = "C:\\Users\\kaijs\\Documents\\GitHub\\Stage\\Stage\\code";
        string script = "main.py";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python.exe",
            Arguments = Path.Combine(path, script),
            WorkingDirectory = path,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.Start();
        }
    }

}