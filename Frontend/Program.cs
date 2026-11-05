
using Backend;

const int Width = 48;  

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "LISTA DOBLE// LIGADA ";

if (SelectType() == "1")
{
    RunMenu(new DoublyLinkedList<int>(), "NUMEROS ENTEROS", text => int.Parse(text));
}
else
{
    RunMenu(new DoublyLinkedList<string>(), "TEXTO", ParseText);
}

static void RunMenu<T>(DoublyLinkedList<T> list, string typeName, Func<string, T> parse) where T : IComparable<T>
{
    string option;
    do
    {
        ShowHeader(typeName, list.IsEmpty);
        option = ShowMenu();
        Console.WriteLine();

        switch (option)
        {
            case "1": 
                {
                    Print("  ► INGRESE EL VALOR ADICIONAL: ", ConsoleColor.Yellow);
                    if (TryRead(parse, out var value))
                    {
                        list.Add(value);
                        PrintLine($"  [OK] '{value}' FUE ADICIONADO.", ConsoleColor.Blue);
                    }
                    break;
                }
            case "2":
                if (HasItems(list))
                {
                    PrintSection("Lista hacia adelante");
                    PrintList(list.ToString());
                }
                break;
            case "3": 
                if (HasItems(list))
                {
                    PrintSection("Lista hacia atrás");
                    PrintList(list.ToStringReverse());
                }
                break;
            case "4":
                if (HasItems(list))
                {
                    list.SortDescending();
                    PrintLine("  [OK] LA LISTA QUEDO ORDENADA DESCENDENTEMENTE.", ConsoleColor.DarkRed);
                    Console.WriteLine();
                    PrintList(list.ToString());
                }
                break;
            case "5":
                {
                    if (!HasItems(list)) break;

                    PrintSection("Moda(s)");
                    var modes = list.GetModes();
                    if (modes.Count == 1)
                    {
                        PrintLine($"  LA MODA ES {modes[0]}.", ConsoleColor.DarkRed);
                    }
                    else
                    {
                        PrintLine($"  LAS MODAS SON {string.Join(", ", modes.Take(modes.Count - 1))} y {modes[^1]}.", ConsoleColor.DarkRed);
                    }
                    break;
                }
            case "6": 
                if (HasItems(list))
                {
                    PrintSection("Gráfico de ocurrencias");
                    PrintChart(list.GetChart());
                }
                break;
            case "7":
                {
                    Print("  ► Ingrese el valor a buscar: ", ConsoleColor.Yellow);
                    if (TryRead(parse, out var value))
                    {
                        if (list.Contains(value))
                        {
                            PrintLine($"  [OK] SI EXISTE EL {value}.", ConsoleColor.Green);
                        }           
                        else
                        {
                            PrintLine($"  [INFO] NO EXISTE EL {value}.", ConsoleColor.DarkYellow);
                        }
                    }
                    break;
                }
            case "8":
                {
                    Print("  ► Ingrese el valor a eliminar: ", ConsoleColor.Yellow);
                    if (TryRead(parse, out var value))
                    {
                        if (list.Remove(value))
                        {
                            PrintLine($"  [OK] Se eliminó una ocurrencia de {value}.", ConsoleColor.DarkGreen);
                        }
                        else
                        {
                            PrintLine($"  [INFO] No existe el {value} en la lista.", ConsoleColor.DarkYellow);
                        }
                    }
                    break;
                }
            case "9":
                {
                    Print("  ► Ingrese el valor a eliminar: ", ConsoleColor.DarkYellow);
                    if (TryRead(parse, out var value))
                    {
                        var removed = list.RemoveAll(value);
                        if (removed > 0)
                        {
                            PrintLine($"  [OK] Se eliminaron {removed} ocurrencia de {value}.", ConsoleColor.DarkBlue);
                        }
                        else
                        {
                            PrintLine($"  [INFO] No existe el {value} en la lista.", ConsoleColor.DarkYellow);
                        }
                    }
                    break;
                }
            case "0":
                PrintLine("  ¡Hasta luego!", ConsoleColor.Cyan);
                break;
            default:
                PrintLine("  [ERROR] Opción inválida. Elija un número del 0 al 9.", ConsoleColor.Red);
                break;
        }

        if (option != "0")
        {
            Pause();
        }
    } while (option != "0");
}

static string SelectType()
{
    string? choice;
    do
    {
        ShowBanner();
        PrintLine("  ¿Qué tipo de dato guardará la lista?", ConsoleColor.White);
        Console.WriteLine();
        Print("    1", ConsoleColor.Yellow); PrintLine(". Números enteros", ConsoleColor.White);
        Print("    2", ConsoleColor.Yellow); PrintLine(". Texto", ConsoleColor.White);
        Console.WriteLine();
        Print("  ► Elija una opción: ", ConsoleColor.Yellow);
        choice = Console.ReadLine()?.Trim();

        if (choice != "1" && choice != "2")
        {
            PrintLine("  [ERROR] Opción inválida.", ConsoleColor.Red);
            Pause();
        }
    } while (choice != "1" && choice != "2");

    return choice!;
}

static void ShowBanner()
{
    Console.Clear();
    Console.WriteLine();
    PrintLine("  ╔" + new string('═', Width) + "╗", ConsoleColor.Cyan);
    PrintLine("  ║" + Center("LISTA DOBLE// LIGADA", Width) + "║", ConsoleColor.Cyan);
    PrintLine("  ║" + Center("BIENVENIDO ZULU Y COMPAÑEROS", Width) + "║", ConsoleColor.DarkGreen);
    PrintLine("  ╚" + new string('═', Width) + "╝", ConsoleColor.Green);
    Console.WriteLine();
}

static void ShowHeader(string typeName, bool isEmpty)
{
    ShowBanner();
    Print("  Tipo de dato: ", ConsoleColor.Gray);
    Print(typeName, ConsoleColor.Yellow);
    Print("   │   Lista: ", ConsoleColor.DarkCyan);
    PrintLine(isEmpty ? "vacía" : "con elementos", isEmpty ? ConsoleColor.DarkYellow : ConsoleColor.Green);
    Console.WriteLine();
}

static string ShowMenu()
{
    PrintLine("  ┌" + new string('─', Width) + "┐", ConsoleColor.DarkCyan);
    Print("  │", ConsoleColor.DarkCyan);
    Print(Center("MENÚ", Width), ConsoleColor.White);
    PrintLine("│", ConsoleColor.DarkCyan);
    PrintLine("  ├" + new string('─', Width) + "┤", ConsoleColor.DarkCyan);

    MenuRow("1", "Adicionar.");
    MenuRow("2", "Mostrar hacia adelante.");
    MenuRow("3", "Mostrar hacia atrás.");
    MenuRow("4", "Ordenar descendentemente.");
    MenuRow("5", "Mostrar la(s) moda(s).");
    MenuRow("6", "Mostrar gráfico.");
    MenuRow("7", "Existe.");
    MenuRow("8", "Eliminar una ocurrencia.");
    MenuRow("9", "Eliminar todas las ocurrencias.");
    PrintLine("  ├" + new string('─', Width) + "┤", ConsoleColor.DarkCyan);
    MenuRow("0", "Salir.", ConsoleColor.Yellow);

    PrintLine("  └" + new string('─', Width) + "┘", ConsoleColor.DarkCyan);
    Console.WriteLine();
    Print("  ► Ingrese su opción: ", ConsoleColor.Yellow);
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static void MenuRow(string number, string text, ConsoleColor numberColor = ConsoleColor.Yellow)
{
    Print("  │  ", ConsoleColor.DarkCyan);
    Print(number, numberColor);
    Print(". " + text.PadRight(Width - 5), ConsoleColor.White);
    PrintLine("│", ConsoleColor.DarkCyan);
}


static void Print(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

static void PrintLine(string text, ConsoleColor color)
{
    Print(text, color);
    Console.WriteLine();
}

static string Center(string text, int width)
{
    var left = (width - text.Length) / 2;
    return (new string(' ', left) + text).PadRight(width);
}

static void PrintSection(string title)
{
    PrintLine($"  ── {title} ──", ConsoleColor.DarkMagenta);
    Console.WriteLine();
}

static void PrintList(string text)
{
    var parts = text.Split(" <-> ");
    Print("  ", ConsoleColor.White);
    for (var i = 0; i < parts.Length; i++)
    {
        var isLast = i == parts.Length - 1;   
        Print(parts[i], isLast ? ConsoleColor.DarkGray : ConsoleColor.Blue);
        if (!isLast)
        {
            Print(" <-> ", ConsoleColor.DarkRed);
        }
    }
    Console.WriteLine();
}

static void PrintChart(string chart)
{
    var lines = chart.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    foreach (var line in lines)
    {
        var cut = line.LastIndexOf(' ');
        Print("  " + line[..cut].PadRight(10), ConsoleColor.White);
        Print("│ ", ConsoleColor.DarkGreen);
        PrintLine(line[(cut + 1)..], ConsoleColor.Green);
    }
}

static void Pause()
{
    Console.WriteLine();
    Print("  Presione una tecla para continuar...", ConsoleColor.DarkYellow);
    Console.ReadKey(true);
}

static bool HasItems<T>(DoublyLinkedList<T> list) where T : IComparable<T>
{
    if (!list.IsEmpty)
    {
        return true;
    }

    PrintLine("  [INFO] La lista está vacía.", ConsoleColor.DarkGreen);
    return false;
}

static bool TryRead<T>(Func<string, T> parse, out T value)
{
    var input = Console.ReadLine() ?? string.Empty;
    try
    {
        value = parse(input);
        return true;
    }
    catch (Exception ex) when (ex is FormatException or OverflowException)
    {
        PrintLine($"  [ERROR] El valor \"{input}\" no es válido.", ConsoleColor.Red);
        value = default!;
        return false;
    }
}

static string ParseText(string text)
{
    var trimmed = text.Trim();
    return trimmed.Length > 0 ? trimmed : throw new FormatException();
}