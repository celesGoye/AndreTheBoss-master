using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : Building
{
	public int cd;
	
    public override string GetDescription()
	{
		string description="Revive the last human who died in battle, after reviving the altar records nothing.";
		return description;
	}
}
