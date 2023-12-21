// See https://aka.ms/new-console-template for more information
using Q1Lab4;

Console.WriteLine("Question 01 - Lab 04");

// Invokes methods
Question1_1();
Question1_2();
Question1_3();
Question1_4();
Question1_5();
Question1_6();


// 1.1 List the names of the countries in alphabetical order [0.5 mark]
void Question1_1()
{
    List<Country> countries = Country.GetCountries();
    IEnumerable<string> countryNames = countries.Select(country => country.Name).OrderBy(name => name).ToList();
    Console.WriteLine("Countries in alphabetical order: ");
    
    foreach(string countryName in countryNames)
    {
        Console.WriteLine(countryName);
    }

}

// 1.2 List the names of the countries in descending order of number of resources [0.5 mark]
void Question1_2()
{
    List<Country> countries = Country.GetCountries();
    IEnumerable<string> countryNames = countries.OrderByDescending(country => country.Resources.Count).Select(country => country.Name).ToList();

    Console.WriteLine("Countries in descending order of number of resources: ");

    foreach (string countryName in countryNames)
    {
        Console.WriteLine(countryName);
    }

}

// 1.3 List the names of the countries that shares a border with Argentina [0.5 mark]
void Question1_3()
{
    List<Country> countries = Country.GetCountries();
    IEnumerable<string> NeighborWithArgentina = countries.FirstOrDefault(country => country.Name == "Argentina")?.Borders;

    Console.WriteLine("Countries that shares a border with Argentina: ");

    if (NeighborWithArgentina != null)
    {
        foreach(Country country in countries.Where(country => NeighborWithArgentina.Contains(country.Name)))
        {
            Console.WriteLine(country.Name);
        }
    }

}

// 1.4 List the names of the countries that has more than 10,000,000 population [0.5 mark]
void Question1_4()
{
    List<Country> countries = Country.GetCountries();
    //IEnumerable<string> countryNames = countries.Where(population => Population > 10000000).
    IEnumerable<string> countriesWithMsPopulation = countries.Where(country => country.Population > 10000000).Select(country => country.Name).ToList();
    Console.WriteLine("Countries that has more than 10 000 000 population: ");

    foreach (string countryName in countriesWithMsPopulation)
    {
        Console.WriteLine(countryName);
    }

}

// 1.5 List the country with highest population [1 mark]
void Question1_5()
{
    List<Country> countries = Country.GetCountries();

    Country countryWithHighestPopulation = countries.OrderByDescending(country => country.Population).FirstOrDefault();
    


    if (countryWithHighestPopulation != null)
    {
        Console.WriteLine($"Country with highest population is: {countryWithHighestPopulation.Name} with {countryWithHighestPopulation.Population} population.");
    }

}

// 1.6 List all the religion in south America in dictionary order [1 mark]
void Question1_6()
{
    List<Country> countries = Country.GetCountries();
    IEnumerable<string> southAmericaReligion = countries.SelectMany(country => country.Religions).Distinct().OrderBy(religion => religion).ToList();
    
    Console.WriteLine("Religions in south America in dictionary order: ");

    foreach (string religion in southAmericaReligion)
    {
        Console.WriteLine(religion);
    }

}
