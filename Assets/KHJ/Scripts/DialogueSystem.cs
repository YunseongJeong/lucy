using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; private set; }

        
        public GameObject dialogueImage;
        public TextMeshProUGUI dialogueText;
        public float typingSpeed = 0.05f;
        public float delayBetweenDialogues = 1.2f;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            dialogueImage.SetActive(false);
        }

        public void ShowDialogue(string[] dialogues, float delayBetweenDialogues = 1.5f) // 두 문장 이상 출력
        {
            StartCoroutine(DisplayDialogues(dialogues, delayBetweenDialogues));
        }

        public void ShowDialogue(string dialogue) // 한 문장 출력
        {
            string[] singleDialogue = new string[] { dialogue };
            ShowDialogue(singleDialogue);
        }

        private IEnumerator DisplayDialogues(string[] dialogues, float delayBetweenDialogues)
        {
            dialogueImage.SetActive(true);
            Time.timeScale = 0f;

            foreach (string dialogue in dialogues)
            {
                yield return StartCoroutine(TypeDialogue(dialogue));
                yield return new WaitForSecondsRealtime(delayBetweenDialogues);
            }

            dialogueImage.SetActive(false);
            Time.timeScale = 1f;
        }

        private IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";

            foreach (char letter in dialogue.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }
    }
}

