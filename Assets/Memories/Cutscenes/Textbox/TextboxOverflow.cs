using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Memories.Cutscenes.Textbox;

public class TextboxOverflow : MonoBehaviour
{
    public BookTextbox textbox;
    public float hiddenLocalPositionY;
    public float activeLocalPositionY;

    private bool _isShown;

    private void Update()
    {
        if (textbox.LastCharacterDropdown < 0 || textbox.tmp.maxVisibleCharacters < textbox.LastCharacterDropdown) Hide();
        else if (textbox.tmp.maxVisibleCharacters >= textbox.LastCharacterDropdown) Show();
    }

    private void Show()
    {
        if (_isShown) return;
        _isShown = true;

        transform.DOComplete();
        transform.DOLocalMoveY(activeLocalPositionY, 0.25f).SetEase(Ease.OutCubic);
    }

    private void Hide()
    {
        if (!_isShown) return;
        _isShown = false;
        
        transform.DOComplete();
        transform.DOLocalMoveY(hiddenLocalPositionY, 0.25f).SetEase(Ease.InCubic);
    }
}
