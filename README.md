# Sistema de Vendas — Console C# (.NET 10 LTS)

Projeto direcionado ao **.NET 10**, 

## Pré-requisito

.NET SDK 10.0 instalado (`dotnet --version` deve mostrar `10.x`).
Download: https://dotnet.microsoft.com/download/dotnet/10.0

## Como rodar

```bash
dotnet run
```

(dentro da pasta do projeto, onde está o `LojaVendas.csproj`)

## Estrutura

```
Models/
  Cliente.cs
  Venda.cs
Pagamentos/
  FormaPagamento.cs   (classe abstrata)
  PagamentoPix.cs
  PagamentoCartao.cs
  PagamentoDinheiro.cs
Program.cs             (menu / top-level statements)
```

## Onde aparece cada solicitação

- **Encapsulamento** — `Cliente.Cpf` só tem `get` (definido uma vez, no
  construtor). `Venda.ValorCompra` e `Venda.Situacao` também só têm `get`
  público; a única forma de mudar a situação para `Pago` é chamando
  `venda.Pagar(formaPagamento)`, que valida as regras de negócio
  (valor > 0 ao criar, venda paga não pode ser paga de novo).

- **Herança** — `PagamentoPix`, `PagamentoCartao` e `PagamentoDinheiro`
  herdam de `FormaPagamento`.

- **Abstração** — `FormaPagamento` é `abstract`: define o método
  `CalcularValorFinal(decimal valor)` sem dizer como calcular, e não pode
  ser instanciada diretamente (`new FormaPagamento()` não compila).

- **Polimorfismo** — em `Venda.Pagar` e em `RealizarPagamento()` no
  `Program.cs`, o código trabalha sempre com a variável do tipo
  `FormaPagamento`, chamando `CalcularValorFinal`, sem nenhum
  `if (formaPagamento is PagamentoPix)`. Quem decide a regra é o próprio
  objeto concreto que foi criado (Pix, Cartão ou Dinheiro).

## Exemplo de uso

```
1 - Cadastrar venda
Número: 1
Cliente: Ana Souza
CPF: 12345678900
Valor: 500

Venda cadastrada com sucesso!
Situação: Pendente

3 - Realizar pagamento
Número da venda: 1
Escolha a forma de pagamento:
1 - PIX
Opção: 1

Valor original: R$ 500,00
Forma de pagamento: PIX
Valor final: R$ 475,00
Pagamento realizado com sucesso.
```
