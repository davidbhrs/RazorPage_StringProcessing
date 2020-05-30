# Tutorial ASP.Net

In diesem Tutorial wird erläutert wie eine Razor Page erstellt wird, auf der ein User eine beliebige Zeichenkette mit mindestens zehn Wörtern eingeben kann, dessen Vokale dann vollständig mit dem Buchstaben "i" ersetzt werden. Zusätzlich soll eine Historie implementiert werden, aus der ersichtlich wird, welche Input-Strings bereits eingegeben wurden und welcher Output-String hieraus generiert wurde.



### Erstellung eines Razor-Projektes

Eine einfachere und schnellere Alternative, zu der Erstellung einer Razor Page über ein Kommandozeilenprogramm, ist die Erstellung über Visual Studio. Dies geht einfach über Create a new project -->  ASP.Net Core Web Application (C#). Nun sollte eine neue Page erstellt werden, in der die Anwendung implementiert wird. In Visual Studio geht das über einen Rechtsklick auf den Pages Ordner --> Add --> Razor Page.



### Erstellung des Input-Formulars

Als erstes muss auf der Page (.cshtml) ein Formular erstellt werden. Dieses soll aus einer textarea bestehen, in die die Zeichenkette eingegeben werden kann und einem submit-button. Die textarea muss mit dem Tag Helper "asp-for" ausgestattet werden. Hierdurch kann der eingegebene String später an das Page Model (.cshtml.cs) gebunden werden und dort auf die Eingabe zugegriffen werden. Außerdem kann ein Tag Helper "asp-page-handler" hinzugefügt werden. Hierdurch kann ein Page Handler verwendet werden, um mehrere POST-Methoden auf einer Seite realisieren zu können. Die Verwendung eines Page Handlers wird im weiteren Verlauf des Tutorials erläutert.

```html
<form method="post">
	<textarea asp-for="inputString"></textarea>
    <button type="submit" asp-page-handler="Processing">Submit</button>
</form>
```

 Durch den Tag Helper kann dann im Page Model der Wert des "inputString" an das Page Model gebunden werden und dort auf ihn zugegriffen werden.

```c#
public class MainModel : PageModel
{
	[BindProperty]
	public string inputString { get; set; }
}
```



### Validierung der Eingabe 

Nun muss überprüft werden, ob mindestens zehn Wörter eingegeben wurden, bzw. ob überhaupt eine Eingabe erfolgte. Dies vereinfacht ASP.Net, sobald der benötigte namespace geladen wurde. Die Überprüfung, ob ein Wert eingegeben wurde erfolgt einfach über das Hinzufügen von "[Required]". Für eine individuelle Fehlermeldung kann in Klammern dahinter "ErrorMessage=*" eingegeben werden. Für die Überprüfung auf die Anzahl der Wörter können RegEx mit dem Ausdruck "[RegularExpressions]" verwendet werden.  Eine RegEx zur Überprüfung von mindestens zehn Wörtern könnte lauten "(\w+\s+){9}\w.\*".  "\w+\s" steht dabei für mindestens einen Buchstaben gefolgt von mindestens einem Leerzeichen. Diese Kombination muss mindestens neun mal auftreten. Danach muss noch mindestens einmal ein Buchstabe auftauchen.

```c#
using System.ComponentModel.DataAnnotations; // benötigter namespace

[BindProperty]
[Required(ErrorMessage = "Sie haben keine Zeichenkette eingegeben")]
[RegularExpression(@"(\w+\s){9}..*", ErrorMessage = " Sie haben zu wenig Wörter eingegeben.")]
public string inputString { get; set; }
```

Wenn keine Zeichenkette eingegeben wurde, oder die eingegebene nicht mit der RegEx übereinstimmt wird die Fehlermeldung in der View (.cshtml) in einem span mit dem Tag helper "asp-validation-for" ausgegeben:

```html
<span asp-validation-for="inputString"></span>
```



### Verarbeitung des Strings

Die Verarbeitung erfolgt ausschließlich im Page Model. Bei einem submit des Formulares wir eine POST-Anfrage gestellt. Da ein Page Handler implementiert wurde, erfolgt die Verarbeitung in der Handler Methode OnPostProcessing(). Diese wird bei jedem submit des HTML-Formulars ausgeführt. Zunächst muss überprüft werden, ob die Validierung positiv war, was durch "ModelState.IsValid" erfolgt. Hiernach kann über jeden Buchstaben des eingegebenen Strings iteriert werden und überprüft werden, ob ein Vokal vorkommt. Hierzu kann z. B. ein neue Variable angelegt werden, der alle Konsonanten hinzugefügt werden und falls ein Vokal gefunden wird, ein "i" hinzugefügt wird. Diese Variable kann als ViewData implementiert werden, um sie später für die View zugänglich zu machen. 

```c#
[ViewData]
public string newString { get; set; }
 
char[] vokale = { 'a', 'e', 'o', 'u' }; // Array mit den möglichen Vokalen

public IActionResult OnPostProcessing()
{
    if (ModelState.IsValid)
    {    
    	inputString = inputString.ToLower();
    	
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

    return Page();
}
```

Bei ViewData handelt es sich um ein Dictionary, auf das die View einfach über den Key zugreifen kann.

```html
<p>@ViewData["newString"]</p>
```



### Erstellung einer Historie

Um eine Historie zu implementieren wird eine zweidimensionale Liste benötigt, die auch nach einer POST-Anfrage noch beständig ist. Die einfachste Möglichkeit das umzusetzen ist mit einer statischen Variable. Dieser Variable werden nun, nachdem die Zeichenkette überarbeitet wurde, die eingegebene Zeichenkette und die neue überarbeitete Zeichenkette hinzugefügt. Die Liste sollte allerdings nicht über ViewData zurückgegeben werden. Stattdessen kann eine public Variable erstellt werden und in der View, mit Hilfe der Razor Syntax, über diese iteriert werden.

```c#
static List<List<string>> history = new List<List<string>>(); // statische, dauerhafte Liste 
List<string> historyEntry = new List<string>(); // eindimensionale Liste für die aktuellen Werte
public List<List<string>> historyOutput = new List<List<string>>(); // Liste die in der View zurückgegeben wird

public IActionResult OnPostProcessing()
{
	// ...
	historyEntry.Add(inputString);
    historyEntry.Add(newString);
    history.Add(historyEntry);
    historyOutput = history;
}
```

Über "@Model" kann innerhalb der View auf eine Variable des Page Models zugegriffen werden. 

```html
@foreach (var entry in @Model.historyOutput)
{
    <p>@entry[0] = @entry[1]</p>
}
```

Sinnvoll wäre noch ein Button, der auf die Index Seite zurückführt und die Historie danach löscht, damit die Anwendung als beendet angesehen werden kann. Hierzu wird in der View ein Exit-Button mit einem weiteren Page Handler angelegt.

```html
<button type="submit" asp-page-handler="ExitPage">Exit</button>
```

In einer weiteren Handler Methode in dem Page Model wird die Historie nun bereinigt und die Index-Seite zurückgegeben.

```c#
public IActionResult OnPostExitPage()
{
    history = new List<List<string>>();
    return RedirectToPage("/index");
}
```



