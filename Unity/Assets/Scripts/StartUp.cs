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
        string path = "C:\\Users\\Documents\\GitHub\\Stage\\Stage\\code";
        string script = "main.py";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python.exe",  // Assuming 'python.exe' is in the system PATH
            Arguments = Path.Combine(path, script),
            WorkingDirectory = path,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.EnableRaisingEvents = true;  // Enable Exited event handling
            process.Exited += (sender, e) =>
            {
                // Perform cleanup or close the current process
                Environment.Exit(0);
            };

            process.Start();

            // Optionally wait for the process to exit
            process.WaitForExit();
        }
    }


}