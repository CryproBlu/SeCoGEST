// Effettua l'aggiunta del metodo trim nel prototipo delle stringhe
if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/gm, '');
    };
}

// Restituisce la larghezza della finestra del browser
function GetWindowsWidth() {
    var winW = 630;
    if (document.body && document.body.offsetWidth) {
        winW = document.body.offsetWidth;
    }
    if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
        winW = document.documentElement.offsetWidth;
    }
    if (window.innerWidth && window.innerHeight) {
        winW = window.innerWidth;
    }
    return winW;
}


// Restituisce l'altezza della finestra del browser
function GetWindowsHeight() {
    var winH = 460;
    if (document.body && document.body.offsetWidth) {
        winH = document.body.offsetHeight;
    }
    if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
        winH = document.documentElement.offsetHeight;
    }
    if (window.innerWidth && window.innerHeight) {
        winH = window.innerHeight;
    }
    return winH;
}


function getOffset(el) {
    var _x = 0;
    var _y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
        _x += el.offsetLeft - el.scrollLeft;
        _y += el.offsetTop - el.scrollTop;
        el = el.offsetParent;
    }
    return { top: _y, left: _x };
}



// Aggiorna l'eventuale oggetto Validator Rummary inserito nella pagina
function UpdateValidatorSummary() {
    ValidationSummaryOnSubmit(null);
}




// Restituisce un valore booleano che indica se il parametro passato si riferisce ad un numero o meno
function isNumeric(x) {
    // I use this function like this: if (isNumeric(myVar)) { } 
    // regular expression that validates a value is numeric 

    var RegExp = /^(-)?(\d*)(\.?)(\d*)$/;
    // Note: this WILL allow a number that ends in a decimal: -452. 
    // compare the argument to the RegEx 
    // the 'match' function returns 0 if the value didn't match 

    var result = x.match(RegExp);
    return result;
}




// Permette la sottoscrizione ad un evento specifico
function addEvent(obj, type, fn) {
    if (obj.attachEvent) {
        obj['e' + type + fn] = fn;
        obj[type + fn] = function () { obj['e' + type + fn](window.event); }
        obj.attachEvent('on' + type, obj[type + fn]);
    } else
        obj.addEventListener(type, fn, false);

    //    Esempio di impiego:
    //    addEvent(window, 'resize', function (event) {
    //        alert('The page has been resized');
    //    });
}




//Blocca la pagina mostrando un div semi-trasparente a copertura dei contenuti.
//Gli oggetti che devono restare utilizzabili devono avere uno z-index maggiore di 9998.
function LockPage() {
    var divPageBlocker = document.getElementById("divPageBlocker");
    if (divPageBlocker != null) divPageBlocker.style.display = "Block";
}


//Blocca la pagina mostrando un div semi-trasparente a copertura dei contenuti.
//Gli oggetti che devono restare utilizzabili devono avere uno z-index maggiore di 9998.
function UnlockPage() {
    var divPageBlocker = document.getElementById("divPageBlocker");
    if (divPageBlocker != null) divPageBlocker.style.display = "None";
}


//Blocca la pagina mostrando un div semi-trasparente a copertura dei contenuti ed un messaggio che indica il caricamento dei dati
//Gli oggetti che devono restare utilizzabili devono avere uno z-index maggiore di 9998.
function ShowLoading() {
    var divPageLoading = document.getElementById("divPageLoading");
    if (divPageLoading) divPageLoading.style.display = "Block";
}

function HideLoading() {
    var divPageLoading = document.getElementById("divPageLoading");
    if (divPageLoading) divPageLoading.style.display = "None";
}

function RadAjaxPanelOnResponseEndHideLoading(sender, args) {
    HideLoading();
}


/**************************************
Controllo del Codice Fiscale
***************************************/
function ControllaCodiceFiscale(cf) {
    var validi, i, s, set1, set2, setpari, setdisp;
    if (cf == '') return '';
    cf = cf.toUpperCase();
    if (cf.length != 16)
        return "La lunghezza del codice fiscale non è\n"
		+ "corretta: il codice fiscale dovrebbe essere lungo\n"
		+ "esattamente 16 caratteri.\n";
    validi = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    for (i = 0; i < 16; i++) {
        if (validi.indexOf(cf.charAt(i)) == -1)
            return "Il codice fiscale contiene un carattere non valido `" +
				cf.charAt(i) +
				"'.\nI caratteri validi sono le lettere e le cifre.\n";
    }
    set1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    set2 = "ABCDEFGHIJABCDEFGHIJKLMNOPQRSTUVWXYZ";
    setpari = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    setdisp = "BAKPLCQDREVOSFTGUHMINJWZYX";
    s = 0;
    for (i = 1; i <= 13; i += 2)
        s += setpari.indexOf(set2.charAt(set1.indexOf(cf.charAt(i))));
    for (i = 0; i <= 14; i += 2)
        s += setdisp.indexOf(set2.charAt(set1.indexOf(cf.charAt(i))));
    if (s % 26 != cf.charCodeAt(15) - 'A'.charCodeAt(0))
        return "Il codice fiscale non è corretto:\n" +
			"il codice di controllo non corrisponde.\n";
    return "";
}


/**************************************
Controllo della PARTITA IVA
***************************************/
function ControllaPartitaIVA(pi) {
    if (pi == '') return '';
    if (pi.length != 11)
        return "La lunghezza della partita IVA non è\n" +
			"corretta: la partita IVA dovrebbe essere lunga\n" +
			"esattamente 11 caratteri.\n";
    validi = "0123456789";
    for (i = 0; i < 11; i++) {
        if (validi.indexOf(pi.charAt(i)) == -1)
            return "La partita IVA contiene un carattere non valido `" +
				pi.charAt(i) + "'.\nI caratteri validi sono le cifre.\n";
    }
    s = 0;
    for (i = 0; i <= 9; i += 2)
        s += pi.charCodeAt(i) - '0'.charCodeAt(0);
    for (i = 1; i <= 9; i += 2) {
        c = 2 * (pi.charCodeAt(i) - '0'.charCodeAt(0));
        if (c > 9) c = c - 9;
        s += c;
    }
    if ((10 - s % 10) % 10 != pi.charCodeAt(10) - '0'.charCodeAt(0))
        return "La partita IVA non è valida:\n" +
			"il codice di controllo non corrisponde.\n";
    return '';
}




/* Restituisce un valore booleano che indica se la data passata è futura o meno */
function ControllaSeDataFutura(stringaDataDaControllare) {
    //debugger;

    if (stringaDataDaControllare == undefined) {
        return false;
    }

    var anno = parseInt(stringaDataDaControllare.substring(0, 4), 10);
    var mese = parseInt(stringaDataDaControllare.substring(5, 7), 10) - 1;
    var giorno = parseInt(stringaDataDaControllare.substring(8, 10), 10);

    var dataDaControllare = new Date(anno, mese, giorno);
    var dataOdierna = new Date();

    return (dataDaControllare > dataOdierna);
}

/* Effettua la conversione della stringa passata in una data (oggetto Date()) */
function ConvertStringToDate(dateString) {

    // Controllo il parametro passato
    if (dateString == undefined || dateString == null || !(typeof(dateString) == 'string' && isNaN(dateString)) || dateString == "") {
        return null;
    }

    var anno = parseInt(dateString.substring(0, 4), 10);
    var mese = parseInt(dateString.substring(5, 7), 10) - 1;
    var giorno = parseInt(dateString.substring(8, 10), 10);

    return new Date(anno, mese, giorno);

    //try {
    //    var newDate = new Date(dateString);

    //    if (isNaN(newDate)) {
    //        throw "Impossibile generare un oggetto Date in base alla data passata come parametro.";
    //    }
    //    else {
    //        return newDate;
    //    }        
    //}
    //catch (ex) {

    //    var anno = parseInt(dateString.substring(0, 4), 10);
    //    var mese = parseInt(dateString.substring(5, 7), 10) - 1;
    //    var giorno = parseInt(dateString.substring(8, 10), 10);

    //    return new Date(anno, mese, giorno);
    //}    
}

/* Effettua la conversione della stringa passata in una datetime (oggetto Date()) */
function ConvertStringToDateTime(dateTimeString) {

    // Controllo il parametro passato
    if (dateTimeString == undefined || dateTimeString == null || !(typeof (dateTimeString) == 'string' && isNaN(dateTimeString)) || dateTimeString == "") {
        return null;
    }
    try {
        var newDate = new Date(dateTimeString);

        if (isNaN(newDate)) {
            throw "Impossibile generare un oggetto DateTime in base alla data passata come parametro.";
        }
        else {
            return newDate;
        }
    }
    catch (ex) {

        var anno = parseInt(dateTimeString.substring(0, 4), 10);
        var mese = parseInt(dateTimeString.substring(5, 7), 10) - 1;
        var giorno = parseInt(dateTimeString.substring(8, 10), 10);
        var ora = parseInt(dateTimeString.substring(11, 13), 10);
        var minuti = parseInt(dateTimeString.substring(14, 16), 10)
        var secondi = parseInt(dateTimeString.substring(17, 19), 10);

        return new Date(anno, mese, giorno, ora, minuti, secondi);
    }
}

// Effettua la sostituzione del carattere > con &gt; ed il carattere < con &lt;
function ReplaceGreaterThanLessThanInputTextTextAreaValue() {
    
    if (typeof ($) == "undefined") { return; }

    // Recupero gli oggetti
    var $selector = $("textarea, input[type='text']");

    // Verifico che ci siano degli elementi
    if ($selector != null && $selector.length > 0) {

        // Per ogni elemento effettuo la sostituzione
        $selector.each(function () {
            var html = $(this).val();
            html = html.replace("<", "&lt;");
            html = html.replace(">", "&gt;");
            $(this).val(html);
        });
    }
}

// Effettua la sostituzione del carattere &gt; con > ed il carattere &lt; con < al caricamento
function ReplaceGreaterThanLessThanInputTextTextAreaValueOnLoad() {
    
    $(document).ready(function () {
        // Recupero gli oggetti
        var $selector = $("textarea, input[type='text']");

        // Verifico che ci siano degli elementi
        if ($selector != null && $selector.length > 0) {

            // Per ogni elemento effettuo la sostituzione
            $selector.each(function () {
                var html = $(this).val();
                html = html.replace("&lt;", "<");
                html = html.replace("&gt;", ">");
                $(this).val(html);
            });
        }
    });
}

// Abilita o disabilita il validatore che ha come id un valore identico a quello pasato come parametro
function EnableValidator(validatorID, enabled) {

    if (validatorID == null) { return; }
    if (enabled == null) { enabled = false; }

    var validatorObject = document.getElementById(validatorID);
    if (validatorObject != null) {
        validatorObject.enabled = enabled;
        ValidatorUpdateDisplay(validatorObject);
    }    
}

// Mostra lo span il cui id è passato come parametro
function ShowImage(imageID, show) {
    if (show == null || typeof show != "boolean") {
        show = false;
    }

    var imageObject = $get(imageID);
    if (imageObject != null)
    {
        if (show) {
            imageObject.setAttribute("style", "display:block;");
        }
        else {
            imageObject.setAttribute("style", "display:none;");
        }
    }
}

// Recupera il valore dal combo passato come parametro
function CheckRadComboBoxValueIsAssociated(radCombo) {
    if (radCombo != null &&
        radCombo.get_value &&
        radCombo.get_text) {

        // Se il combo contiene un valore oppure sia il testo che il valore del combo sono stringhe vuote, allora viene restituito true
        if ((radCombo.get_value() != null && radCombo.get_value() != "") ||
            (radCombo.get_text() == "" && radCombo.get_value() == ""))
        {
            return true;
        }
        else {
            return false;
        }        
    }
    else {
        return false;
    }
}

// Effettua la validazione del validatore il cui clientID e validation groupo sono passati come parametro
function ValidateValidator(validatorClientID, validatorValidationGroup) {

    // Get the specific validator element
    var validator = $get(validatorClientID);

    // Validate chosen validator
    ValidatorValidate(validator);

    // Update validation summary for chosen validation group
    ValidatorUpdateIsValid();
    ValidationSummaryOnSubmit(validatorValidationGroup);

    if (!Page_ClientValidate(validatorValidationGroup)) {
        validator.style.visibility = "visible";
        return false;
    }
    else {
        return true;
    }
}

// Pulisce e nasconde il validation summary il cui ID è passato come un parametro
function ClearAndHideValidationSummary(validationSummaryClientID, hideValidator) {
    if (hideValidator == null || typeof hideValidator != "boolean") {
        hideValidator = true;
    }

    var validationSummary = $get(validationSummaryClientID);
    if (validationSummary == null) {
        return;
    }

    validationSummary.innerHTML = "";
    validationSummary.innerText = "";

    if (validationSummary === true) {
        validationSummary.style.display = "none";
    }
}

// Effettua la validazione del formato relativo all'indirizzo email passato come parametro
function EmailAddressIsValid(emailAddress) {

    if (emailAddress == null) emailAddress = "";

    var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(emailAddress);
}

function calcolaRicaricoValoreDaValoreIniziale(valoreIniziale, percentuale) {
    if (valoreIniziale == null || valoreIniziale == "") valoreIniziale = 0;
    if (percentuale == null || percentuale == "") percentuale = 100;

    var valore = (valoreIniziale / 100) * percentuale;
    return valore;
}

function calcolaRicaricoValoreDaValoreFinale(valoreFinale, percentuale) {

    if (valoreFinale == null || valoreFinale == "") valoreFinale = 0;
    if (percentuale == null || percentuale == "") percentuale = 100;

    var valore = Math.round((valoreFinale / 100) * percentuale);
    return valore;
}

function calcolaRicaricoPercentuale(valoreIniziale, valoreFinale) {
    if (valoreIniziale == null || valoreIniziale == "") valoreIniziale = 0;
    if (valoreFinale == null || valoreFinale == "") valoreFinale = 0;

    var incremento = valoreFinale - valoreIniziale;
    var aumento = 0;

    if (valoreFinale > 0) {
        aumento = (incremento / valoreIniziale) * 100;
    }

    return aumento;
}