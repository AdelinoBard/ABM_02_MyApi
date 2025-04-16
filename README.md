# ABM_MyApi

API RESTful em .NET funcional, enxuta e bem estruturada 

Criação de uma **API RESTful** em .NET com estrutura organizada, implementando um controller (`UsersController`) com operações _CRUD_ básicas usando uma lista estática de usuários e seguindo boas práticas iniciais de roteamento e versionamento.

- **Create (_Criar_)** – Adicionar novos registros ou dados ao sistema.
- **Read (_Ler_)** – Consultar ou visualizar os dados armazenados.
- **Update (_Atualizar_)** – Modificar registros existentes.
- **Delete (_Excluir_)** – Remover dados do sistema.

---

## **O que foi feito!**

### Estrutura do Projeto
- `Controllers/`, `Models/`: organizados.
- `Program.cs`: usando o estilo minimalista do .NET 6+.

### Roteamento
- Uso de `[Route("api/v1/[controller]")]`: já preparando versionamento da API.

### CRUD Completo
- O controller cobre todos os métodos básicos: `GET`, `GET by id`, `POST`, `PUT`, `DELETE`.

### Boas práticas já aplicadas:
- Verificação de existência do usuário antes de atualizar/deletar.
- Uso do `CreatedAtAction` no `POST`.
- Retorno apropriado (`NotFound()`, `NoContent()`, `Ok()`).

---

## **A fazer!**

### 1. Separar a lógica de negócios (usar `Services/`)

Atualmente, o controller está manipulando a lista `_users`. Isso é OK pra aprender, mas em produção, podemos separar isso em uma camada de serviço, por exemplo:

```csharp
// Services/UserService.cs
public class UserService
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "John Doe" },
        new User { Id = 2, Name = "Jane Smith" }
    };

    public IEnumerable<User> GetAll() => _users;
    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
    public void Add(User user) => _users.Add(user);
    public bool Update(int id, User updated)
    {
        var user = GetById(id);
        if (user is null) return false;
        user.Name = updated.Name;
        return true;
    }
    public bool Delete(int id)
    {
        var user = GetById(id);
        return user is not null && _users.Remove(user);
    }
}
```

Depois injetamos no controller via construtor.

---

### 2. Adicionar validação (usando `DataAnnotations`)

Podemos validar a entrada de dados:

```csharp
public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
```

E no `CreateUser`, basta checar `ModelState` (mas o ASP.NET faz isso automaticamente com `[ApiController]`).

---

### 3. Incrementar o ID automaticamente

Atualmente o `POST` espera que o cliente mande um ID. Melhor seria o servidor gerar isso:

```csharp
[HttpPost]
public ActionResult<User> CreateUser(User user)
{
    user.Id = _users.Max(u => u.Id) + 1;
    _users.Add(user);
    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
}
```

---

### 4. Habilitar o uso de serviços no `Program.cs` (se for usar Services depois)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<UserService>(); // exemplo

var app = builder.Build();

app.MapControllers();

app.Run();
```

---
