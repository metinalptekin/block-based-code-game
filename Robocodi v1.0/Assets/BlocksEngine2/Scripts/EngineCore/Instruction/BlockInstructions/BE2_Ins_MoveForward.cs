using System.Collections;
using TMPro;
using UnityEngine;
using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;

public class BE2_Ins_MoveForward : BE2_InstructionBase, I_BE2_Instruction
{
    public new void Function()
    {
        var target = TargetObject as BE2_TargetObjectSpacecraft3D;
        if (target != null)
        {
            int steps = 1;

            Transform section0 = transform.Find("Section0");
            if (section0 != null)
            {
                TMP_InputField inputField = section0.GetComponentInChildren<TMP_InputField>();
                if (inputField != null && !string.IsNullOrEmpty(inputField.text))
                {
                    if (int.TryParse(inputField.text, out int parsed))
                        steps = Mathf.Max(1, parsed);
                }
            }

            if (!target.isMoving)
                target.StartCoroutine(MoveSteps(steps, target));
        }
    }

    IEnumerator MoveSteps(int steps, BE2_TargetObjectSpacecraft3D target)
    {
        for (int i = 0; i < steps; i++)
        {
            while (target.isMoving)
                yield return null;

            yield return target.MoveOneStepSmooth();
        }

        while (target.isMoving)
            yield return null;

        ExecuteNextInstruction();
    }
}
