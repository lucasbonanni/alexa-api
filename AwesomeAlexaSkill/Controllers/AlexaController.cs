using System.Web.Http;

namespace AwesomeAlexaSkill.Controllers
{
    public class AlexaController : ApiController
    {
        [HttpPost,Route("api/alexa/awesome")]
        public dynamic AwesomeAlexaSkill(dynamic request)
        {
            return new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Hello world"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "AwesomeAlexaSkill",
                        content = "Intelligence is the ability to adapt to change."
                    },
                    shouldEndSession = true
                }
            };
        }
    }
}
