using System.Globalization;
using LojaVendas.Models;
using LojaVendas.Pagamentos;

// Cultura pt-BR para exibir valores como "R$ 1.234,56".
CultureInfo culturaBr = new CultureInfo("pt-BR");
Thread.CurrentThread.CurrentCulture = culturaBr;

List<Venda> vendas = new List<Venda>();
bool continuarExecutando = true;

while (continuarExecutando)
{
    ExibirMenu();
    string opcao = Console.ReadLine() ?? string.Empty;

    switch (opcao)
    {
        case "1":
            CadastrarVenda();
            break;
        case "2":
            ListarVendas();
            break;
        case "3":
            RealizarPagamento();
            break;
        case "0":
            continuarExecutando = false;
            Console.WriteLine("Encerrando o sistema...");
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }

    Console.WriteLine();
}

void ExibirMenu()
{
    Console.WriteLine("================================");
    Console.WriteLine("SISTEMA DE VENDAS");
    Console.WriteLine("================================");
    Console.WriteLine("1 - Cadastrar venda");
    Console.WriteLine("2 - Listar vendas");
    Console.WriteLine("3 - Realizar pagamento");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("================================");
    Console.Write("Escolha uma opção: ");
}

void CadastrarVenda()
{
    try
    {
        Console.Write("Número: ");
        int numero = int.Parse(Console.ReadLine() ?? string.Empty);

        if (vendas.Any(v => v.Numero == numero))
        {
            Console.WriteLine("Já existe uma venda cadastrada com esse número.");
            return;
        }

        Console.Write("Cliente: ");
        string nome = Console.ReadLine() ?? string.Empty;

        Console.Write("CPF: ");
        string cpf = Console.ReadLine() ?? string.Empty;

        Console.Write("Valor: ");
        decimal valor = decimal.Parse(Console.ReadLine() ?? string.Empty, culturaBr);

        // A validação de "valor > 0" e a inicialização como Pendente
        // acontecem dentro das próprias classes Cliente e Venda.
        Cliente cliente = new Cliente(nome, cpf);
        Venda venda = new Venda(numero, cliente, valor);
        vendas.Add(venda);

        Console.WriteLine();
        Console.WriteLine("Venda cadastrada com sucesso!");
        Console.WriteLine($"Situação: {venda.Situacao}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Não foi possível cadastrar a venda: {ex.Message}");
    }
}

void ListarVendas()
{
    if (vendas.Count == 0)
    {
        Console.WriteLine("Nenhuma venda cadastrada.");
        return;
    }

    foreach (Venda venda in vendas)
    {
        Console.WriteLine($"Venda: {venda.Numero}");
        Console.WriteLine($"Cliente: {venda.Cliente.Nome}");
        Console.WriteLine($"Valor original: {venda.ValorCompra:C2}");
        Console.WriteLine($"Situação: {venda.Situacao}");

        // Só existe forma de pagamento e valor final depois que a venda foi paga.
        if (venda.Situacao == SituacaoVenda.Pago && venda.FormaPagamentoUtilizada is not null)
        {
            Console.WriteLine($"Forma de pagamento: {venda.FormaPagamentoUtilizada.Nome}");
            Console.WriteLine($"Valor final: {venda.ValorFinal:C2}");
        }

        Console.WriteLine("--------------------------------");
    }
}

void RealizarPagamento()
{
    Console.Write("Número da venda: ");
    if (!int.TryParse(Console.ReadLine(), out int numero))
    {
        Console.WriteLine("Número inválido.");
        return;
    }

    Venda? venda = vendas.FirstOrDefault(v => v.Numero == numero);
    if (venda is null)
    {
        Console.WriteLine("Venda não encontrada.");
        return;
    }

    if (venda.Situacao == SituacaoVenda.Pago)
    {
        Console.WriteLine("Essa venda já foi paga.");
        return;
    }

    Console.WriteLine("Escolha a forma de pagamento:");
    Console.WriteLine("1 - PIX");
    Console.WriteLine("2 - Cartão de crédito");
    Console.WriteLine("3 - Dinheiro");
    Console.Write("Opção: ");
    string opcaoPagamento = Console.ReadLine() ?? string.Empty;

    // POLIMORFISMO em ação: a variável é do tipo abstrato FormaPagamento.
    // O switch só decide QUAL objeto concreto criar; depois disso, o
    // restante do código (venda.Pagar) não sabe e não precisa saber
    // qual é o tipo real por trás da variável.
    FormaPagamento? formaPagamento = opcaoPagamento switch
    {
        "1" => new PagamentoPix(),
        "2" => new PagamentoCartao(),
        "3" => new PagamentoDinheiro(),
        _ => null
    };

    if (formaPagamento is null)
    {
        Console.WriteLine("Forma de pagamento inválida.");
        return;
    }

    try
    {
        decimal valorOriginal = venda.ValorCompra;
        decimal valorFinal = venda.Pagar(formaPagamento);

        Console.WriteLine();
        Console.WriteLine($"Valor original: {valorOriginal:C2}");
        Console.WriteLine($"Forma de pagamento: {formaPagamento.Nome}");
        Console.WriteLine($"Valor final: {valorFinal:C2}");
        Console.WriteLine("Pagamento realizado com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Não foi possível realizar o pagamento: {ex.Message}");
    }
}
