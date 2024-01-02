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
        }

        [HttpGet("{fileName}/{rootNodeName}")]
        public ActionResult<IEnumerable<Node>> Get(string fileName = "completeTree", string rootNodeName = "Konstruktionstyp1")
            //TODO: Change fileName to master dgml, add root node id name
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

                var result = parser.ParseDgml(dgmlFilePath, rootNodeName);
                return Ok(result);
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
