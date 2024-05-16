using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LLM;
using UnityEngine.Networking;

public class OpenAITextToSpeech : TTS
{
    #region ��������

    [SerializeField] private string api_key=string.Empty;//apikey
    [SerializeField] private ModelType m_ModelType = ModelType.tts_1;//ģ��
    [SerializeField] private VoiceType m_Voice = VoiceType.onyx;//����

    #endregion
    private void Awake()
    {
        m_PostURL = "https://api.openai.com/v1/audio/speech";
    }

    /// <summary>
    /// �����ϳɣ����غϳ��ı�
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_callback"></param>
    public override void Speak(string _msg, Action<AudioClip, string> _callback)
    {
        StartCoroutine(GetVoice(_msg, _callback));
    }

    private IEnumerator GetVoice(string _msg, Action<AudioClip, string> _callback)
    {
        stopwatch.Restart();
        using (UnityWebRequest request = UnityWebRequest.Post(m_PostURL, new WWWForm()))
        {
            PostData _postData = new PostData
            {
                model = m_ModelType.ToString().Replace('_','-'),
                input = _msg,
                voice= m_Voice.ToString()
            };

            string _jsonText = JsonUtility.ToJson(_postData).Trim();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerAudioClip(m_PostURL, AudioType.MPEG);

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", api_key));

            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                AudioClip audioClip = ((DownloadHandlerAudioClip)request.downloadHandler).audioClip;
                _callback(audioClip, _msg);

            }
            else
            {
                Debug.LogError("�����ϳ�ʧ��: " + request.error);
            }

            stopwatch.Stop();
            Debug.Log("openAI�����ϳɣ�" + stopwatch.Elapsed.TotalSeconds);
        }
    }

    #region ���ݶ���

    /// <summary>
    /// ���͵ı���
    /// </summary>
    [Serializable]
    public class PostData
    {
        public string model = string.Empty;//ģ������
        public string input = string.Empty;//�ı�����
        public string voice = string.Empty;//����
    }
    /// <summary>
    /// ģ������
    /// </summary>
    public enum ModelType
    {
        tts_1,
        tts_1_hd
    }
    /// <summary>
    /// ��������
    /// </summary>
    public enum VoiceType
    {
        alloy,
        echo,
        fable,
        onyx,
        nova,
        shimmer
    }

    #endregion

}
