using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using System;

namespace Assets.Scripts.ComputerDecisionMaker
{
    interface ComputerPickMove
    {
        public Move pickMoveForMachine(Board board, bool isWhiteTurn);

        public (long, double) getStats();
    }
}
