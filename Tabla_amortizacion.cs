
using Spectre.Console;


decimal montoPrestamo = AnsiConsole.Ask<decimal>(
    "Monto del prestamo: ");

decimal tasaInteres = AnsiConsole.Ask<decimal>(
    "Tasa de interes anual (%): ");

int plazoPrestamo = AnsiConsole.Ask<int>(
    "Plazo del prestamo en meses: ");


if (montoPrestamo <= 0 || tasaInteres < 0 || plazoPrestamo <= 0)
{
    AnsiConsole.MarkupLine(
        "[red]El monto y el plazo deben ser mayores que cero, y la tasa no puede ser negativa.[/]");
    return;
}


var tabla = new Table()
    .ShowRowSeparators()
    .RoundedBorder()
    .BorderColor(Color.Blue);

tabla.AddColumn("No. de cuota");
tabla.AddColumn("Pago de cuota");
tabla.AddColumn("Interes a pagar");
tabla.AddColumn("Abono a capital");
tabla.AddColumn("Saldo");


decimal tasaInteresMensual = tasaInteres / 12m / 100m;


decimal cuotaFija;

if (tasaInteresMensual == 0)
{
    cuotaFija = montoPrestamo / plazoPrestamo;
}
else
{
    
    decimal potencia = 1m + tasaInteresMensual;

    for (int j = 1; j < plazoPrestamo; j++)
    {
        potencia *= 1m + tasaInteresMensual;
    }

    
    cuotaFija = montoPrestamo * tasaInteresMensual
        * potencia / (potencia - 1m);
}

cuotaFija = Math.Round(cuotaFija, 2);


decimal saldo = montoPrestamo;


for (int i = 1; i <= plazoPrestamo; i++)
{
    
    decimal interes = Math.Round(
        saldo * tasaInteresMensual, 2);

    
    decimal abonoCapital = cuotaFija - interes;

    
    decimal pago = cuotaFija;

    if (i == plazoPrestamo || abonoCapital > saldo)
    {
        abonoCapital = saldo;
        pago = interes + abonoCapital;
    }

    
    saldo = Math.Round(saldo - abonoCapital, 2);

    
    tabla.AddRow(
        i.ToString(),
        $"{pago:N2}",
        $"{interes:N2}",
        $"{abonoCapital:N2}",
        $"{saldo:N2}");
}

AnsiConsole.Write(tabla);

Table table;



