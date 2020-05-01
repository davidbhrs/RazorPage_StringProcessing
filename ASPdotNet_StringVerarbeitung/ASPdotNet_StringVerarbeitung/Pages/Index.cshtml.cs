using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ASPdotNet_StringVerarbeitung.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }



        /* Variablen */
        [BindProperty]
        public string inputString { get; set; }
        
        string newString = "";
        char[] vokale = { 'a' , 'e', 'i', 'o', 'u'};
        char newChar = ' ';



        /* Methoden */
        public void OnGet()
        {

        }

        public void OnPost()
        {
            string[] words = inputString.Split(" ");
            int wordNum = words.Length;
            if (wordNum >= 10)
            {
                foreach (var character in inputString)
                {
                    if (vokale.Contains(character))
                    {
                        newChar = 'i';
                        newString += newChar;
                    }
                    else
                    {
                        newString += character;
                    }
                }
            }
            else
            {
                newString = "Sie müssen mindestens 10 Wörter eingeben," +
                    " um die App verwenden zu können. In Ihrer aktuellen " +
                    $"Zeichenkette befinden sich allerdings nur { wordNum } " +
                    "Wörter.";
            }
            ViewData["result"] = newString;
        }
    }
}
