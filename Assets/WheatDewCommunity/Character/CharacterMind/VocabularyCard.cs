using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocabularyCard : MonoBehaviour
{
    private Button self;
    private bool isPressed;
    public string word;
    public Card card;
    public CreateSendWordCommandButton createSendWordCommandButton;

    private void Start()
    {
        self = GetComponent<UnityEngine.UI.Button>();
        isPressed = false;
        if (card == Card.blue)
            createSendWordCommandButton.blueCards.Add(this);
        else if (card == Card.green)
            createSendWordCommandButton.greenCards.Add(this);
        else if (card == Card.yellow)
            createSendWordCommandButton.yellowCards.Add(this);

        self.onClick.AddListener(delegate
        {
            if (card == Card.blue)
            {
                foreach (var item in createSendWordCommandButton.blueCards)
                    item.ResetColor();
                createSendWordCommandButton.b_card = word;
            }
            if (card == Card.green)
            {
                foreach (var item in createSendWordCommandButton.greenCards)
                    item.ResetColor();
                createSendWordCommandButton.g_card = word;
            }
            if (card == Card.yellow)
            {
                foreach (var item in createSendWordCommandButton.yellowCards)
                    item.ResetColor();
                createSendWordCommandButton.y_card = word;
            }
            else if (card == Card.green)
                createSendWordCommandButton.g_card = word;
            else if (card == Card.yellow)
                createSendWordCommandButton.y_card = word;
            ColorBlock temp = new ColorBlock();
            temp = self.colors;
            if (isPressed)
                temp.normalColor = Color.white;
            else
                temp.normalColor = self.colors.pressedColor;
            self.colors = temp;
        });
    }

    public void ResetColor()
    {
        ColorBlock temp = new ColorBlock();
        temp = self.colors;
        temp.normalColor = Color.white;
        self.colors = temp;
    }
}
