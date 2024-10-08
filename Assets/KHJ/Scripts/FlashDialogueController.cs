using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FlashDialogue
{
    public class FlashDialogueController : MonoBehaviour
    {
        public static FlashDialogueController Instance { get; private set; }
        public FlashDialogueData flashDialogueData;
        public GameObject dialogueImage;
        public GameObject dialogueCharacter;
        public GameObject inventoryCanvas;
        public TextMeshProUGUI dialogueText;

        public float typingSpeed = 0.05f;
        public float delayBetweenDialogues = 1.2f;

        private int currentDialogueIndex = 0;
        private string[] currentDialogues;
        private bool equipFlash = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            dialogueImage.SetActive(false);
            dialogueCharacter.SetActive(false);
        }

        public void StartDialogueCoroutine()
        {
            StartCoroutine(StartDialogue());
        }

        private IEnumerator StartDialogue()
        {
            for (int i = 0; i < flashDialogueData.dialogues.Count; i++)
            {
                ShowDialogue(flashDialogueData.dialogues[i].texts);
                yield return StartCoroutine(WaitForCondition());
            }
        }

        // 두 문장 이상 출력시
        public void ShowDialogue(string[] dialogues, float delayBetweenDialogues = 1.5f)
        {
            currentDialogues = dialogues;
            StartCoroutine(DisplayDialogues());
        }

        // 한 문장 출력시
        public void ShowDialogue(string dialogue)
        {
            string[] singleDialogue = new string[] { dialogue };
            ShowDialogue(singleDialogue);
        }

        private IEnumerator DisplayDialogues()
        {
            dialogueImage.SetActive(true);
            dialogueCharacter.SetActive(true);
            Time.timeScale = 0f;

            for (int i = 0; i < currentDialogues.Length; i++)
            {
                yield return StartCoroutine(TypeDialogue(currentDialogues[i]));
                yield return new WaitForSecondsRealtime(delayBetweenDialogues);
            }

            
        }

        // 한 글자씩 출력하는 기능
        private IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";

            foreach (char letter in dialogue.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

        }

        // 다음 대사로 넘어가는 조건
        private IEnumerator WaitForCondition()
        {
            switch (currentDialogueIndex)
            {
                case 0:
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));
                    currentDialogueIndex++;
                    break;

                case 1:
                    yield return new WaitUntil(() => equipFlash);
                    currentDialogueIndex++;
                    break;

                case 2:
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
                    EndDialogue();
                    break;
            }
        }

        public void EquipFlash()
        {
            equipFlash = true;
        }

        private void EndDialogue()
        {
            dialogueImage.SetActive(false);
            dialogueCharacter.SetActive(false);
            //inventoryCanvas.SetActive(false);
            dialogueText.text = "";
            Time.timeScale = 1f;
        }
    }

}
