using System.Collections;
using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Environment;

public class BE2_Ins_TurnDirection : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    public new void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        var controller = TargetObject.Transform.GetComponent<BE2_TargetObjectSpacecraft3D>();

        if (controller != null)
        {
            if (_value == "Sol" || _value.ToLower() == "left")
            {
                controller.StartCoroutine(WaitForTurn(controller.TurnLeftSmooth(), controller));
            }
            else if (_value == "Sağ" || _value.ToLower() == "right")
            {
                controller.StartCoroutine(WaitForTurn(controller.TurnRightSmooth(), controller));
            }
            else
            {
                ExecuteNextInstruction();
            }
        }
        else
        {
            ExecuteNextInstruction();
        }
    }

    private IEnumerator WaitForTurn(IEnumerator turnCoroutine, BE2_TargetObjectSpacecraft3D controller)
    {
        yield return controller.StartCoroutine(turnCoroutine);

        ExecuteNextInstruction();
    }
}
