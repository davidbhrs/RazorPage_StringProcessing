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
        [RegularExpression(@"(.*\s){9}..*", ErrorMessage = "Upps. Sie haben zu wenig Wörter eingegeben.")]
        public string inputString { get; set; }
        public bool val = false;
        public bool activated = false;
        public bool started = false;
        public List<string> historyEntry = new List<string>();
        public static List<List<string>> history = new List<List<string>>();
        public  List<List<string>> historyOutput = new List<List<string>>();


        string newString = "";
        char[] vokale = { 'a', 'e', 'i', 'o', 'u' };
        char newChar = ' ';
        string[] words = { };
        int wordNum = 0;




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

            words = inputString.Split(" ");
            if (words.Contains("exit"))
            {
                return RedirectToPage("/index");
            }

            if (ModelState.IsValid)
            {
                val = true;
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
            return Page();
        }



    }
}
