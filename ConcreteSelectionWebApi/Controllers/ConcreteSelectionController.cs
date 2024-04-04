using ConcreteSelectionWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteSelectionWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConcreteSelectionController : ControllerBase
    {
        private readonly ILogger<ConcreteSelectionController> _logger;
        private readonly IDgmlLabelBasedReaderService _dgmlLabelBasedReaderService;
        private const string _entryPointLabel = "Konstruktionstyp";
        IDgmlLabelBasedReaderService readerService = new DgmlLabelBasedReaderService();

        public ConcreteSelectionController(ILogger<ConcreteSelectionController> logger, IDgmlLabelBasedReaderService dgmlLabelBasedReaderService)
        {
            _logger = logger;
            _dgmlLabelBasedReaderService = dgmlLabelBasedReaderService;

        }

        [HttpGet(Name = "ConcreteSelections")]
        public IActionResult Get()
        {
            try
            {
                string jsonTree = readerService.GetNodeTreeByLabelAsJson(_entryPointLabel);

                return Ok(jsonTree);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
