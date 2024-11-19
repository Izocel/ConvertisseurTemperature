
using System.Text;

internal class Program
{
    private static string SourceSymbol = "";
    private static double SourceTemperature = 0;
    private static bool IsTemperatureAssigned = false;
    private static string ConvertedSymbol = "";
    private static double ConvertedTemperature = 0;

    private static void Main(string[] args)
    {
        ParseArguments(args);
        ConvertArguments();
        OutputConversion();

        System.Console.WriteLine("\nPress Any Key To Exit:");
        Console.ReadLine();
    }

    /// <summary>
    /// Parses command-line arguments to extract temperature and unit information.
    /// </summary>
    /// <param name="args">An array of command-line arguments.</param>
    /// <remarks>
    /// If the temperature or unit is not valid, prompts for user input to correct them.
    /// </remarks>
    private static void ParseArguments(string[] args)
    {
        for (int i = 0; i < args.Length; i += 2)
        {
            string name = args[i];
            string value = i + 1 < args.Length ? args[i + 1] : "";
            ParseArgumentKeyValue([name, value]);
        }

        if (!IsTemperatureAssigned)
            ParseArgumentKeyValue(["-t", ""]);

        if (!IsValidUnitSymbol(SourceSymbol))
            ParseArgumentKeyValue(["-u", ""]);
    }

    /// <summary>
    /// Parses a key-value pair from command-line arguments and delegates processing
    /// based on the key.
    /// </summary>
    /// <param name="keyValue">An array containing the key and value as strings.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an unrecognized key is encountered.</exception>
    private static void ParseArgumentKeyValue(string[] keyValue)
    {
        string? name = keyValue[0]?.Replace("-", "").ToLower();
        string? value = keyValue[1];

        switch (name)
        {
            case "t":
            case "temperature":
                HandleTemperatureInput(value);
                break;

            case "u":
            case "unit":
                HandleUnitInput(value);
                break;

            case "h":
            case "help":
                PrintHelp();
                return;

            default:
                ArgumentOutOfRangeException e = new(name);
                PrintHelp($"\n{e.Message}\n");
                return;
        }
    }

    /// <summary>
    /// Handles the input for the base temperature value, prompting the user until a valid numeric value is entered.
    /// </summary>
    /// <param name="value">An optional initial temperature value to validate, defaulting to null.</param>
    private static void HandleTemperatureInput(string? value = null)
    {
        IsTemperatureAssigned = double.TryParse(value, out SourceTemperature);

        while (!IsTemperatureAssigned)
        {
            Console.Write("Base temperature value: ");
            IsTemperatureAssigned = double.TryParse(Console.ReadLine(), out SourceTemperature);
        }
    }

    /// <summary>
    /// Handles the input for the temperature unit, prompting the user until a valid unit symbol ('C' or 'F') is entered.
    /// </summary>
    /// <param name="value">An optional initial unit symbol to validate, defaulting to "nil".</param>
    private static void HandleUnitInput(string? value = "nil")
    {
        while (!IsValidUnitSymbol(value))
        {
            Console.Write("Base temperature unit (C or F): ");
            value = Console.ReadLine().ToUpper();
        }

        SourceSymbol = value.ToUpper();
        ConvertedSymbol = value == "F" ? "C" : "F";
    }

    /// <summary>
    /// Determines if the provided temperature unit symbol is valid.
    /// </summary>
    /// <param name="symbol">The unit symbol to validate, which can be 'C' for Celsius or 'F' for Fahrenheit.</param>
    /// <returns>True if the symbol is valid; otherwise, false.</returns>
    private static bool IsValidUnitSymbol(string? symbol)
    {
        switch (symbol?.ToUpper())
        {
            case "C":
            case "F":
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Converts the source temperature from Celsius to Fahrenheit or vice versa
    /// based on the source unit symbol.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the source unit symbol is not recognized as 'C' or 'F'.
    /// </exception>
    private static void ConvertArguments()
    {
        switch (SourceSymbol.ToUpper())
        {
            case "C":
                ConvertedTemperature = (SourceTemperature - 32) / 1.8;
                break;
            case "F":
                ConvertedTemperature = SourceTemperature * 1.8 + 32;
                break;
            default:
                throw new ArgumentOutOfRangeException(SourceSymbol);
        }
    }

    /// <summary>
    /// Outputs the temperature conversion result to the console.
    /// </summary>
    /// <remarks>
    /// Formats the source and converted temperatures with their respective unit symbols
    /// and displays them in a readable format.
    /// </remarks>
    private static void OutputConversion()
    {
        StringBuilder SB = new($"{SourceTemperature}°{SourceSymbol}");
        SB.Append(" => ");
        SB.Append($"{ConvertedTemperature}°{ConvertedSymbol}");
        Console.WriteLine(SB.ToString());
    }

    /// <summary>
    /// Displays the help information for the ConsoleApp program.
    /// </summary>
    /// <remarks>
    /// The help includes the program name, synopsis, description, and available options.
    /// </remarks>
    private static void PrintHelp(string? prepend = null)
    {
        StringBuilder SB = new(prepend);

        SB.Append("\nNOM");
        SB.Append("\nConvertisseurTempérature");

        SB.Append("\n\nSYNOPSIS");
        SB.Append("\nConsoleApp [OPTIONS...]");

        SB.Append("\n\nDESCRIPTION");
        SB.Append("\nConvertit une température en °C si entrée en °F et vice-versa.");

        SB.Append("\n\nOPTIONS");
        SB.Append("\n-h, --help         Affiche le présent manuel d'utilisation et quitte le programme.");
        SB.Append("\n-t, --temperature  Valeur de température à convertir.");
        SB.Append("\n-u, --unit        Unité de température entrée («C» pour Celcisus ou «F» pour Fahrenheit).");

        Console.WriteLine(SB.ToString() + "\n");
    }
}