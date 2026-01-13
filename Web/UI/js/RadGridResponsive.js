var SCREEN_XS = 360;
var SCREEN_SM = 490;
var SCREEN_MD = 640;

$(window).resize(function () {
    SettaGriglieResponsive();
});

$(window).load(function () {
    SettaGriglieResponsive();
});

//$(window).on("orientationchange", function (event) {
//    alert("orientationchange");
//    SettaGriglieResponsive();
//});

// Effettua il settaggio delle griglie Telerik che hanno la classe 'GridResponsiveColumns'
function SettaGriglieResponsive() {

    // Vengono recuperate dalla pagina le griglie Telerik Responsive
    var griglieResponsive = $("div.RadGrid.GridResponsiveColumns");

    // Nel caso in cui esistano ..
    if (griglieResponsive != null && griglieResponsive.size && griglieResponsive.size() > 0) {

        // Per ognuna vengono settate le varie impostazioni richiamando il metodo 'SettaGrigliaResponsive'
        $.each(griglieResponsive, function (index, value) {

            SettaGrigliaResponsive(value);
        });
    }
}

// Effettua il settaggio delle caratteristiche necessarie per avere una griglia telerik responsive
function SettaGrigliaResponsive(radGridHTML) {

    // Viene recuperata la griglia Telerik mediante il metodo find passando come parametro l'id presente nell'oggetto HTML
    var radGrid = $find(radGridHTML.id);

    // Nel caso in cui la griglia esista ..
    if (radGrid != null && radGrid.get_masterTableView) {

        // Viene recuperata la master table view
        var masterTableView = radGrid.get_masterTableView();

        // Nel caso in cui esista ..
        if (masterTableView != null && masterTableView.get_element) {

            // Viene recuperato il tipo di layout da utilizzare
            var isFixedLayout = ($(window).width() > SCREEN_SM);

            // Nel caso in cui si tratti di un layout fisso .. viene ridisegnata la griglia
            if (isFixedLayout) {
                radGrid.repaint();
            }

            // Viene recuperato l'oggetti HTML relativo alla master table view
            var masterTableViewElement = masterTableView.get_element();

            // Nel caso in cui l'oggetto non sia null ..
            if (masterTableViewElement != null) {

                // Viene settato il layout della master table ..
                SettaTableLayoutDellaMasterTable(masterTableViewElement, isFixedLayout);

                // Viene settata la larghezza relativa al raggruppamento delle colonne ..
                SettaLarghezzaRaggruppamentoColonne(masterTableViewElement, isFixedLayout);
            }
        }
    }
}

// Effettua il settaggio del layout relativo alla master table view
function SettaTableLayoutDellaMasterTable(masterTableViewElement, isFixedLayout) {
    var tableLayout = (isFixedLayout) ? "fixed" : "auto";
    $(masterTableViewElement).css("table-layout", tableLayout);
}

// Effettua il settaggio della larghezza del raggruppamento delle colonne
function SettaLarghezzaRaggruppamentoColonne(masterTableViewElement, isFixedLayout) {

    // Viene recuperato il raggruppamento delle colonne ..
    var colgroup = $(masterTableViewElement).find("colgroup");

    // Nel caso in cui esista ..
    if (colgroup != null && colgroup.length > 0) {

        // Vengono recuperate le colonne ..
        var cols = colgroup.find("col");

        // Nel caso in cui esistano ..
        if (cols != null && cols.length > 0) {

            // Per ogni colonna ..
            $.each(cols, function (indexCol, valueCol) {

                // Viene settata la larghezza della colonna presente nel raggruppamento
                SettaLarghezzaRaggruppamentoColonna(valueCol, isFixedLayout);
            });
        }
    }
}

// Effettua il settaggio della larghezza della colonna presente nel raggruppamento
function SettaLarghezzaRaggruppamentoColonna(colonnaHTML, isFixedLayout) {
    var display = (isFixedLayout) ? "" : "none";
    $(colonnaHTML).css("display", display);
}

// Effettua la gestione dell'evento OnGridCreated in modo da ridimensionare il form di edit e filtro con la griglia in EditMode a PopUp
function RadGrid_OnGridCreated(sender, e) {
    try {
        GestisciDimensioniEditFilterFormRadGrid(sender);
    } catch (ex) {
        alert(ex.message);
    }
}

// Effettua la gestione della dimensioni dei filtri della griglia passata come parametro
function GestisciDimensioniEditFilterFormRadGrid(griglia) {

    // Vengono ridimensionato il form di edit della griglia passata come parametro
    GestisciDimensioniEditFormRadGrid(griglia);

    // Viene recuperato il DOM della griglia passata
    var grigliaHTML = griglia.get_element();

    // Viene recuperato l'oggetto jQuery del DOM recuperato
    var $grigliaHTML = $(grigliaHTML);

    // Vengono recuperati i tasti che mostrano il form del filtro
    var $filtriGriglia = $grigliaHTML.find("button.rgActionButton.rgFilter");

    // Nel caso in cui i tasti esistano ..
    if ($filtriGriglia != null && $filtriGriglia.length > 0) {

        // Viene associata la funzione che effettua il ridimensionamento del form di filtro della griglia passata come parametro al click su uno dei tasti relativi ai filtri
        $filtriGriglia.click(function () {
            GestisciDimensioniFilterFormRadGrid(griglia);
        });
    }
}

function GestisciDimensioniEditFormRadGrid(griglia) {

    // Viene recuperato il DOM della griglia passata
    var grigliaHTML = griglia.get_element();

    // Viene recuperato l'oggetto jQuery del DOM recuperato
    var $grigliaHTML = $(grigliaHTML);

    var rgMobileForm = $grigliaHTML.find("div.rgMobileMenu.rgMobileEditForm > div.rgMobileForm");

    if (rgMobileForm != null && rgMobileForm.length > 0) {

        var rgMobileFormInnerHeight = rgMobileForm.prop('scrollHeight');
        var rgCommandRowHeight = $grigliaHTML.find("div.rgMobileMenu.rgMobileEditForm > div.rgCommandRow").height();

        var somma = rgMobileFormInnerHeight + rgCommandRowHeight;
        if ($grigliaHTML.height() < somma) {
            griglia.get_element().style.height = somma + "px";
            griglia.repaint();
        }
    }
}

function GestisciDimensioniFilterFormRadGrid(griglia) {

    // Viene recuperato il DOM della griglia passata
    var grigliaHTML = griglia.get_element();

    // Viene recuperato l'oggetto jQuery del DOM recuperato
    var $grigliaHTML = $(grigliaHTML);

    var rgMobileForm = $grigliaHTML.find("div.rgMobileMenu.rgMobileFilterForm > div.rgMobileForm");

    if (rgMobileForm != null && rgMobileForm.length > 0) {

        var rgMobileFormInnerHeight = rgMobileForm.prop('scrollHeight');
        var rgCommandRowHeight = $grigliaHTML.find("div.rgMobileMenu.rgMobileFilterForm > div.rgCommandRow").height();

        var somma = rgMobileFormInnerHeight + rgCommandRowHeight;
        if ($grigliaHTML.height() < somma) {
            griglia.get_element().style.height = somma + "px";
            griglia.repaint();
        }
    }
}