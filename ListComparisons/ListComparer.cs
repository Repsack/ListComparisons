internal class ListComparer
{
  string[] txtLines;
  int ampersAndsFound;
  int firstYear = 2010;
  int yearAdditional;
  //List<string> entriesLastYear, entriesThisYear,quitters;
  List<List<string>> entries;
  List<List<string>> quitters;
  string basePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.ToString()+"\\";
  string thelistspath = "thelists.txt";

  internal void load()
  {
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine($"Trying to find a file named ({thelistspath}),");
    Console.WriteLine($" and the code is searching in the following folder:");
    Console.WriteLine($"{basePath}");
    Console.WriteLine();
    txtLines = File.ReadAllLines(basePath+thelistspath);
    ampersAndsFound = 0; //We assume there are at least 1 list, meaning NO ampers ands
    foreach (string line in txtLines)
    {
      if(line == "&") //Each "&" signifies that there is another list.
        ampersAndsFound++;
    }
    Console.WriteLine($"Found ({ampersAndsFound}) Ampers-And signs.");
    Console.WriteLine($"Program will assume ({ampersAndsFound+1}) list(s).");
    Console.WriteLine();
  }

  internal void compare()
  {
    //Creating the lists
    entries = new List<List<string>>();
    quitters = new List<List<string>>();
    for (int i = 0; i < ampersAndsFound; i++) //For every "&" we saw
    {
      entries.Add(new List<string>()); //We need to ready a new list of entries
      quitters.Add(new List<string>()); //and a new list of quitters
    }
    entries.Add(new List<string>()); //even seeing 1 "&" means there is at least 2 lists.

    //Populating the entries lists
    int andsDetected = 0;
    foreach (string line in txtLines)
    {
      if (line == "&")
      {
        andsDetected++;
        continue;
      }
      entries[andsDetected].Add(line);
    }

    Console.WriteLine("Found these list of members for different years:");
    foreach (List<string> yearEntries in entries)
    {
      Console.WriteLine($"Year ({firstYear+yearAdditional}): ({yearEntries.Count}) members.");
      yearAdditional++;
    }
    Console.WriteLine();

    //Detecting Quitters
    yearAdditional = 0;
    for (int i = 0; i < entries.Count - 1; i++) //If there are 5 lists, we must do 4 comparisons
    {
      quitters[i] = 
        entries[i].Distinct() //All the unique members of the current year
        .Except( //Remove every member that follows:
          entries[i].Intersect(entries[i+1])) //All members that are in both the current and next year
        .ToList(); //Now we have a collection of members that are BOTH:
                   //present in current year, and also NOT present in next year.
    }

    Console.WriteLine("Found these list of quitters for different years:");
    foreach (List<string> yearQuitters in quitters)
    {
      Console.WriteLine($"Year ({firstYear + yearAdditional}): ({yearQuitters.Count}) quitters.");
      yearAdditional++;
    }
    Console.WriteLine();
  }

  internal void display()
  {
    Console.WriteLine($"Assuming that the first year is ({firstYear})");
    Console.WriteLine();
    yearAdditional = 0;
    foreach (List<string> yearEntries in entries)
    {
      Console.WriteLine($"Members for the year ({firstYear+yearAdditional})");
      foreach (string member in yearEntries)
      {
        Console.WriteLine($"({member})");
      }
      yearAdditional++;
      Console.WriteLine();
    }
    Console.WriteLine("-------------------------");
    Console.WriteLine();
    yearAdditional = 0;
    Console.WriteLine("Here are everyone that quit:");
    Console.WriteLine();
    foreach (List<string> yearQuitters in quitters)
    {
      Console.WriteLine($"Quitters for the year ({firstYear + yearAdditional}):");
      if (yearQuitters.Count < 1)
      {
        Console.WriteLine("No quitters this year.");
        Console.WriteLine();
        continue;
      }
      foreach (string quitter in yearQuitters)
      {
        Console.WriteLine($"({quitter})");
      }
      Console.WriteLine();
      yearAdditional++;
    }
  }
}