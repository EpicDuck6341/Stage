using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class RecordAudio : MonoBehaviour
{
    private AudioClip recordedAudio;
    private string microphoneDevice;
    private bool isRecording = false;
    private CountdownManager CM;
    private int duration = 8;
    private bool recBusy = false;
    private HttpClientUnity http;
    [HideInInspector]
    public int count;

    void Start()
    {
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        http = GameObject.Find("AudioManager").GetComponent<HttpClientUnity>();
    }

    void Update()
    {
        if (CM.isStarted && !recBusy)
        {
            recBusy = true;
            // Choose a microphone device (you can enumerate available devices)
            microphoneDevice = Microphone.devices[0]; // Change index as needed

            // Start recording with a specified device, length, and frequency
            recordedAudio = Microphone.Start(microphoneDevice, false, duration, 44100);

            // Wait for the specified duration to complete recording
            StartCoroutine(WaitForRecordingToFinish());
        }
    }

    IEnumerator WaitForRecordingToFinish()
    {
        yield return new WaitForSeconds(duration);

        // Stop recording
        Microphone.End(microphoneDevice);

        // Save the recorded audio with a unique filename
        string folderPath = Path.Combine(Application.dataPath, "Audio", "Recordings");
        string baseFileName = "givenAnswer.wav";
        string filePath = GenerateUniqueFileName(folderPath, baseFileName);
        SaveRecording(filePath);
        
        recBusy = false;
        http.sendAudio();
    }

    string GenerateUniqueFileName(string folderPath, string baseFileName)
    {
        string fullPath = Path.Combine(folderPath, baseFileName);
        count = 1;

        while (File.Exists(fullPath))
        {
            baseFileName = $"givenAnswer{count}.wav";
            fullPath = Path.Combine(folderPath, baseFileName);
            count++;
        }
        return fullPath;
    }

    void SaveRecording(string filePath)
    {
        // Create a new empty WAV file
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fileStream);

        // Convert the AudioClip to a float array
        float[] samples = new float[recordedAudio.samples];
        recordedAudio.GetData(samples, 0);

        // Write the WAV header
        writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
        writer.Write(samples.Length * 2 + 36);
        writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[4] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((ushort)1);
        writer.Write((ushort)recordedAudio.channels);
        writer.Write(recordedAudio.frequency);
        writer.Write(recordedAudio.frequency * recordedAudio.channels * 2);
        writer.Write((ushort)(recordedAudio.channels * 2));
        writer.Write((ushort)16);
        writer.Write(new char[4] { 'd', 'a', 't', 'a' });
        writer.Write(samples.Length * 2);

        // Write audio data
        foreach (float sample in samples)
        {
            writer.Write((short)(sample * 32767.0f));
        }

        // Clean up
        writer.Close();
        fileStream.Close();
    }
}
