using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VertexUnityPlayer;

public class ComponentWindow : MonoBehaviour
{

    public FloatingButton RecordButton;
    public FloatingButton PlayButton;

    public TextMesh ComponentName;
    public TextMesh ComponentDescription;
    bool isRecording = false;
    static bool isUploading = false;
    static AudioSource AudioSrc;

    void Start()
    {
        // Get the close button and listen for close events
        RecordButton.Clicked += RecordButton_Clicked;
        PlayButton.Clicked += PlayButton_Clicked;

        AudioSrc = gameObject.GetComponent<AudioSource>();

    }

    private void PlayButton_Clicked(GameObject button)
    {
        Debug.Log("PlayButton_Clicked!");
        GetAndPlayRecording();
    }

    private void RecordButton_Clicked(GameObject button)
    {
        Debug.Log("Record button clicked!");

        if (!isRecording)
        {
            RecordMessage();
        }
    }

    private void CloseButton_Activated(GameObject source)
    {
        Hide();
    }

    // hide panel windows
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // show panel window
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Methods for Recording functionality
    public void GetAndPlayRecording()
    {
        StartCoroutine(StopRecording());
        AudioSrc.Play();
        StartCoroutine(GetRecording());
    }

    public void RecordMessage()
    {
        StopAllCoroutines();
        StartCoroutine(StartRecording());
    }

    // Coroutine to start recording audio
    IEnumerator StartRecording()
    {
        isRecording = true;
        isUploading = true;
        AudioSrc.clip = Microphone.Start(Microphone.devices[0], false, 15, 44100);
        while (Microphone.IsRecording(Microphone.devices[0]))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Microphone.End(Microphone.devices[0]);
        isRecording = false;
        // Start Upload coroutine once recording is finished.
        yield return UploadAudioOnServer();
    }

    // Coroutine to upload recorded audio on server
    IEnumerator UploadAudioOnServer()
    {
        float[] samples = new float[AudioSrc.clip.samples * AudioSrc.clip.channels];
        AudioSrc.clip.GetData(samples, 0);

        var byteArray = new byte[samples.Length * 4];
        Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);

        // Post recorded audio clips on server using UnityWebRequest
        using (UnityWebRequest WebRequest = UnityWebRequest.Post("https://staging.vertx.cloud/core/v1.0/resource/088c0839-d2d1-4808-87d4-a33ca223876e/audioClip.fbx", ""))
        {
            WebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            WebRequest.uploadHandler = new UploadHandlerRaw(byteArray);
            WebRequest.uploadHandler.contentType = "application/octet-stream";
            WebRequest.downloadHandler = new DownloadHandlerBuffer();
            WebRequest.AddVertexAuth();
            yield return WebRequest.SendWebRequest();
            Debug.Log("Audio file Uploaded");
        }
        yield return null;
        isUploading = false;
    }

    IEnumerator StopRecording()
    {
        isRecording = false;
        Microphone.End(Microphone.devices[0]);
        yield return null;
    }

    IEnumerator GetRecording()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://staging.vertx.cloud/core/v1.0/resource/088c0839-d2d1-4808-87d4-a33ca223876e/audioClip.fbx"))
        {
            www.SetRequestHeader("Content-Type", "application/octet-stream");
            www.AddVertexAuth();
            while (isUploading)
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                float[] downloadArray = new float[results.Length / 4];
                Buffer.BlockCopy(results, 0, downloadArray, 0, results.Length);
                AudioSrc.clip.SetData(downloadArray, 0);
                Debug.Log("Audio file downloaded");
                yield return new WaitForSeconds(7f);
            }
            AudioSrc.Play();
            Debug.Log("Downloaded Audio Playing");
        }
        yield return new WaitForSeconds(7f);
    }

}
