
using System;
using System.Linq;
using System.Collections.Generic;

namespace DnD_Dice
{
  public class Program {
    public static int Main(string[] args) {
      Die? die = null;

      int sideCount   = 0;
      int roleAmount  = 0;
      int step        = 0;
      int start       = 0;

      bool couldParseSideCount  = false;
      bool couldParseRoleAmount = false;
      bool couldParseStep       = false;
      bool couldParseStart      = false;

      int[] roles = new int[0];

      switch (args.Length) {
        // With side count
        case 1:
          couldParseSideCount = int.TryParse(args[0], out sideCount);

          if (!couldParseSideCount)
            printHelp();

          die = new Die(sideCount);
          Die.PrintRole(die.Role());
        break;

        // With side count and role amount
        case 2:
          couldParseSideCount = int.TryParse(args[0], out sideCount);
          couldParseRoleAmount = int.TryParse(args[1], out roleAmount);

          if (!couldParseSideCount || !couldParseRoleAmount)
            printHelp();

          die = new Die(sideCount);
          roles = die.Role(roleAmount);
          Die.PrintRoles(roles, die);
        break;

        // With side count, role amount, and step
        case 3:
          couldParseSideCount = int.TryParse(args[0], out sideCount);
          couldParseRoleAmount = int.TryParse(args[1], out roleAmount);
          couldParseStep = int.TryParse(args[2], out step);

          if (!couldParseSideCount || !couldParseRoleAmount || !couldParseStep)
            printHelp();

          die = new Die(sideCount, step);
          roles = die.Role(roleAmount);
          Die.PrintRoles(roles, die);
        break;

        // With side count, role amount, step, and start
        case 4:
          couldParseSideCount = int.TryParse(args[0], out sideCount);
          couldParseRoleAmount = int.TryParse(args[1], out roleAmount);
          couldParseStep = int.TryParse(args[2], out step);
          couldParseStart = int.TryParse(args[3], out start);

          if (
            !couldParseSideCount || !couldParseRoleAmount
            ||   !couldParseStep || !couldParseStart)
            printHelp();

          die = new Die(sideCount, step, start);
          roles = die.Role(roleAmount);
          Die.PrintRoles(roles, die);
        break;
        default:
          printHelp();
        break;
      }

      System.Console.WriteLine(die);

      return 0;
    }

    public static void printHelp() {
      string sep = string.Join("", Enumerable.Range(0, 60).Select(_ => "-").ToArray());

      System.Console.WriteLine("Legend: [requred], {optional}");
      System.Console.WriteLine("Usage: throw [sides] {roles} {step} {start}");
      System.Console.WriteLine(sep);
      System.Console.WriteLine("\tsides:\tcontrols the amount of sides that the die has");
      System.Console.WriteLine("\t\t\t\t\tIf step and start are empty, the will both be 1\r\n");
      System.Console.WriteLine("\troles:\tcontrols the amount of roles the die will do, if left empty it will be 1");
      System.Console.WriteLine();
      System.Console.WriteLine("\tstep:\t\tcontrols the step between each face value.");
      System.Console.WriteLine("\t\t\t\t\tIf start is empty, then the first number will be 1");
      System.Console.WriteLine();
      System.Console.WriteLine("\tstart:\tcontrols the lowest number on the die");
      System.Console.WriteLine(sep);
      System.Console.WriteLine("Sides are calculated as so: side = start * step");
      Environment.Exit(1);
    }
  }
}


public class Die {
  public int SideCount;
  public int[] Values;
  public int Step, Start;

  public Die(int sideCount, int step=1, int start=1) {
    SideCount = sideCount;
    Step = step;
    Start = start;
    Values = Enumerable.Range(start, sideCount).Select(item => item * step).ToArray();
  }

  public int Role() {
    Random rng = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

    int index = rng.Next() % Values.Length;
    return Values[index];
  }

  public int[] Role(int count) {
    int[] r = Enumerable
      .Range(0, count)
      .Select(item => Role())
      .ToArray();
    return r;
  }

  public static void PrintRole(int role) {
    System.Console.WriteLine($"Role: {role}");
    return;
  }

  public static void PrintRoles(IEnumerable<int> roles, Die? die=null) {
    if (die!=null) {
      System.Console.WriteLine($"Sides: {die.SideCount}");
      System.Console.WriteLine($"Step:  {die.Step}");
    }

    int total = 0;

    for (int i = 0; i < roles.Count(_ => true); i++) {
      int elem = roles.ElementAt(i);
      System.Console.Write($"{i+1}: ");
      Die.PrintRole(elem);
      total += elem;
    }

    System.Console.WriteLine($"Total: {total}");
    return;
  }

  public override string ToString()
  {
    return $"Die(SideCount={SideCount}, Values={{{string.Join(", ", Values)}}})";
  }
}
