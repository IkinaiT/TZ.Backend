using TZ.Backend.Console.Adapter.Interfaces;
using TZ.Backend.Console.Adapter.Runtime;

var host = "localhost";
var username = "postgres";
var password = "admin";

while (true)
{
    var ended = false;

    Console.WriteLine("Configurate connection string? [y/n]");
    switch (Console.ReadLine()?.ToLower())
    {
        case "y":
            string? temp = string.Empty;

            Console.Write("Host >>");
            temp = Console.ReadLine();

            host = string.IsNullOrEmpty(temp) ? host : temp;

            Console.Write("Username >>");
            temp = Console.ReadLine();

            username = string.IsNullOrEmpty(temp) ? host : temp;

            Console.Write("Password >>");
            temp = Console.ReadLine();

            password = string.IsNullOrEmpty(temp) ? host : temp;

            Console.WriteLine("OK\n\n");

            ended = true;

            break;

        case "n":
            ended = true;
            break;

        default:
            Console.WriteLine("Incorrect input");
            break;
    }

    if (ended)
        break;
}

IProduct _booksProduct = new BooksAdapter($"Host={host};Username={username};Password={password};Database=TZ_Backend;Pooling=True");
bool endSession = false;

await _booksProduct.FillDatabase();

while (!endSession)
{
    Console.Write("Command waiting. For help write -help >> ");
    var fullCommand = Console.ReadLine()?.ToLower();
    var command = fullCommand?.Split(' ')[0];

    switch (command)
    {
        case "-help":
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\nget - returns a full list of products, sorted by Id by default");
            Console.WriteLine("Flags:");
            Console.WriteLine("    --title=%% - consider the product in the search results only if the specified substring is found in the name");
            Console.WriteLine("    --author=%% - include the product in the search results only if the specified substring appears in the author field");
            Console.WriteLine("    --date=%% - similar, but for date, date in yyyy-MM-dd format");
            Console.WriteLine("    --order-by=[title|author|date|count] - sort the list of products by the specified field");
            Console.WriteLine("Flags can be used simultaneously, for example: `get --title \"for dummies\" --order-by=\"count\"`\n");

            Console.WriteLine("buy - reduces the quantity of the specified goods by 1");
            Console.WriteLine("Flags:");
            Console.WriteLine("    --id=%% - Id of the product to be purchased\n");

            Console.WriteLine("restock - increases random quantity of random goods by random positive number. If ID or quantity is specified by flag, then replenish according to specified flags.");
            Console.WriteLine("Flags:");
            Console.WriteLine("    --id=%% - Id of the product whose quantity needs to be replenished");
            Console.WriteLine("    --count=%% - number by which to increase the quantity of the product\n");
            Console.WriteLine("For exit enter quit or exit\n");

            Console.ResetColor();
            break;

        case "get":
            Console.WriteLine(await _booksProduct.Get(SplitGetFlags(fullCommand))); ;
            break;

        case "buy":
            Console.WriteLine(await _booksProduct.Buy(SplitBuyFlags(fullCommand))); ;
            break;

        case "restock":
            Console.WriteLine(await _booksProduct.Restock(SplitRestockFlags(fullCommand))); ;
            break;

        case "quit":
        case "exit":
            endSession = true;
            break;

        default:
            Console.WriteLine("WRONG INPUT!!!");
            break;
    }
}

static string?[] SplitGetFlags(string? command)
{
    string?[] result = [null, null, null, null];

    if(command != null)
    {
        var splited = command.Split("--");

        foreach (var line in splited)
        {
            var temp = line.Trim().Split('=');

            if (string.Equals(temp[0], "title"))
            {
                if (result[0] == null)
                    result[0] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }

            if (string.Equals(temp[0], "author"))
            {
                if (result[1] == null)
                    result[1] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }

            if (string.Equals(temp[0], "date"))
            {
                if (result[2] == null)
                    result[2] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }

            if (string.Equals(temp[0], "order-by"))
            {
                if (result[3] == null)
                    result[3] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }
        }
    }

    return result;
}

static string? SplitBuyFlags(string? command)
{
    string? result = null;

    if (command != null)
    {
        var splited = command.Split("--");

        foreach (var line in splited)
        {
            var temp = line.Trim().Split('=');

            if (string.Equals(temp[0], "id"))
            {
                result ??= temp[1].Trim().Substring(1, temp[1].Length - 2);
            }
        }
    }

    return result;
}

static string?[] SplitRestockFlags(string? command)
{
    string?[] result = [null, null];

    if (command != null)
    {
        var splited = command.Split("--");

        foreach (var line in splited)
        {
            var temp = line.Trim().Split('=');

            if (string.Equals(temp[0], "id"))
            {
                if (result[0] == null)
                    result[0] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }

            if (string.Equals(temp[0], "count"))
            {
                if (result[1] == null)
                    result[1] = temp[1].Trim().Substring(1, temp[1].Length - 2);
            }
        }
    }

    return result;
}