using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations; // namespace for validation
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


        /*
         *  Variablen 
         */
         // public Variablen auf die von der View aus zugegriffen wird
        [BindProperty]
        [Required(ErrorMessage = "Sie haben keine Zeichenkette eingegeben")]
        [RegularExpression(@"\s*(\w+\s+){9}\w.*", ErrorMessage = "Upps. Sie haben zu wenig Wörter eingegeben.")]
        public string inputString { get; set; } // eingegebene Zeichenkette
        public bool activated = false; // Variable, ob die Anwendung schon einmal ausgeführt wurde oder nicht
        public bool started = false; // Startvariable, um die eigentliche Anwendung anzuzeigen
        public List<List<string>> historyOutput = new List<List<string>>(); // Output der Historie
        // Anzahl der ersetzten Vokale
        public int aNum = 0;
        public int eNum = 0;
        public int iNum = 0;
        public int oNum = 0;
        public int uNum = 0;
        [ViewData]
        public string newString { get; set; }
        [ViewData]
        public int wordNum { get; set; }

        // Variablen die nur im Page Modell existieren
        char[] vokale = { 'a', 'e', 'o', 'u' }; 
        string[] words = { };
        List<string> historyEntry = new List<string>();
        // Historie als statische Variable, damit sie auch nach einem POST beständig ist
        static List<List<string>> history = new List<List<string>>();



        public IActionResult OnPostStarting()
        {
            started = true;
            return Page();
        }

        public IActionResult OnPostProcessing()
        {
            started = true;
            activated = true;

            words = inputString.Split(" ");
            if (words.Contains("exit"))
            {
                history = new List<List<string>>();
                return RedirectToPage("/index");
            }

            if (ModelState.IsValid)
            {
                inputString = inputString.ToLower();
                
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
                        newString += 'i';
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

            return Page();
        }

        public IActionResult OnPostExitPage()
        {
            history = new List<List<string>>();
            return RedirectToPage("/index");
        }

    }
}
