jQuery(document).ready(function() {
    

    $("select#battery_id").hide();
    $("#battery_label").hide();

    $("select#column_id").hide();
    $("#column_label").hide();

    $("select#elevator_id").hide();
    $("#elevator_label").hide();
    
    $('select#building_id').change(function() {
        hideWrongChildren("elevator_id", $(this).val())
        if ($(this).children().attr("name") === undefined) {
            
            $("select#battery_id").prop('selectedIndex', 0)
            $("select#column_id").prop('selectedIndex', 0)
            $("select#elevator_id").prop('selectedIndex', 0)
        }  
        console.log("BUILDING TRIGGERED")
        ShowHide();
    });

    $('select#battery_id').change(function() {
        hideWrongChildren("elevator_id", $(this).val())

        if ($(this).children().attr("name") === undefined) {
            
            $("select#column_id").prop('selectedIndex', 0)
            $("select#elevator_id").prop('selectedIndex', 0)
        }
        ShowHide();
    });

    $("select#column_id").change(function() {   
        hideWrongChildren("elevator_id", $(this).val())
        if ($(this).children().attr("name") === undefined) {
            
            $("select#elevator_id").prop('selectedIndex', 0)
        }
        ShowHide();
    });
});
    
function hideWrongChildren(idOfSelector, keyOfParent){
    $(`#${idOfSelector} > option`).each(function () {
        $(this).show()
        var parentId = $(this).attr('name')
        if (parentId === undefined) {
            return
        }
        if (parentId != keyOfParent) {
            $(this).hide()
        }
    })
  }


function ShowHide () {
    $("select#building_id").hide();
    $("#building_label").hide();
    $("select#battery_id").hide();
    $("#battery_label").hide();
    $("select#column_id").hide();
    $("#column_label").hide();
    $("select#elevator_id").hide();
    $("#elevator_label").hide();
    $("select#building_id").show();
    $("#building_label").show();
    if ($("select#building_id").val() != "null") {
        //console.log("BUILDING ISNT NULL")
        //console.log()
        $("select#battery_id").show();
        $("#battery_label").show();
        if ($("select#battery_id").val() != "null") {
            //console.log("BATTERY ISNT NULL")
            $("select#column_id").show();
            $("#column_label").show();
            if ($("select#column_id").val() != "null") {
               // console.log("COLUMN ISNT NULL")
                $("select#elevator_id").show();
                $("#elevator_label").show();
            }
        }
    }
    
};