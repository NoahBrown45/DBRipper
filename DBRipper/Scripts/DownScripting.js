$(document).ready(function () {

    $('#lnkDownSubmit').click(function () {
        //Lets clear out any lingering info in our results.
        $('#down-status').html("");

        //Lets grab the components of our connection string.
        var DBName = $("#txtDownDBName").val();
        var DBServer = $("#txtDownServer").val();
        var UserName = $("#txtDownUserName").val();
        var Password = $("#txtDownPassword").val();

        //This is where we store our results.
        var results = "";

        //Double check to see if we have all of the components.
        if (DBName != "" && DBServer != "" && UserName != "" && Password != "") {
            $.ajax({
                type: "POST",
                url: "Index.aspx/Down_Generate",
                data: "{DBName:'" + DBName + "',DBServer:'" + DBServer + ",UserName:'" + UserName + "',Password:'" + Password + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    results = msg;
                },
                failure: function (msg) {
                    alert(msg);
                }
            });
        } else {
            //Let the user know we need all four connection parameters.
            results = "Missing one or more connection parameters, please set all connection parameters before proceeding.";
        }

        $('#down-status').html(results);
    });

});