using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEmojiPanel : MonoBehaviour
{
   [SerializeField] private List<Button> listEmojiButtons;
   [SerializeField] private ChatUI m_ChatUI;

   private void Start()
   {
      foreach (var emojiButton in listEmojiButtons)
      {
         emojiButton.onClick.AddListener((() => m_ChatUI.AddEmojiToInputField(listEmojiButtons.IndexOf(emojiButton))));
      }
   }
}
