using ApplicationFormApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace ApplicationFormApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public QuestionsController(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer("ApplicationDb", "ApplicationContainer");
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Text = questionDto.Text,
                Type = questionDto.Type,
                Options = questionDto.Options,
            };

            await _container.CreateItemAsync(question, new PartitionKey(question.Id));
            return CreatedAtAction(nameof(GetQuestion), new { Id = question.Id } question);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Question>(id, new PartitionKey(id));
                return Ok(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = id,
                Text = questionDto.Text,
                Type = questionDto.Type,
                Options = questionDto.Options,
            };

            await _container.ReplaceContainerAsync(question, id, new PartitionKey(id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            await _container.DeleteItemAsync<Question>(id, new PartitionKey());
            return NoContent();
        }
    }

    }
