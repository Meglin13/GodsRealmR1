using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class EqupmentModifier : Modifier
{
    //TODO: Решить что делать с баффами экипировки
    public EqupmentModifier(StatType StatStype, float Amount) : base(StatStype, Amount)
    {
        this.DurationInSecs = 0;
        this.IsPermanent = true;
        ModifierType = ModType.Buff;
    }
}