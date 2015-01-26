<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>Database Decompiler</title>
    <link type="text/css" rel="stylesheet" href="/Styles/MainStyles.css" />
</head>
<body>
    <script type="text/javascript" src="/Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/SharedScript.js"></script>
    <script type="text/javascript" src="/Scripts/DownScripting.js"></script>
    
    <div class="tabs-row">
        <span class="tab active-tab" id="down">Down Compile</span>
        <!--<span class="tab passive-tab" id="up">Up Compile</span>-->
    </div>
    <div class="panels">
        <div class="content-panel active-panel" id="content-down">
            <span class="field-title">Server Address:</span>  <input type="text" id="txtDownServer" /><br />
            <span class="field-title">Database Name:</span>  <input type="text" id="txtDownDBName" /><br />
            <span class="field-title">Username:</span> <input type="text" id="txtDownUserName" /><br />
            <span class="field-title">Password:</span> <input type="text" id="txtDownPassword" /><br />

            <a class="form-button" id="lnkDownSubmit" href="javascript:void(0);">Generate Content</a>
            <a class="form-button" id="lnkDownClear" href="javascript:void(0);">Clear</a>

            <div class="status-area" id="down-status"></div>
        </div>
        <!--<div class="content-panel passive-panel" id="content-up">
            <span class="field-title">Server Address:</span>  <input type="text" id="txtUpServer" /><br />
            <span class="field-title">Username:</span> <input type="text" id="txtUpUserName" /><br />
            <span class="field-title">Password:</span> <input type="text" id="txtUpPassword" /><br />
            <span class="field-title">Files To Generate:</span>

            <a class="form-button" id="lnkUpSubmit" href="javascript:void(0);">Submit</a>
            <a class="form-button" id="lnkUpClear" href="javascript:void(0);">Clear</a>

            <div class="status-area"></div>
        </div>-->
    </div>
</body>
</html>
