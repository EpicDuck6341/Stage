using System.Diagnostics;
using System.IO;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private Process process;

    private void Start()
    {
        StartPython();
    }

    //Starts the the local flask server with the speech recognition
    private void StartPython()
    {
        string path = Path.GetFullPath("..\\code");
        string script = "main.py";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python.exe", // Assuming 'python.exe' is in the system PATH
            Arguments = Path.Combine(path, script),
            WorkingDirectory = path,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        process = new Process { StartInfo = startInfo };

        process.Start();
    }

    private void Update()
    {
        if (process != null && process.HasExited)
        {
            OnApplicationQuit();
        }
    }

    private void OnApplicationQuit()
    {
        if (process != null && !process.HasExited)
        {
            // Stop the process if it's still running
            process.Kill();
            process.WaitForExit(); // Wait for the process to finish cleaning up resources
        }
    }
}