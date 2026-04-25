using UnityEngine; 
using TMPro;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    public TMP_Text instruction;
    void Start()
    {
        ShowAgenda();
    }

    public void ShowAgenda()
    {
        instruction.text += "There is some nasty bugs in my games. But seems like the players actually found some combination to be fun...Here is what I found:\n";
        instruction.text += "- that water surface glitch actually looks good\n";
        instruction.text += "- the random hit box of rocks is annoying\n";
        instruction.text += "- the glitchy grass sometimes scares people\n";
        instruction.text += "- If I fix the bug where you can duplicate fruits by tucking them into glitchy rocks, people get angry. They really are having fun with that.\n";
        instruction.text += "- The stacking of all environments actually looks pretty good aesthetically\n";
        instruction.text += "- Players likes to get pictures on the hidden tree beneath the pond surface\n";
    }
}
