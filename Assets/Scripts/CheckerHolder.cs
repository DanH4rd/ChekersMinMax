using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;

public class CheckerHolder : MonoBehaviour
{
    public int order;
    public Checker checker { get; set; }

    public (int,int) checkerId {get { return checker == null ? (-1, -1) : checker.currentPosition;  }}

}
