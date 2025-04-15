// Importa namespaces necessários para o funcionamento da API:
using Microsoft.AspNetCore.Mvc;        // Funcionalidades para criar controladores e endpoints REST
using MyApi.Models;                   // Importa os modelos definidos na pasta Models (ex: classe User)
using System.Collections.Generic;     // Permite o uso de coleções genéricas, como List<T>
using System.Linq;                    // Permite o uso de métodos LINQ (ex: FirstOrDefault)

namespace MyApi.Controllers           // Define o namespace do controlador (pode ser organizado por projeto)
{
    // Indica que esta classe é um controlador de API
    [ApiController]

    // Define a rota base para todos os endpoints deste controlador: "api/v1/users"
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        // Lista estática de usuários simulando um "banco de dados" em memória
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Name = "John Doe" },
            new User { Id = 2, Name = "Jane Smith" }
        };

        // GET: api/v1/users
        // Retorna todos os usuários cadastrados
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_users); // Retorna HTTP 200 com a lista de usuários
        }

        // GET: api/v1/users/{id}
        // Retorna um usuário específico pelo ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // Busca o usuário
            if (user == null)
            {
                return NotFound(); // Retorna HTTP 404 se não encontrado
            }
            return Ok(user); // Retorna HTTP 200 com o usuário
        }

        // POST: api/v1/users
        // Cria um novo usuário
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _users.Add(user); // Adiciona o usuário à lista
            // Retorna HTTP 201 com a localização do novo recurso criado
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/v1/users/{id}
        // Atualiza os dados de um usuário existente
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // Busca o usuário
            if (user == null)
            {
                return NotFound(); // Retorna 404 se não encontrado
            }

            user.Name = updatedUser.Name; // Atualiza o nome
            return NoContent(); // Retorna HTTP 204 (sem conteúdo)
        }

        // DELETE: api/v1/users/{id}
        // Remove um usuário da lista
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // Busca o usuário
            if (user == null)
            {
                return NotFound(); // Retorna 404 se não encontrado
            }

            _users.Remove(user); // Remove o usuário da lista
            return NoContent(); // Retorna 204 indicando sucesso sem conteúdo
        }
    }
}
