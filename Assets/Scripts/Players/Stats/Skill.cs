using System;

public class Skill
{
	private string name;
	private AbilityNames ability;
	private bool isProficient;

	public Skill (string name, AbilityNames ability, bool isProficient)
	{
		this.name = name;
		this.ability = ability;
		this.isProficient = isProficient;
	}

	public string Name {
		get{ return name; }
		set{ this.name = value;}
	}
	public AbilityNames Ability {
		get{ return ability; }
		set{ this.ability = value;}
	}
	public bool IsProficient {
		get{ return isProficient; }
		set{ this.isProficient = value;}
	}
}


