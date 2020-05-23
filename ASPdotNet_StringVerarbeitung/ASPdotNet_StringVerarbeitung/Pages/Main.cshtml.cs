﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ASPdotNet_StringVerarbeitung.Pages
{
    public class MainModel : PageModel
    {
        [BindProperty]
        [Required (ErrorMessage = "Sie haben keine Zeichenkette eingegeben")]
        [RegularExpression(@"(.*\s){9}..*", ErrorMessage = "Upps. Sie haben zu wenig Wörter eingegeben.")]
        public string inputString { get; set; }
        public bool val = false;
        public bool activated = false;

        string newString = "";
        char[] vokale = { 'a', 'e', 'i', 'o', 'u' };
        char newChar = ' ';
        string[] words = { };
        int wordNum = 0;

        



        /* Methoden */
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            activated = true;
            if (ModelState.IsValid)
            {
                val = true;

                words = inputString.Split(" ");
                wordNum = words.Length;

                if (words.Contains("exit"))
                {
                    return RedirectToPage("/index");
                }

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
                //newString = "Ihre Eingabe war ungültig.";
            }

            ViewData["result"] = newString;
            return Page();
        }
    }
}