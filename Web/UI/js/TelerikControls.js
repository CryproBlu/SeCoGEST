// Funzione utilizzata per il copntroll delle date inserite in un RadDatePicker
// Valida l'oggetto è stata indicata una Data futura.
function ControllaDataFutura(sender, args) {
    var stringaDataDaControllare = args.Value;
    args.IsValid = !ControllaSeDataFutura(stringaDataDaControllare);
}


function RichiediConfermaClickPulsantiToolbar(sender, args) {
    RichiediConfermaClickAggiorna(sender, args);
}

// Funzione utilizzata per avvisare l'utente nel caso di pressione del tasto Aggiorna
function RichiediConfermaClickAggiorna(sender, args) {
    var button = args.get_item();
    if (button.get_commandName() == "Aggiorna") {
        args.set_cancel(!confirm('Aggiornare i dati visualizzati?\n\nLe eventuali modifiche apportate ai dati e non salvate andranno perse!'));
    }
}

// Funzione utilizzata per effettuare un postback che indichi di caricare i dati
// per il controllo RadPanelBar che ha generato l'evento di espansione/chiusura
function CaricaInfoOperazioniOnExpand(sender, args) {
    if (sender != null && sender.get_id) {
        __doPostBack(sender.get_id(), 'fill');        
    }
    HideLoading();
}

// Funzione utilizzata per fare in modo che il caricamento dei dati del RadPanelBar
// sia effettuato solo al generarsi dell'evento di espansione e non di chiusura.
function InfoOperazioniPanelClicking(sender, args) {
    if (args != null &&
        args.get_item &&
        args.get_item() != null &&
        args.get_item().get_expanded &&
        args.get_item().get_expanded()) {

        args.set_cancel(true);
        args.get_item().set_expanded(false);
    }
}

// Setta lo z-index dei menu presenti nella pagina
function SetRadMenuZIndexStyle(zindex) {

    // Se il tipo di jquery non è definito non faccio nulla
    if (typeof ($) == "undefined") { return; }

    // Recupero gli oggetti RadMenu
    var $radMenu = $("div.RadMenu");

    // Controllo il numero di elementi
    if ($radMenu != null && $radMenu.length > 0) {

        if (zindex == null) {
            zindex = "2999";
        }

        // Per ogniuno setto lo z-index
        $.each($radMenu, function () {
            $(this).css("z-index", zindex.toString());
        });
    }
}

// Effettua il restore dello z-index dei menu presenti nella pagina
function RestoreRadMenuZIndexStyle() {

    // Se il tipo di jquery non è definito non faccio nulla
    if (typeof ($) == "undefined") { return; }

    // Recupero gli oggetti RadMenu
    var $radMenu = $("div.RadMenu");

    // Controllo il numero di elementi
    if ($radMenu != null && $radMenu.length > 0) {

        // Per ogniuno setto lo z-index
        $.each($radMenu, function () {
            // 7000 - z-index predefinito di telerik per i menu
            $(this).css("z-index", "7000");
        });
    }
}

// Recupera gli elementi dalla griglia Telerik passata come parametro
function GetRadGridDataItems(radGrid) {
    var dataItems = null;

    if (radGrid != null &&
        radGrid.get_masterTableView &&
        radGrid.get_masterTableView() != null &&
        radGrid.get_masterTableView().get_dataItems) {

        dataItems = radGrid.get_masterTableView().get_dataItems();
    }

    return dataItems;
}

// Effettua la disabilitazione delle richieste AJAX da parte della griglia quando viene effettuata una chiamata dal tasto di esportazione presente nella griglia telerik
function DisableAjaxIfEventTargetIsRadGridButtonExport(sender, eventargs) {
    
    // Viene verificata la presenza dei metodi necessari ad effettuare la disabilitazione ..
    if (eventargs != null &&
        eventargs.get_eventTarget &&
        eventargs.set_enableAjax && 
        $telerik && 
        $telerik.findGrid) {

        // Viene recuperato l'event target della richiesta AJAX
        var eventTargetElement = eventargs.get_eventTargetElement();
        if (eventTargetElement == null || eventTargetElement.id == null) return;

        var eventArgument = eventargs.get_eventArgument();
        if (eventArgument == null) return;

        // Per verificare che l'elemento sia una griglia viene recuperato l'oggetto RadGrid
        var grigliaTelerik = $telerik.findGrid(eventTargetElement.id);

        // Nel caso l'oggetto RadGrid sia valorizzato e l'EventArgument dell'evento contenga la stringa ExportTo ..
        if (grigliaTelerik != null && eventArgument.toLowerCase().indexOf("exportto") > 0) {

            // Viene disabilitato l'utilizzo della chiamate tramite AJAX
            eventargs.set_enableAjax(false);
        }
    }
}

// Mostra a video un messaggio di errore
function showMessage(title, messageBody, alertCompleted) {

    if (messageBody == null && title != null && $.trim(title) != "") {
        messageBody = title;
        title = "Attenzione";
    }

    if (title == null || $.trim(title) == "") {
        title = "Attenzione";
    }

    if (alertCompleted == null) {
        alertCompleted = function (args) { };
    }

    try {
        radalert(messageBody, 500, null, title, alertCompleted, null, null);
    }
    catch (ex) {

        var messaggio = messageBody;
        while (messaggio.indexOf("<br/>") >= 0) {
            messaggio = messageBody.replace("<br/>", "\n");
        }

        alert(messaggio);
        alertCompleted(true);
    }
}

// Richiede, mediante una finestra, delle informazioni all'utente
function askData(title, question, promptCompleted) {

    if (title == null || $.trim(title) == "") {
        title = "Titolo";
    }

    if (question == null || $.trim(question) == "") {
        question = "Testo inserimento";
    }

    if (promptCompleted == null) {
        promptCompleted = function (data) { };
    }

    try {
        radprompt(question, promptCompleted, 500, null, null, title, "");
    } catch (e) {
        var data = prompt(question, "");
        promptCompleted(data);
    }
}


// Mostra all'utente una finestra di conferma 
function askConfirm(title, question, confirmCompleted) {

    if (title == null || $.trim(title) == "") {
        title = "Titolo";
    }

    if (question == null || $.trim(question) == "") {
        question = "Domanda";
    }

    if (confirmCompleted == null) {
        confirmCompleted = function (data) { };
    }

    try {
        radconfirm(question, confirmCompleted, 500, null, null, title, null);
    } catch (e) {
        var data = confirm(question, "");
        promptCompleted(data);
    }
}

/**
 * Effettua la pulizia degli elemento richiesti da un combo con il load on-demand attivo * 
 * @param {any} combo
 */
function clearComboRequestItems(combo) {
    try {

        if (combo == null) return;

        combo.requestItems("", false);
        combo.clearItems();
        //combo.clearSelection();
    } catch (e) {
        alert(e.message);
    }
}

/**
 * Recupera il valore selezionato dal combo il cui identificativo è stato passato come parametro
 * @param {string} comboClientId
 */
function getSelectedValueFromCombo(comboClientId) {
    var combo = $find(comboClientId);

    var valore = "";
    if (combo != null && combo.get_selectedItem) {
        var selectedItem = combo.get_selectedItem();
        if (selectedItem != null && selectedItem.get_value) {
            valore = selectedItem.get_value();
        }

        if (valore == null || valore == "") {
            valore = combo.get_value();
        }
    }

    if (valore == null) valore = "";

    if (valore != null && valore != "") {
        if (valore == combo.get_emptyMessage()) valore = "";
    }

    return valore;
}

/**
 * Recupera il testo selezionato dal combo il cui identificativo è stato passato come parametro
 * @param {string} comboClientId
 */
function getTextFromCombo(comboClientId) {
    var combo = $find(comboClientId);

    var valore = "";
    if (combo != null && combo.get_selectedItem) {
        var selectedItem = combo.get_selectedItem();
        if (selectedItem != null && selectedItem.get_text) {
            valore = selectedItem.get_text();
        }

        if (valore == null || valore == "") {
            valore = combo.get_text();
        }
    }

    if (valore == null) valore = "";

    if (valore != null && valore != "") {
        if (valore == combo.get_emptyMessage()) valore = "";
    }

    return valore;
}