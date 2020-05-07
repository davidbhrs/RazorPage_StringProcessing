using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;

namespace ASPdotNet_StringVerarbeitung.Pages
{
    public class testModel : PageModel
    {
        [BindProperty]
        [Required]
        [RegularExpression(@"(.*\s){9}..*")]
        public string inputString { get; set; }

        bool startup = true;
        string newString = "";
        char[] vokale = { 'a', 'e', 'i', 'o', 'u' };
        char newChar = ' ';



        /* Methoden */
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            startup = false;
            string[] words = inputString.Split(" ");
            int wordNum = words.Length;
            if (words.Contains("exit"))
            {
                return RedirectToPage("/index");
            }
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
            return Page();
        }
    }
}