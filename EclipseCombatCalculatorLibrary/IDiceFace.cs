﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculatorLibrary
{
    public interface IDiceFace
    {
        int DamageToOpponent { get; }
        int DamageToSelf { get; }
    }
}
