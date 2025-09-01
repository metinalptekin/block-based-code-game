using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;

public class BE2_Ins_ChangeColor : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    public new void Function()
    {
        // Input al
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        Debug.Log("BE2_Ins_ChangeColor: Seçilen renk -> " + _value);

        Color newColor = Color.white;

        switch (_value)
{
    case "Random":
    case "Rastgele":
        newColor = new Color(Random.Range(0f, 1f),
                             Random.Range(0f, 1f),
                             Random.Range(0f, 1f),
                             1f);
        break;
    case "Red":
    case "Kırmızı":
        ColorUtility.TryParseHtmlString("#FF0000", out newColor);
        break;
    case "Orange":
    case "Turuncu":
        ColorUtility.TryParseHtmlString("#FF7F00", out newColor);
        break;
    case "Yellow":
    case "Sarı":
        ColorUtility.TryParseHtmlString("#FFFF00", out newColor);
        break;
    case "Green":
    case "Yeşil":
        ColorUtility.TryParseHtmlString("#00FF00", out newColor);
        break;
    case "Blue":
    case "Mavi":
        ColorUtility.TryParseHtmlString("#0000FF", out newColor);
        break;
    case "Indigo":
    case "Çivit":
        ColorUtility.TryParseHtmlString("#2E2B5F", out newColor);
        break;
    case "Violet":
    case "Mor":
        ColorUtility.TryParseHtmlString("#8B00FF", out newColor);
        break;
    default:
        Debug.LogWarning("BE2_Ins_ChangeColor: Tanımlanmamış renk -> " + _value);
        break;
}


        // Target objeyi logla
        if (TargetObject == null || TargetObject.Transform == null)
        {
            Debug.LogWarning("BE2_Ins_ChangeColor: TargetObject veya Transform null!");
        }
        else
        {
            Debug.Log("BE2_Ins_ChangeColor: TargetObject bulundu -> " + TargetObject.Transform.name);
        }

        // SpriteRenderer bul
        var spriteRenderer = TargetObject.Transform.Find("MySpriteVisual")?.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = TargetObject.Transform.GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            Debug.Log("BE2_Ins_ChangeColor: SpriteRenderer bulundu, renk uygulanıyor");
            spriteRenderer.color = newColor;
        }
        else
        {
            Debug.LogWarning("BE2_Ins_ChangeColor: MySpriteVisual veya SpriteRenderer bulunamadı!");
        }

        ExecuteNextInstruction();
    }
}
