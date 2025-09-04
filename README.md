# 🧾 ExtractData

> Extração automática e precisa de dados sensíveis (nome, CPF, data de nascimento, número de documentos etc.) usando inteligência artificial e modelos LLM via OpenRouter.



## 📌 Visão Geral

O `ExtractData` é uma aplicação em C# que automatiza a extração de informações estruturadas a partir de documentos digitais como **imagens** e **PDFs**. Utiliza **modelos de linguagem multimodais** (como GPT-4-Vision e Gemini) através da API do [OpenRouter](https://openrouter.ai).

Ideal para sistemas que lidam com **dados sensíveis**, como cadastros, validação de documentos, onboarding de clientes, entre outros.



## 🧠 Motivação

Este projeto nasceu da necessidade de uma operação que lida diariamente com documentos sensíveis. A extração manual desses dados era:

- Demorada  
- Sujeita a erros  
- Difícil de escalar  

Além disso, era necessário garantir **alta disponibilidade** e **resiliência**, o que foi possível com o uso do **OpenRouter**, que permite alternar entre diferentes modelos de linguagem (OpenAI, Google, Anthropic etc.) com facilidade.



## 🧰 Tecnologias Utilizadas

- [.NET / C#](https://dotnet.microsoft.com/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [OpenRouter API](https://openrouter.ai) – gateway para LLMs multimodais
- `System.Net.Http` – envio das requisições



## ⚙️ Configuração

### ✅ Variáveis de Ambiente

Defina as seguintes variáveis antes de executar o projeto:

| Variável                | Descrição                                         | Valor padrão                                              |
|------------------------|---------------------------------------------------|------------------------------------------------------------|
| `OPENROUTER_USERNAME`  | Nome do projeto ou usuário                         | `Extension Project`                                        |
| `OPENROUTER_MODEL`     | Modelo utilizado                                   | `google/gemini-2.5-flash-image-preview:free`               |
| `OPENROUTER_APIURL`    | Endpoint da API do OpenRouter                      | `https://openrouter.ai/api/v1/chat/completions`            |
| `OPENROUTER_APIKEY`    | Chave da API (obrigatória)                         | *deve ser fornecida manualmente*                           |

Você pode definir essas variáveis diretamente no terminal ou em um arquivo `.env` com suporte externo.



## ▶️ Como Usar

### 1. Adicionar imagens à lista

```csharp
List<string> imagens = new List<string>();
imagens.Add(@"C:\caminho\para\imagem1.jpeg");
imagens.Add(@"C:\caminho\para\imagem2.png");
```

### 2. Executar extração

```csharp
var resultado = await ExtrairDadosComImagensAsync(
    "Extraia o nome, CPF e a data de nascimento dessa imagem",
    imagens
);

Console.WriteLine(resultado.ToString());

```



## 🧪 Métodos Disponíveis
🔹 ExtrairTextoComPromptAsync(string prompt)

Envia um prompt de texto puro para o modelo.

🔹 ExtrairDadosComImagensAsync(string prompt, List<string> imagens)

Converte imagens para base64 e envia junto ao prompt.

Espera uma resposta JSON estruturada.

🔹 ExtrairDadosComPdfAsync(string prompt, string pdfPath)

Envia um arquivo PDF como entrada multimodal.

Útil para contratos e documentos mais complexos.

🔹 EnviarRequisicaoAsync(object payload)

Método interno que envia o payload via POST ao OpenRouter.



## 💡 Exemplo de Prompt

```csharp
Extraia o nome, CPF e a data de nascimento dessa imagem.
Retorne os dados no seguinte formato JSON:
{ "nome": "", "data_nascimento": "", "cpf": "" }
```



## ✅ Benefícios

🚀 Alta assertividade com modelos LLM de última geração

🔀 Escolha flexível de modelos (Gemini, GPT-4-Vision, Claude etc.)

📷 Suporte a imagens e PDFs como entrada

💡 Fácil integração em sistemas existentes

🧱 Código limpo, modular e pronto para escalar



## 📜 Licença

Projeto de uso interno e educacional.
Atenção: certifique-se de validar o uso de LLMs com dados sensíveis conforme a legislação de privacidade (ex: LGPD, GDPR).

---

🤝 Contribuição

Sinta-se à vontade para abrir issues ou forks para melhorias.
Este projeto é uma base e pode ser expandido para múltiplos casos de uso corporativos.
