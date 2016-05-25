using System;

public class MathUtils
{
	public static double CalculateDistance(int pmX1, int pmX2, int pmZ1, int pmZ2)
	{
		return Math.Sqrt ((Math.Abs (pmX2 - pmX1) * Math.Abs (pmX2 - pmX1)) + (Math.Abs (pmZ2 - pmZ1) * Math.Abs (pmZ2 - pmZ1)));
	}
}


