using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations; // namespace for validation
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

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
        [RegularExpression(@"(\w+\s+){9}\w.*", ErrorMessage = "Upps. Sie haben zu wenig Wörter eingegeben.")]
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
        public string newString { get; set; } // überarbeitete Zeichenkette
        [ViewData]
        public int wordNum { get; set; } // Anzahl der eingegebenen Wörter

        // Variablen die nur im Page Modell existieren
        char[] vokale = { 'a', 'e', 'o', 'u' }; 
        string[] words = { };
        List<string> historyEntry = new List<string>();
        // Historie als statische Variable, damit sie auch nach einem POST beständig ist
        static List<List<string>> history = new List<List<string>>();


        // Starten der Anwendung, um das Eignabeformular anzuzeigen
        public IActionResult OnPostStarting()
        {
            started = true;
            return Page();
        }

        // Verarbeitung der eingegebenen Zeichenkette
        public IActionResult OnPostProcessing()
        {
            started = true;
            activated = true;

            // Die clientseitige Validierung schlägt z. B. nicht zu, wenn nur Leerzeichen eingegeben wurden
            if (inputString != null)
            {
                // splittet den String an einem Leerzeichen. Würde bei einem String der nur aus Leerzeichen besteht auf einen Fehler laufen
                words = inputString.Split(" ");
                // Wenn das "exit" eingegeben wird soll die Anwendung beendet werden, auch wenn < 10 Wörter eingegeben wurden.
                if (words.Contains("exit"))
                {
                    history = new List<List<string>>();
                    return RedirectToPage("/index");
                }
            }

            // War die Validierung positiv?
            if (ModelState.IsValid)
            {
                // Groß- & Kleinschreibung soll nicht beachtet werden
                inputString = inputString.ToLower();
                
                wordNum = words.Length;

                // Zählen der enthaltenden Vokale
                aNum = inputString.Split('a').Length - 1;
                eNum = inputString.Split('e').Length - 1;
                oNum = inputString.Split('o').Length - 1;
                uNum = inputString.Split('u').Length - 1;

                // Anzahl der Wörter
                wordNum = words.Length;

                // iterieren über jeden Buchstaben der eingegebenen Zeichenkette
                foreach (var character in inputString)
                {
                    // Wenn ein Buchstabe mit einem Vokal identisch ist, wird er durch ein i ersetzt
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
            // Hinzufügen der Werte in die Historie
            historyEntry.Add(inputString);
            historyEntry.Add(newString);
            history.Add(historyEntry);
            // Erstellung einer "Output"-Variable, auf die innerhalb der View zugegriffen werden kann
            historyOutput = history;

            return Page();
        }

        // Beenden der Anwendung, indem die Indexseite zurückgegeben wird und die Historie gelöscht wird
        public IActionResult OnPostExitPage()
        {
            history = new List<List<string>>();
            return RedirectToPage("/index");
        }

    }
}
