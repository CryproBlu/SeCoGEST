/*function ruCaricamentoAllegato_OnClientFileSelected(sender, eventArgs, eventTargetMemorizzaFileCaricati, userControlID) {
    if (sender == null || !sender.getFileInputs) return;

    var inputs = sender.getFileInputs();
    if (inputs != null && $(inputs).size() > 0) {
        __doPostBack(eventTargetMemorizzaFileCaricati, userControlID);
    }
    else {
        radalert("Non è stato selezionato nessun file.", 500, 200, "Attenzione", null, null);
    }
}*/

function rbCarica_OnClientClicking(sender, eventArgs, rauCaricamentoAllegatoClientID) {
    try {

        var rauCaricamentoAllegato = $find(rauCaricamentoAllegatoClientID);
        if (rauCaricamentoAllegato != null && rauCaricamentoAllegato._fileInput != null) {

            if (rauCaricamentoAllegato._fileInput.value == null || $.trim(rauCaricamentoAllegato._fileInput.value) == "") {
                radalert("Non è stato selezionato nessun file.", 500, 200, "Attenzione", null, null);
                eventArgs.set_cancel(true);
            }
        }
        else {
            radalert("Non è stato possibile recuperare l'oggetto che effettua l'upload del file.", 500, 200, "Attenzione", null, null);
            eventArgs.set_cancel(true);
        }
        
    } catch (ex) {
        radalert(ex.message, 500, 200, "Attenzione", null, null);

        if (eventArgs != null) {
            eventArgs.set_cancel(true);
        }        
    }
}

function OnRadProgressManagerClientProgressStarted(sender, eventArgs, userControlID, rpaCarcamentoAllegati_ClientID) {
    var progressArea = $find(rpaCarcamentoAllegati_ClientID);

    if (progressArea == null) return;

    if (sender != null && sender._selectedFilesCount != undefined && sender._selectedFilesCount == 0) {
        progressArea.hide();
    }
    else {
        progressArea.show();
    }
}