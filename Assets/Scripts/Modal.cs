using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public struct ModalMessage
{
    public string text;
    public string yesText;
    public string noText;
    public UnityAction yesEvent;
    public UnityAction noEvent;
    public int type;
    public ModalMessage(string text, string yesText, string noText, UnityAction yesEvent, UnityAction noEvent)
    {
        this.type = 0;
        this.text = text;
        this.noText = noText;
        this.yesText = yesText;
        this.noEvent = noEvent;
        this.yesEvent = yesEvent;
    }
    public ModalMessage(string text, string okText, UnityAction okEvent)
    {
        this.type = 1;
        this.text = text;
        this.noText = okText;
        this.yesText = "";
        this.noEvent = okEvent;
        this.yesEvent = null;
    }
}

public class Modal
{
    private static Modal instance;
    private static GameObject modal;
    private GameObject noButton;
    private GameObject yesButton;
    private TextMeshProUGUI yesButtonText;
    private TextMeshProUGUI noButtonText;
    private TextMeshProUGUI text;
    private Queue<ModalMessage> messages;
    private ModalMessage? displayMessage;
    private bool isShowing;

    public static Modal GetInstance()
    {
        if (instance == null)
        {
            instance = new Modal();
        }
        return instance;
    }

    public static void Destroy()
    {
        instance = null;
    }

    public Modal()
    {
        messages = new Queue<ModalMessage>();
        modal = GameObject.Find("ModalController/Modal");
        noButton = GameObject.Find("ModalController/Modal/btnNo");
        yesButton = GameObject.Find("ModalController/Modal/btnYes");
        yesButtonText = GameObject.Find("ModalController/Modal/btnYes/Text").GetComponent<TextMeshProUGUI>();
        noButtonText = GameObject.Find("ModalController/Modal/btnNo/Text").GetComponent<TextMeshProUGUI>();
        text = GameObject.Find("ModalController/Modal/Text").GetComponent<TextMeshProUGUI>();
        modal.SetActive(false);
    }

    public void Update()
    {
        if (!isShowing && messages.Count > 0)
        {
            if (messages.Peek().type == 0)
            {
                ShowModal(messages.Peek().text, messages.Peek().yesText, messages.Peek().noText, messages.Peek().yesEvent, messages.Peek().noEvent);
            }
            else
            {
                ShowModal(messages.Peek().text, messages.Peek().noText, messages.Peek().noEvent);
            }
            messages.Dequeue();
        }
    }

    public void ShowModal(string modalText, string yesText, string noText, UnityAction yesEvent, UnityAction noEvent, bool prior = false)
    {
        ModalMessage message = new ModalMessage(modalText, yesText, noText, yesEvent, noEvent);
        if (isShowing && !displayMessage.Equals(message))
        {
            if (!messages.Contains(message) && !prior) messages.Enqueue(message);
            if (!messages.Contains(message) && prior)
            {
                AddToFrontOfQueue(message);
                isShowing = false;
            }
            return;
        }
        isShowing = true;
        displayMessage = message;
        text.SetText(modalText);
        noButtonText.SetText(noText);
        yesButtonText.SetText(yesText);

        yesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        yesButton.GetComponent<Button>().onClick.AddListener(CloseModal);
        yesButton.GetComponent<Button>().onClick.AddListener(yesEvent);

        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(CloseModal);
        noButton.GetComponent<Button>().onClick.AddListener(noEvent);


        yesButton.SetActive(true);
        noButton.SetActive(true);
        modal.SetActive(true);
    }

    public void ShowModal(string modalText, string okText, UnityAction okEvent, bool prior = false)
    {
        ModalMessage message = new ModalMessage(modalText, okText, okEvent);
        if (isShowing && !displayMessage.Equals(message))
        {
            if (!messages.Contains(message) && !prior) messages.Enqueue(message);
            if (!messages.Contains(message) && prior)
            {
                AddToFrontOfQueue(message);
                isShowing = false;
            }
            return;
        }
        isShowing = true;
        displayMessage = message;
        text.SetText(modalText);
        noButtonText.SetText(okText);

        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(CloseModal);
        noButton.GetComponent<Button>().onClick.AddListener(okEvent);

        yesButton.SetActive(false);
        noButton.SetActive(true);
        modal.SetActive(true);
    }

    private void CloseModal()
    {
        modal.SetActive(false);
        displayMessage = null;
        isShowing = false;
    }

    private void AddToFrontOfQueue(ModalMessage mess)
    {
        ModalMessage[] clone = new ModalMessage[messages.Count];
        messages.CopyTo(clone, 0);
        messages.Clear();
        messages.Enqueue(mess);
        if (displayMessage != null)
        {
            messages.Enqueue((ModalMessage)displayMessage);
        }
        for (int i = 0; i < clone.Length; i++)
        {
            messages.Enqueue(clone[i]);
        }
    }
}
