using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ASPdotNet_StringVerarbeitung.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Sie haben keine Zeichenkette eingegeben")]
        [RegularExpression(@"(\w+\s){9}..*", ErrorMessage = "Upps. Sie haben zu wenig Wörter eingegeben.")]
        public string inputString { get; set; }
        public bool activated = false;
        public bool started = false; // Startvariable, um die eigentliche Anwendung anzuzeigen
        public List<List<string>> historyOutput = new List<List<string>>();
        public int aNum = 0;
        public int eNum = 0;
        public int iNum = 0;
        public int oNum = 0;
        public int uNum = 0;

        string newString = "";
        char[] vokale = { 'a', 'e', 'o', 'u' };
        char newChar = ' ';
        string[] words = { };
        int wordNum = 0;
        List<string> historyEntry = new List<string>();
        static List<List<string>> history = new List<List<string>>();

        


        /* Methoden */
        public void OnGet()
        {

        }

        public IActionResult OnPostStarting()
        {
            started = true;
            return Page();
        }

        public IActionResult OnPostProcessing()
        {
            started = true;
            activated = true;

            
            if (words.Contains("exit"))
            {
                history = new List<List<string>>();
                return RedirectToPage("/index");
            }

            if (ModelState.IsValid)
            {
                inputString = inputString.ToLower();

                words = inputString.Split(" ");
                wordNum = words.Length;

                aNum = inputString.Split('a').Length - 1;
                eNum = inputString.Split('e').Length - 1;
                oNum = inputString.Split('o').Length - 1;
                uNum = inputString.Split('u').Length - 1;

                wordNum = words.Length;
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
            historyEntry.Add(inputString);
            historyEntry.Add(newString);
            history.Add(historyEntry);
            historyOutput = history;

            ViewData["result"] = newString;
            ViewData["wordNum"] = wordNum;
            return Page();
        }

        public IActionResult OnPostExitPage()
        {
            history = new List<List<string>>();
            return RedirectToPage("/index");
        }

    }
}
