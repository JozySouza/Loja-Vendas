namespace LojaVendas.Pagamentos;

/// <summary>
/// Dinheiro: o valor final é igual ao valor da compra, sem desconto
/// nem acréscimo.
/// </summary>
public class PagamentoDinheiro : FormaPagamento
{
    public override string Nome => "Dinheiro";

    public override decimal CalcularValorFinal(decimal valor)
    {
        return valor;
    }
}
