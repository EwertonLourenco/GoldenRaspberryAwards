# Golden Raspberry Awards API

Esta API RESTful permite consultar dados dos indicados e vencedores da categoria "Pior Filme" do **Golden Raspberry Awards**, com base nos intervalos de premiações para produtores.

## Funcionalidades

- Importa os dados do arquivo `movielist.csv` para um banco de dados em memória.
- Calcula:
  - O **menor intervalo** entre prêmios consecutivos para produtores.
  - O **maior intervalo** entre prêmios consecutivos para produtores.
- Expõe os resultados em um formato JSON conforme especificado.

## Requisitos do Sistema

- **.NET 6 ou superior** instalado.
- A API usa um banco de dados em memória por padrão.
- O projeto utiliza **Swagger** para documentação dos endpoints.

## Como Executar o Projeto

1. Clone este repositório:
   ```
   git clone https://github.com/EwertonLourenco/GoldenRaspberryAwards.git
   cd SEU-REPOSITORIO
   ```

2. Restaure as dependências do projeto:
   ```
   dotnet restore
   ```

3. Execute o projeto:
   ```
   dotnet run
   ```

4. Acesse o Swagger para testar os endpoints:
   - URL padrão: http://localhost:5000/swagger

## Endpoints

### `GET /api/awards/intervals`

Retorna os intervalos **mínimos** e **máximos** entre prêmios consecutivos para os produtores.

**Exemplo de Resposta**:
```json
{
  "min": [
    {
      "producer": "Bo Derek",
      "interval": 6,
      "previousWin": 1984,
      "followingWin": 1990
    }
  ],
  "max": [
    {
      "producer": "Matthew Vaughn",
      "interval": 13,
      "previousWin": 2002,
      "followingWin": 2015
    }
  ]
}
```

## Como Rodar os Testes

1. Navegue até o diretório raiz do projeto:
   ```
   cd GoldenRaspberryAwards.Tests
   ```

2. Execute os testes:
   ```
   dotnet test
   ```

3. Todos os testes de integração validarão:
   - O cálculo dos intervalos.
   - O formato da resposta dos endpoints.

## Estrutura do Projeto

```
GoldenRaspberryAwards.Api/
├── Application/
│   ├── Services/          # Lógica de negócios
│   ├── Models/            # Classes auxiliares
├── Domain/
│   ├── DTOs/              # Contratos de entrada/saída da API
│   ├── Entities/          # Representação de dados persistidos
├── Infrastructure/
│   ├── Data/              # Configuração do banco de dados
├── Presentation/
│   ├── Controllers/       # Controladores REST
├── movielist.csv          # Fonte de dados
```

## Licença

Este projeto é licenciado sob a [MIT License](https://opensource.org/licenses/MIT).

## Autores

- Éwerton Lourenço
- ewerton_lourenco@outlook.com
