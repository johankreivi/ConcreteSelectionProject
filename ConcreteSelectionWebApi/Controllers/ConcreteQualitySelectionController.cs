using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Helpers;
using Entity;
using Microsoft.Extensions.Configuration;

namespace ConcreteSelectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcreteQualitySelectionController : ControllerBase
    {
        private Node _rootNode;
        private IConfiguration _configuration;
        private readonly IConfigurationSection _dgmlFilePathSection;
        private ILogger<ConcreteQualitySelectionController> _logger;

        public ConcreteQualitySelectionController(IConfiguration configuration, ILogger<ConcreteQualitySelectionController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _dgmlFilePathSection = configuration.GetSection("DgmlFilePaths");
            _rootNode = new Node("placeholder", "placeholder");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Node>> Get(string nodeId = null, string fileName = "unreinforcedConcrete") //TODO: Change fileName to master dgml
            {

            try
            {
                DgmlParser parser = new DgmlParser();
                _logger.LogInformation("DGML file path section: " + _dgmlFilePathSection);

                var dgmlFilePath = _dgmlFilePathSection.GetSection(fileName).Value;

                if (string.IsNullOrEmpty(dgmlFilePath))
                {
                    _logger.LogError("DGML file path is not configured");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                _rootNode = parser.ParseDgml(dgmlFilePath);
                if (string.IsNullOrEmpty(nodeId))
                {
                    return Ok(_rootNode); // Return default starting point
                }

                var currentNode = FindNode(_rootNode, nodeId);
                return currentNode != null ? Ok(currentNode.Children) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while parsing DGML file");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        private Node FindNode(Node node, string nodeId)
        {
            // Implement logic to find a node by ID using DgmlParser
            // Return null if not found

            var parser = new DgmlParser();



            return null;
        }
    }
}
