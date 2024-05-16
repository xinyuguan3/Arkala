using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KnowledgeBaidu : LLM
{

    #region �������
    [SerializeField] private string m_ApiKey = string.Empty;//��Կ
    [SerializeField] private string m_ConversationID = string.Empty;//�Ի�ID
    #endregion

    private void Awake()
    {
        url = "https://appbuilder.baidu.com/rpc/2.0/cloud_hub/v1/ai_engine/agi_platform/v1/instance/integrated";
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <returns></returns>
    public override void PostMsg(string _msg, Action<string> _callback)
    {
        //���淢�͵���Ϣ�б�
        m_DataList.Add(new SendData("user", _msg));
        StartCoroutine(Request(_msg, _callback));
    }

    /// <summary>
    /// ��������
    /// </summary> 
    /// <param name="_postWord"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public override IEnumerator Request(string _postWord, System.Action<string> _callback)
    {
        stopwatch.Restart();
        string jsonPayload = JsonConvert.SerializeObject(new RequestData
        {
            query = _postWord,
            conversation_id = m_ConversationID
        });

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-Appbuilder-Authorization", string.Format("Bearer {0}", m_ApiKey));

            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                string _msg = request.downloadHandler.text;
                ResponseData response = JsonConvert.DeserializeObject<ResponseData>(_msg);

                if (response.code == 0)
                {
                    string _msgBack = response.result.answer;
                    m_ConversationID = response.result.conversation_id;
                    //��Ӽ�¼
                    m_DataList.Add(new SendData("assistant", _msgBack));
                    //�ص�
                    _callback(_msgBack);
                }
                else
                {
                    LogError(response.code);
                }
            }
            else
            {
                Debug.Log(request.error);
            }

        }

        stopwatch.Stop();
        Debug.Log("����֪ʶ��ظ���ʱ��" + stopwatch.Elapsed.TotalSeconds);
    }

    /// <summary>
    /// ��ӡ������Ϣ
    /// </summary>
    /// <param name="_code"></param>
    private void LogError(int _code)
    {
        if (_code == 400)
        {
            Debug.Log("�����������");
            return;
        }
        if (_code == 401)
        {
            Debug.Log("Ȩ�޴���");
            return;
        }
        if (_code == 404)
        {
            Debug.Log("�˻���Ӧ�á�ģ�͡�ģ����޷��ҵ�");
            return;
        }
        if (_code == 500)
        {
            Debug.Log("�������ڲ�����");
            return;
        }
        if (_code == 1001)
        {
            Debug.Log("���ó��ޣ���Ѷ�Ȳ���");
            return;
        }
        if (_code == 1004)
        {
            Debug.Log("ģ�ͷ��񱨴�");
            return;
        }
        if (_code == 1005)
        {
            Debug.Log("ģ�����У�����");
            return;
        }
        if (_code == 1006)
        {
            Debug.Log("��Ѷ���ѹ���");
            return;
        }
        if (_code == 1007)
        {
            Debug.Log("ǧ�������޷�����");
            return;
        }
        if (_code == 1008)
        {
            Debug.Log("ǧ���������ʧ�ܣ�һ����Ȩ�޴���");
            return;
        }
    }


    #region ���ݶ���
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]public class RequestData
    {
        [SerializeField] public string query=string.Empty;//��������
        [SerializeField] public string response_mode = "blocking";//��Ӧģʽ��֧���������֣�1. streaming����ʽ��Ӧ��ʹ��SSEЭ�� 2. blocking��������Ӧ
        [SerializeField] public string conversation_id=string.Empty;//�Ի�ID�����Ի���Ӧ����Ч���ڶԻ���Ӧ���У�1. �գ���ʾ���½��Ự 2. �ǿգ���ʾ�ڶ�Ӧ�ĻỰ�м������жԻ��������ڲ�ά���Ի���ʷ
    }

    /// <summary>
    /// ���ص�����
    /// </summary>
    [Serializable]
    public class ResponseData
    {
        [SerializeField] public int code;//�����롣��0Ϊ������ο�������˵��
        [SerializeField] public string message=string.Empty;//������Ϣ
        [SerializeField] public Result result = new Result();//�ظ���Ϣ
    }

    [Serializable]
    public class Result
    {
        [SerializeField] public string answer = string.Empty;//�ظ�����
        [SerializeField] public string conversation_id = string.Empty;//�Ի�ID
    }

    #endregion


}
